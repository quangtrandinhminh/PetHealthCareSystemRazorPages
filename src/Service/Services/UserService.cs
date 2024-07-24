using BusinessObject.DTO.User;
using BusinessObject.DTO.Vet;
using BusinessObject.Entities.Identity;
using BusinessObject.Mapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository.Interfaces;
using Serilog;
using Service.IServices;
using System.Text.RegularExpressions;
using Repository.Extensions;
using Utility.Constants;
using Utility.Enum;
using Utility.Exceptions;
using Utility.Helpers;

namespace Service.Services;

public class UserService(IServiceProvider serviceProvider) : IUserService
{
    private readonly IUserRepository _userRepository = serviceProvider.GetRequiredService<IUserRepository>();
    private readonly MapperlyMapper _mapper = serviceProvider.GetRequiredService<MapperlyMapper>();
    private readonly RoleManager<RoleEntity> _roleManager = serviceProvider.GetRequiredService<RoleManager<RoleEntity>>();
    private readonly UserManager<UserEntity> _userManager = serviceProvider.GetRequiredService<UserManager<UserEntity>>();
    private readonly ILogger _logger = Log.Logger;
    private readonly SignInManager<UserEntity> _signInManager = serviceProvider.GetRequiredService<SignInManager<UserEntity>>();

    public async Task<PaginatedList<UserResponseDto>> GetAllUsersAsync(int pageNumber, int pageSize)
    {
        var users = _userRepository.GetAllWithCondition(u => u.DeletedTime == null);

        if (users == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND, ResponseMessageConstantsUser.USER_NOT_FOUND
                , StatusCodes.Status404NotFound);
        }

        var response = _mapper.Map(users);
        var paginatedList = await PaginatedList<UserResponseDto>.CreateAsync(response, pageNumber, pageSize);
        return paginatedList;
    }

    public async Task<PaginatedList<UserResponseDto>> GetAllUserWithFilter(UserFilterDto filter, int pageNumber,
        int pageSize)
    {
        var users = _userRepository.GetAllWithCondition(u => u.DeletedTime == null);

        if (users == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND, ResponseMessageConstantsUser.USER_NOT_FOUND
                           , StatusCodes.Status404NotFound);
        }

        if (!string.IsNullOrEmpty(filter.Name))
        {
            users = users.Where(u => u.FullName != null && u.FullName.Contains(filter.Name));
        }

        if (!string.IsNullOrEmpty(filter.Email))
        {
            users = users.Where(u => u.Email != null && u.Email.Contains(filter.Email));
        }

        if (!string.IsNullOrEmpty(filter.PhoneNumber))
        {
            users = users.Where(u => u.PhoneNumber != null && u.PhoneNumber.Contains(filter.PhoneNumber));
        }

        if (!string.IsNullOrEmpty(filter.Address))
        {
            users = users.Where(u => u.Address != null && u.Address.Contains(filter.Address));
        }

        if (!string.IsNullOrEmpty(filter.Role))
        {
            users = users.Where(u => u.UserRoles.Any(ur => ur.Role.Name == filter.Role));
        }

        var response = _mapper.Map(users);
        var paginatedList = await PaginatedList<UserResponseDto>.CreateAsync(response, pageNumber, pageSize);
        return paginatedList;
    }

    public async Task<IList<UserResponseDto>> GetAllUsersByRoleAsync(UserRole role)
    {
        switch (role)
        {
            case UserRole.Admin:
                return await GetAdminsAsync();
            case UserRole.Staff:
                return await GetStaffAsync();
            case UserRole.Vet:
                return await GetVetsAsync();
            case UserRole.Customer:
                return await GetCustomersAsync();
            default:
                throw new AppException(ResponseCodeConstants.INVALID_INPUT, ResponseMessageIdentity.ROLE_INVALID, StatusCodes.Status400BadRequest);
        }
    }

    private async Task<IList<UserResponseDto>> GetAdminsAsync()
    {
        var admins = await _userManager.GetUsersInRoleAsync(UserRole.Admin.ToString());

        if (admins == null || admins.Count == 0)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND, ResponseMessageConstantsUser.ADMIN_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        var response = _mapper.Map(admins);

        foreach (var vet in response)
        {
            vet.Role = UserRole.Admin.ToString();
        }

        return response;
    }

    private async Task<IList<UserResponseDto>> GetVetsAsync()
    {
        var vets = await _userManager.GetUsersInRoleAsync(UserRole.Vet.ToString());

        if (vets == null || vets.Count == 0)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND, ResponseMessageConstantsUser.VET_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        var response = _mapper.Map(vets);

        foreach (var vet in response)
        {
            vet.Role = UserRole.Vet.ToString();
        }

        return response;
    }

    public async Task<UserResponseDto> GetVetByIdAsync(int id)
    {
        var vets = await _userManager.GetUsersInRoleAsync(UserRole.Vet.ToString());

        if (vets == null || vets.Count == 0)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND, ResponseMessageConstantsUser.VET_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        var response = _mapper.Map(vets);

        foreach (var vet in response)
        {
            vet.Role = UserRole.Vet.ToString();
        }

        var vetResponse = response.Where(e => e.Id == id).FirstOrDefault();

        if (vetResponse == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND, ResponseMessageConstantsUser.VET_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        return vetResponse;
    }

    private async Task<IList<UserResponseDto>> GetStaffAsync()
    {
        var staffs = await _userManager.GetUsersInRoleAsync(UserRole.Staff.ToString());

        if (staffs == null || staffs.Count == 0)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND, ResponseMessageConstantsUser.STAFF_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        var response = _mapper.Map(staffs);

        foreach (var vet in response)
        {
            vet.Role = UserRole.Staff.ToString();
        }

        return response;
    }

    public async Task<IList<UserResponseDto>> GetCustomersAsync()
    {
        var customers = await _userManager.GetUsersInRoleAsync(UserRole.Customer.ToString());

        if (customers == null || customers.Count == 0)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND, ResponseMessageConstantsUser.CUSTOMER_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        var response = _mapper.Map(customers);

        foreach (var vet in response)
        {
            vet.Role = UserRole.Customer.ToString();
        }

        return response;
    }

    public async Task UpdateUserAsync(UserUpdateRequestDto dto, int updatedById)
    {
        _logger.Information("Update user: {@dto} by {updatedById}", dto, updatedById);
        var user = await _userRepository.GetSingleAsync(u => u.Id == updatedById);
        if (user == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND, ResponseMessageConstantsUser.USER_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        var updatedUser = await _userRepository.GetSingleAsync(u => u.Id == dto.Id);
        if (updatedUser == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND, ResponseMessageConstantsUser.USER_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        _mapper.Map(dto, updatedUser);
        updatedUser.LastUpdatedBy = user.FullName;
        updatedUser.LastUpdatedTime = CoreHelper.SystemTimeNow;

        await _userRepository.UpdateAsync(updatedUser);
    }

    public async Task<UserResponseDto> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetSingleAsync(e => e.Id == id);

        return _mapper.UserToUserResponseDto(user);
    }

    public Task DeleteUserAsync(int id)
    {
        throw new NotImplementedException();
    }
}