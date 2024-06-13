using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using BusinessObject.Entities.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Utility.Enum;
using Utility.Helpers;

namespace BusinessObject.Entities.Identity;

public class UserEntity : IdentityUser<int>
{
    public UserEntity()
    {
        CreatedTime = LastUpdatedTime = DateTimeOffset.Now;
    }

    public string? FullName { get; set; }
    public string? Address { get; set; }
    public string? Avatar { get; set; }
    public DateTimeOffset? BirthDate { get; set; }

    public virtual ICollection<UserRoleEntity> UserRoles { get; set; }

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; }

    // Base Property
    public string? CreatedBy { get; set; }
    public string? LastUpdatedBy { get; set; }
    public string? DeletedBy { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
    public DateTimeOffset LastUpdatedTime { get; set; }
    public DateTimeOffset? DeletedTime { get; set; }

    // Identity Property
    public DateTimeOffset? Verified { get; set; }
    public string? OTP { get; set; }
    public DateTimeOffset? OTPExpired { get; set; }
    public bool IsActive => PhoneNumberConfirmed;
    public override bool PhoneNumberConfirmed => Verified.HasValue;

    [NotMapped]
    public override string SecurityStamp { get => base.SecurityStamp; set => base.SecurityStamp = value; }
    [NotMapped]
    public override string ConcurrencyStamp { get => base.ConcurrencyStamp; set => base.ConcurrencyStamp = value; }

    // for customer
    public virtual ICollection<Pet> Pets { get; set; }

    // for vet
    public virtual ICollection<MedicalRecord>? MedicalRecords { get; set; }
}

public class RoleEntity : IdentityRole<int>
{
    public virtual ICollection<UserRoleEntity> UserRoles { get; set; }

    [NotMapped]
    public override string ConcurrencyStamp { get => base.ConcurrencyStamp; set => base.ConcurrencyStamp = value; }

    [NotMapped]
    public override string NormalizedName { get => base.NormalizedName; set => base.NormalizedName = value; }
}

public class UserRoleEntity : IdentityUserRole<int>
{
    public virtual UserEntity User { get; set; }
    public virtual RoleEntity Role { get; set; }
}

public class RefreshToken : BaseEntity
{
    [ForeignKey(nameof(UserId))]
    public UserEntity User { get; set; }
    public int UserId { get; set; }
    public string Token { get; set; }
    public DateTimeOffset Expires { get; set; }
    public bool IsExpired => CoreHelper.SystemTimeNow >= Expires;
    public bool IsActive => !IsExpired;
}

