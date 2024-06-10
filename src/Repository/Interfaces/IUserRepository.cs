using BusinessObject.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Repository.Interfaces
{
    public interface IUserRepository : IUserStore<UserEntity>
    {
        Task<IdentityResult> CreateAsync(UserEntity userEntity);
        Task<IdentityResult> UpdateAsync(UserEntity userEntity);
        Task<UserEntity> GetUserByEmail(string email);
        Task<UserEntity> GetUserByUserName(string userName);
    }
}