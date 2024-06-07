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
using Repository;
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

        public static async Task<UserEntity> GetUserByUserNameAsync(string username)
        {

            return await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
        }

        public static async Task<UserEntity> GetUserByEmailAsync(string email)
        {

            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}