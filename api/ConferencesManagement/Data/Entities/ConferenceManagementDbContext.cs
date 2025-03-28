using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ConferencesManagementDAO.Data.Entities;

public partial class ConferenceManagementDbContext : DbContext
{
    public ConferenceManagementDbContext()
    {
    }

    public ConferenceManagementDbContext(DbContextOptions<ConferenceManagementDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Conference> Conferences { get; set; }

    public virtual DbSet<ConferenceHostingRegistration> ConferenceHostingRegistrations { get; set; }

    public virtual DbSet<ConferenceRole> ConferenceRoles { get; set; }

    public virtual DbSet<Delegates> Delegates { get; set; }

    public virtual DbSet<DelegateConferenceRole> DelegateConferenceRoles { get; set; }

    public virtual DbSet<Registration> Registrations { get; set; }

    public virtual DbSet<SystemRole> SystemRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SystemRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SystemRo__3214EC077662D07F");

            entity.HasIndex(e => e.Name, "UQ__SystemRo__737584F668A83EC4").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasData(
                new SystemRole { Id = 1, Name = "Admin" },
                new SystemRole { Id = 2, Name = "Delegates" },
                new SystemRole { Id = 3, Name = "User" }
            );
        });

        modelBuilder.Entity<Delegates>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Delegate__3214EC077DD59054");

            entity.HasIndex(e => e.Email, "UQ__Delegate__A9D105347AA945E4").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.AvatarUrl).HasMaxLength(255);
            entity.Property(e => e.Biography).HasColumnType("text");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.IsConfirmed).HasDefaultValue(false);
            entity.Property(e => e.Nationality).HasMaxLength(100);
            entity.Property(e => e.Organization).HasMaxLength(255);
            entity.Property(e => e.PassportNumber).HasMaxLength(50);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Position).HasMaxLength(255);

            entity.HasMany(d => d.Roles).WithMany(p => p.Delegates)
                .UsingEntity<Dictionary<string, object>>(
                    "DelegateSystemRole",
                    r => r.HasOne<SystemRole>().WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK__DelegateS__RoleI__3B75D760"),
                    l => l.HasOne<Delegates>().WithMany()
                        .HasForeignKey("DelegateId")
                        .HasConstraintName("FK__DelegateS__Deleg__3A81B327"),
                    j =>
                    {
                        j.HasKey("DelegateId", "RoleId").HasName("PK__Delegate__B995E94AE480C135");
                        j.ToTable("DelegateSystemRoles");
                    });

            entity.HasData(
                new Delegates { Id = 3, FullName = "Admin", Email = "admin@example.com", Phone = "0374567952", PasswordHash = "$2a$11$zpU6itqMV2Pln7ltFIbBKuHx7EZYgzfbMiGx660rCSJ9Hspdia5t2", Organization = "CM", Position = "Admin", Gender = "Male", DateOfBirth = new DateOnly(2001, 7, 15), Biography = "A passionate software engineer with over 5 years of experience in .NET development.", IsConfirmed = true, CreatedAt = new DateTime(2025, 3, 7, 18, 30, 29), },
                new Delegates { Id = 10, FullName = "Duy Binh", Email = "duyb2@example.com", Phone = "023434141", PasswordHash = "$2a$11$/.qwjxNLQQWw/ozh5JYKRO77YxhTrDq0ao7PGHxmVEGhvEZO3.2vy", Organization = "FPT Software", Position = "Software Engineer", Gender = "Male", DateOfBirth = new DateOnly(1993, 7, 15), Nationality = "Vietnam", Address = "123 Hoàn Kiếm, Hà Nội, Vietnam", PassportNumber = "A1234567", Biography = "A passionate software engineer with over 5 years of experience in .NET development.", IsConfirmed = true, CreatedAt = new DateTime(2025, 3, 8, 3, 5, 43) },
                new Delegates { Id = 40, FullName = "Trung Top", Email = "trungtop@gmail.com", Phone = "0374567952", PasswordHash = "$2a$11$W6qQPUmqKMEZKhBQg/NEDusyY7d8xJ8GoXPlz21mAQ8g8sTGtv40.", Organization = "FPTU", Position = "BE", Gender = "Male", DateOfBirth = new DateOnly(2001, 1, 23), Nationality = "Viet Nam", Address = "HN", IsConfirmed = true, CreatedAt = new DateTime(2025, 3, 11, 14, 33, 35) },
                new Delegates { Id = 41, FullName = "Trung Top 2", Email = "trungtop2@gmail.com", Phone = "012345678", PasswordHash = "$2a$11$L5Ame2hdA7p1nLI5LzVvZu8IYcY1MWOHD55x9S7LijDr8kBYWJJGy", Organization = "FPTU", Position = "BE", Gender = "Male", DateOfBirth = new DateOnly(2001, 3, 16), Nationality = "VN", Address = "HN", IsConfirmed = true, CreatedAt = new DateTime(2025, 3, 11, 14, 42, 25) }
                );

        });

        modelBuilder.Entity<Conference>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Conferen__3214EC07AFBF0F38");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Location).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.HasOne(d => d.HostByNavigation).WithMany(p => p.Conferences).HasForeignKey(d => d.HostBy);
        });

        modelBuilder.Entity<ConferenceHostingRegistration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Conferen__3214EC0743A55694");

            entity.ToTable("ConferenceHostingRegistration");

            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.Location).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50).HasDefaultValue("Pending");


            entity.HasOne(d => d.Register).WithMany(p => p.ConferenceHostingRegistrations)
                .HasForeignKey(d => d.RegisterId)
                .HasConstraintName("FK__Conferenc__Regis__4AB81AF0");
        });

        modelBuilder.Entity<ConferenceRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Conferen__3214EC07910A345E");

            entity.HasIndex(e => e.Name, "UQ__Conferen__737584F6F6FDD6D5").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(100);
            entity.HasData(
                new ConferenceRole { Id = 1, Name = "Ban tổ chức" },
                new ConferenceRole { Id = 2, Name = "Diễn giả" },
                new ConferenceRole { Id = 3, Name = "Khách mời" }
            );
        });

        modelBuilder.Entity<DelegateConferenceRole>(entity =>
        {
            entity.HasKey(e => new { e.DelegateId, e.ConferenceId, e.RoleId }).HasName("PK__Delegate__3E19E032B684A715");

            entity.HasOne(d => d.Conference).WithMany(p => p.DelegateConferenceRoles)
                .HasForeignKey(d => d.ConferenceId)
                .HasConstraintName("FK__DelegateC__Confe__37A5467C");

            entity.HasOne(d => d.Delegate).WithMany(p => p.DelegateConferenceRoles)
                .HasForeignKey(d => d.DelegateId)
                .HasConstraintName("FK__DelegateC__Deleg__38996AB5");

            entity.HasOne(d => d.Role).WithMany(p => p.DelegateConferenceRoles)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__DelegateC__RoleI__398D8EEE");
        });

        modelBuilder.Entity<Registration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Registra__3214EC07B791C336");

            entity.Property(e => e.RegisteredAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.Conference).WithMany(p => p.Registrations)
                .HasForeignKey(d => d.ConferenceId)
                .HasConstraintName("FK__Registrat__Confe__3C69FB99");

            entity.HasOne(d => d.Delegate).WithMany(p => p.Registrations)
                .HasForeignKey(d => d.DelegateId)
                .HasConstraintName("FK__Registrat__Deleg__3D5E1FD2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
