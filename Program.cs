using CrewBackend.Entities;
using CrewBackend.Interfaces;
using Microsoft.EntityFrameworkCore;
using CrewBackend.Services;
using CrewBackend.Middlewares;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.CookiePolicy;
using CrewBackend.Models;
using CrewBackend.Factories;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = true,
        ValidateIssuer = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration.GetSection("Jwt:Key").Value!)),
        ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Value,
        ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value
    };
});
//builder.Services.Configure<CookiePolicyOptions>(options =>
//{
//    options.MinimumSameSitePolicy = SameSiteMode.None;
//    options.HttpOnly = HttpOnlyPolicy.None;
//    options.Secure = CookieSecurePolicy.None;
//});
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
//dotnet ef dbcontext scaffold "Name=ConnectionStrings:CrewDB" Microsoft.EntityFrameworkCore.SqlServer --output-dir Entities --force
builder.Services.AddDbContext<CrewDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CrewDB")));
//builder.Services.AddDbContextFactory<CrewDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("CrewDB")));

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGroupsService, GroupsService>();
builder.Services.AddScoped<IUserContextInfo, UserContextInfo>();

builder.Services.AddScoped<IGroupObserverFactory, GroupObserverFactory>();
builder.Services.AddScoped<IGroupNotificatorFactory, GroupNotificatorFactory>();

builder.Services.AddScoped<IEmailAccountService, EmailAccountService>();
builder.Services.AddScoped<IEmailNotificationService, EmailNotificationService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "ReactOrigin",
    policy =>
    {
        policy.WithOrigins(["http://localhost:5173", "https://localhost:5173"])
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .SetIsOriginAllowed(h => true);
        //.WithExposedHeaders("Myheader");
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("ReactOrigin");



app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//app.UseMiddleware<SessionMiddleware>();
app.MapControllers();

app.Run();
