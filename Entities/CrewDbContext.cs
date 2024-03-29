﻿using System;
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

    public virtual DbSet<GroupsPost> GroupsPosts { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserEducation> UserEducations { get; set; }

    public virtual DbSet<UsersGroup> UsersGroups { get; set; }

    public virtual DbSet<UsersNotification> UsersNotifications { get; set; }

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

        modelBuilder.Entity<GroupsPost>(entity =>
        {
            entity.ToTable("Groups_posts");

            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("Create_date");
            entity.Property(e => e.CreatedBy).HasColumnName("Created_by");
            entity.Property(e => e.DeleteDate)
                .HasColumnType("datetime")
                .HasColumnName("Delete_date");
            entity.Property(e => e.EditDate)
                .HasColumnType("datetime")
                .HasColumnName("Edit_date");
            entity.Property(e => e.GroupId).HasColumnName("Group_id");
            entity.Property(e => e.TaggedUsers).HasColumnName("Tagged_users");
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.GroupsPosts)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_Groups_posts_user");

            entity.HasOne(d => d.Group).WithMany(p => p.GroupsPosts)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("FK_Groups_posts_group");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.Property(e => e.Body).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Subject).HasMaxLength(100);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("Create_date");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.User).WithMany(p => p.Sessions)
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

        modelBuilder.Entity<UserEducation>(entity =>
        {
            entity.ToTable("User_educations");

            entity.Property(e => e.AdditionalInfo)
                .IsUnicode(false)
                .HasColumnName("Additional_info");
            entity.Property(e => e.DateFrom).HasColumnName("Date_from");
            entity.Property(e => e.DateTo).HasColumnName("Date_to");
            entity.Property(e => e.Degree)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Field).IsUnicode(false);
            entity.Property(e => e.SchoolName)
                .IsUnicode(false)
                .HasColumnName("School_name");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.User).WithMany(p => p.UserEducations)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_User_education");
        });

        modelBuilder.Entity<UsersGroup>(entity =>
        {
            entity.ToTable("Users_groups");

            entity.Property(e => e.GroupId).HasColumnName("Group_id");
            entity.Property(e => e.RoleId).HasColumnName("Role_id");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.Group).WithMany(p => p.UsersGroups)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("FK_Users_groups_groups");

            entity.HasOne(d => d.Role).WithMany(p => p.UsersGroups)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_Users_groups_roles");

            entity.HasOne(d => d.User).WithMany(p => p.UsersGroups)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Users_groups_user");
        });

        modelBuilder.Entity<UsersNotification>(entity =>
        {
            entity.ToTable("Users_notifications");

            entity.Property(e => e.Details)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.NotifyId).HasColumnName("Notify_id");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.Notify).WithMany(p => p.UsersNotifications)
                .HasForeignKey(d => d.NotifyId)
                .HasConstraintName("FK_Users_notifications_notifications");

            entity.HasOne(d => d.User).WithMany(p => p.UsersNotifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Users_notifications_users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
