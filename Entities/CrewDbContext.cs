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

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:CrewDB");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
