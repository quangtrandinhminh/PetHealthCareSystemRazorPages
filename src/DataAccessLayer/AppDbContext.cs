using BusinessObject;
using BusinessObject.Entities;
using BusinessObject.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Utility.Enum;

namespace DataAccessLayer;

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
    public DbSet<AppointmentPet> AppointmentPets { get; set; }
    public DbSet<MedicalRecord> MedicalRecords { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<MedicalItem> MedicalItems { get; set; }
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

        modelBuilder.Entity<UserEntity>(b =>
        {
            b.HasMany(e => e.Pets)
                .WithOne(e => e.Owner)
                .HasForeignKey(ur => ur.OwnerID);
        });

        modelBuilder.Entity<Pet>()
            .HasOne(p => p.Owner)
            .WithMany(u => u.Pets)
            .HasForeignKey(p => p.OwnerID);

        modelBuilder.Entity<AppointmentPet>()
            .HasKey(ap => new { ap.AppointmentId, ap.PetId });

        modelBuilder.Entity<AppointmentPet>()
            .HasOne(ap => ap.Appointment)
            .WithMany(a => a.AppointmentPets)
            .HasForeignKey(ap => ap.AppointmentId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<AppointmentPet>()
            .HasOne(ap => ap.Pet)
            .WithMany(p => p.AppointmentPets)
            .HasForeignKey(ap => ap.PetId)
            .OnDelete(DeleteBehavior.NoAction);

        var admin = new UserEntity
        {
            Id = 1,
            UserName = "admin",
            FullName = "Admin User",
            NormalizedUserName = "ADMIN",
            Email = "admin@email.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("12345678"),
            SecurityStamp = Guid.NewGuid().ToString(),
        };
        modelBuilder.Entity<UserEntity>().HasData(admin);

        var staff = new UserEntity
        {
            Id = 2,
            UserName = "staff",
            FullName = "Staff User",
            NormalizedUserName = "staff",
            Email = "staff@email.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("12345678"),
            SecurityStamp = Guid.NewGuid().ToString(),
        };
        modelBuilder.Entity<UserEntity>().HasData(staff);

        var vet1 = new UserEntity
        {
            Id = 3,
            FullName = "John Doe",
            UserName = "vet1",
            NormalizedUserName = "JOHNDOE",
            Address = "123 Main St",
            Email = "johndoe@example.com",
            BirthDate = DateTimeOffset.Parse("1985-06-15T00:00:00+00:00"),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("12345678"),
            SecurityStamp = Guid.NewGuid().ToString(),
        };
        modelBuilder.Entity<UserEntity>().HasData(vet1);

        var vet2 = new UserEntity
        {
            Id = 4,
            FullName = "Jane Smith",
            UserName = "vet2",
            NormalizedUserName = "JANESMITH",
            Address = "456 Elm St",
            Email = "janesmith@example.com",
            BirthDate = DateTimeOffset.Parse("1990-09-20T00:00:00+00:00"),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("12345678"),
            SecurityStamp = Guid.NewGuid().ToString(),
        };
        modelBuilder.Entity<UserEntity>().HasData(vet2);

        var vet3 = new UserEntity
        {
            Id = 5,
            FullName = "Alice Johnson",
            UserName = "vet3",
            NormalizedUserName = "ALICEJOHNSON",
            Address = "789 Pine St",
            Email = "alicejohnson@example.com",
            BirthDate = DateTimeOffset.Parse("1978-03-25T00:00:00+00:00"),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("12345678"),
            SecurityStamp = Guid.NewGuid().ToString(),
        };
        modelBuilder.Entity<UserEntity>().HasData(vet3);

        var cus1 = new UserEntity
        {
            Id = 6,
            FullName = "Tran Dinh Minh Quang",
            UserName = "cus1",
            PhoneNumber = "0123456789",
            NormalizedUserName = "CUS1",
            Address = "123 Main St",
            Email = "quangtdmse171391@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("12345678"),
            SecurityStamp = Guid.NewGuid().ToString(),
        };
        modelBuilder.Entity<UserEntity>().HasData(cus1);

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

        var adminUserRole = new UserRoleEntity
        {
            UserId = admin.Id,
            RoleId = 1
        };
        modelBuilder.Entity<UserRoleEntity>().HasData(adminUserRole);

        var staffUserRole = new UserRoleEntity
        {
            UserId = staff.Id,
            RoleId = 2
        };
        modelBuilder.Entity<UserRoleEntity>().HasData(staffUserRole);

        var vet1UserRole = new UserRoleEntity
        {
            UserId = vet1.Id,
            RoleId = 3
        };
        modelBuilder.Entity<UserRoleEntity>().HasData(vet1UserRole);

        var vet2UserRole = new UserRoleEntity
        {
            UserId = vet2.Id,
            RoleId = 3
        };
        modelBuilder.Entity<UserRoleEntity>().HasData(vet2UserRole);

        var vet3UserRole = new UserRoleEntity
        {
            UserId = vet3.Id,
            RoleId = 3
        };
        modelBuilder.Entity<UserRoleEntity>().HasData(vet3UserRole);

        var cus1UserRole = new UserRoleEntity
        {
            UserId = cus1.Id,
            RoleId = 4
        };
        modelBuilder.Entity<UserRoleEntity>().HasData(cus1UserRole);

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