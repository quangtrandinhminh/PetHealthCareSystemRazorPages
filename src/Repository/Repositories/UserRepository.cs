using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BusinessObject.Entities.Identity;
using DataAccessLayer;
using DataAccessLayer.DAO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Repository.Base;
using Repository.Interfaces;
using Utility.Enum;

namespace Repository.Repositories
{
    public class UserRepository : UserStore<UserEntity, RoleEntity, AppDbContext, int>, IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IdentityResult> CreateAsync(UserEntity userEntity) => await UserDao.CreateAsync(userEntity);

        public async Task<IdentityResult> UpdateAsync(UserEntity userEntity) => await UserDao.UpdateAsync(userEntity);

        public async Task<UserEntity> GetUserByEmail(string email) => await UserDao.GetUserByEmailAsync(email);

        public async Task<UserEntity> GetUserByUserName(string userName) => await UserDao.GetUserByUserNameAsync(userName);

        public async Task<UserEntity?> GetSingleAsync(Expression<Func<UserEntity, bool>>? predicate = null, params Expression<Func<UserEntity, object>>[] includeProperties) => await UserDao.GetSingleAsync(predicate, includeProperties);
    }
}