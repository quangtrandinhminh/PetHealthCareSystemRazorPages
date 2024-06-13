using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BusinessObject.DTO.User;
using BusinessObject.Entities.Identity;
using DataAccessLayer.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Utility.Enum;

namespace DataAccessLayer.DAO
{
    public class UserDao
    {
        private static readonly AppDbContext _context = new();
        private static UserManager<UserEntity> _userManager;
        private static SignInManager<UserEntity> _signinManager;


        public static async Task<IdentityResult> CreateAsync(UserEntity user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }

        public static async Task<IdentityResult> UpdateAsync(UserEntity user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }

        public static async Task<UserEntity> GetUserByUserNameAsync(string username)
        {

            return await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
        }

        public static async Task<UserEntity> GetUserByEmailAsync(string email)
        {

            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public static async Task<UserEntity?> GetSingleAsync(Expression<Func<UserEntity, bool>>? predicate = null,
            params Expression<Func<UserEntity, object>>[] includeProperties)
        => await Get(predicate, includeProperties).FirstOrDefaultAsync();

        public static IQueryable<UserEntity> Get(Expression<Func<UserEntity, bool>>? predicate = null, params Expression<Func<UserEntity, object>>[] includeProperties)
        {
            IQueryable<UserEntity> reault = _context.Users.AsNoTracking();
            if (predicate != null)
            {
                reault = reault.Where(predicate);
            }

            includeProperties = includeProperties?.Distinct().ToArray();
            if (includeProperties?.Any() ?? false)
            {
                Expression<Func<UserEntity, object>>[] array = includeProperties;
                foreach (Expression<Func<UserEntity, object>> navigationPropertyPath in array)
                {
                    reault = reault.Include(navigationPropertyPath);
                }
            }

            return reault.Where(x => x.DeletedTime == null);
        }
    }
}