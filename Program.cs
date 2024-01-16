using CrewBackend.Entities;
using CrewBackend.Interfaces;
using Microsoft.EntityFrameworkCore;
using CrewBackend.Services;
using CrewBackend.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//dotnet ef dbcontext scaffold "Name=ConnectionStrings:CrewDB" Microsoft.EntityFrameworkCore.SqlServer --output-dir Entities --force
builder.Services.AddDbContextFactory<CrewDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CrewDB")));

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "ReactOrigin",
    policy =>
    {
        policy.WithOrigins("http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
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

app.UseAuthorization();
app.UseMiddleware<SessionMiddleware>();
app.MapControllers();

app.Run();
