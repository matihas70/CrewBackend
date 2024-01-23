using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CrewBackend.Entities;

public partial class CrewDbContext : DbContext
{
    public CrewDbContext()
    {
    }

    public CrewDbContext(DbContextOptions<CrewDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActivateAccountRequest> ActivateAccountRequests { get; set; }

    public virtual DbSet<AppSetting> AppSettings { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:CrewDB");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActivateAccountRequest>(entity =>
        {
            entity.ToTable("Activate_account_requests");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ExpirationDate)
                .HasColumnType("datetime")
                .HasColumnName("Expiration_date");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.User).WithMany(p => p.ActivateAccountRequests)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_User_activate_request");
        });

        modelBuilder.Entity<AppSetting>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("App_settings");

            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.EmailHost)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Email_host");
            entity.Property(e => e.EmailLogin)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Email_login");
            entity.Property(e => e.EmailPassword)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Email_password");
            entity.Property(e => e.EmailPort).HasColumnName("Email_port");
            entity.Property(e => e.EmailSsl).HasColumnName("Email_ssl");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("Create_date");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_by");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Picture).IsUnicode(false);
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("Create_date");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Session_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Callname)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("Create_date");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Picture).IsUnicode(false);
            entity.Property(e => e.Surname)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
