﻿using BusinessObject.Entities;
using BusinessObject.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection.Emit;
using Utility.Enum;

namespace Repository;

public class AppDbContext : IdentityDbContext<UserEntity, RoleEntity, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public AppDbContext()
    {

    }

    public DbSet<Pet> Pets { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<MedicalRecord> MedicalRecords { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<TimeTable> TimeTables { get; set; }
    public DbSet<Cage> Cage { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Configuration> Configurations { get; set; }
    public DbSet<TransactionDetail> TransactionDetails { get; set; }
    public DbSet<UserRoleEntity> UserRoleEntity { get; set; }
    public DbSet<RoleEntity> Role { get; set; }

    private string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .Build();
        var strConn = config["ConnectionStrings:DefaultConnection"];
        return strConn;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(GetConnectionString());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserEntity>(b =>
        {
            b.Property(u => u.Id)
                .ValueGeneratedOnAdd();
            // Each User can have many entries in the UserRole join table
            b.HasMany(e => e.UserRoles)
                .WithOne(e => e.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        });

        modelBuilder.Entity<RoleEntity>(b =>
        {
            // Each Role can have many entries in the UserRole join table
            b.HasMany(e => e.UserRoles)
                .WithOne(e => e.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
        });

        var roles = new List<RoleEntity>
        {
            new()
            {
                Id = 1,
                Name = UserRole.Admin.ToString(),
                NormalizedName = UserRole.Admin.ToString().ToUpper()
            },
            new()
            {
                Id = 2,
                Name = UserRole.Staff.ToString(),
                NormalizedName = UserRole.Staff.ToString().ToUpper()
            },
            new()
            {
                Id = 3,
                Name = UserRole.Vet.ToString(),
                NormalizedName = UserRole.Vet.ToString().ToUpper()
            },
            new()
            {
                Id = 4,
                Name = UserRole.Customer.ToString(),
                NormalizedName = UserRole.Customer.ToString().ToUpper()
            },
        };
        modelBuilder.Entity<RoleEntity>().HasData(roles);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            if (tableName.StartsWith("AspNet"))
            {
                entityType.SetTableName(tableName.Substring(6));
            }
        }
    }
}