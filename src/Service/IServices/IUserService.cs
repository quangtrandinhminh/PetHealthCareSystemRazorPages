using BusinessObject.DTO.User;
using BusinessObject.DTO.Vet;
using Repository.Extensions;
using Utility.Enum;

namespace Service.IServices;

public interface IUserService
{
    Task<PaginatedList<UserResponseDto>> GetAllUsersAsync(int pageNumber, int pageSize);
    Task<IList<UserResponseDto>> GetAllUsersByRoleAsync(UserRole role);
    Task UpdateUserAsync(UserUpdateRequestDto dto, int updatedById);
    Task<UserResponseDto> GetByIdAsync(int id);
    Task DeleteUserAsync(int id);
    Task<UserResponseDto> GetVetByIdAsync(int id);
}