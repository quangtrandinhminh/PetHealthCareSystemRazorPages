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
using Utility.Constants;
using Utility.Enum;
using Utility.Exceptions;

namespace Service.Services;

public class UserService(IServiceProvider serviceProvider) : IUserService
{
    private readonly IUserRepository _userRepository = serviceProvider.GetRequiredService<IUserRepository>();
    private readonly MapperlyMapper _mapper = serviceProvider.GetRequiredService<MapperlyMapper>();
    private readonly RoleManager<RoleEntity> _roleManager = serviceProvider.GetRequiredService<RoleManager<RoleEntity>>();
    private readonly UserManager<UserEntity> _userManager = serviceProvider.GetRequiredService<UserManager<UserEntity>>();
    private readonly ILogger _logger = Log.Logger;
    private readonly SignInManager<UserEntity> _signInManager = serviceProvider.GetRequiredService<SignInManager<UserEntity>>();

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

    public Task CreateUserAsync(UserCreateRequestDto dto)
    {
        throw new NotImplementedException();
    }

    public Task UpdateUserAsync(UserUpdateRequestDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<UserResponseDto> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task DeleteUserAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task CreateVetAsync(VetRequestDto dto)
    {
        _logger.Information("Register new user: {@dto}", dto);
        // get user by name
        var validateUser = await _userManager.FindByNameAsync(dto.UserName);
        if (validateUser != null)
        {
            throw new AppException(ResponseCodeConstants.EXISTED, ResponseMessageIdentity.EXISTED_USER, StatusCodes.Status400BadRequest);
        }

        var existingUserWithEmail = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUserWithEmail != null)
        {
            throw new AppException(ResponseCodeConstants.EXISTED, ResponseMessageIdentity.EXISTED_EMAIL, StatusCodes.Status400BadRequest);
        }

        var existingUserWithPhone = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == dto.PhoneNumber);
        if (existingUserWithPhone != null)
        {
            throw new AppException(ResponseCodeConstants.EXISTED, ResponseMessageIdentity.EXISTED_PHONE, StatusCodes.Status400BadRequest);
        }

        if (!string.IsNullOrEmpty(dto.PhoneNumber) && !Regex.IsMatch(dto.PhoneNumber, @"^\d{10}$"))
        {
            throw new AppException(ResponseCodeConstants.INVALID_INPUT, ResponseMessageIdentity.PHONENUMBER_INVALID, StatusCodes.Status400BadRequest);
        }

        if (dto.Password != dto.ConfirmPassword)
        {
            throw new AppException(ResponseCodeConstants.INVALID_INPUT, ResponseMessageIdentity.PASSWORD_NOT_MATCH, StatusCodes.Status400BadRequest);
        }

        try
        {
            var account = _mapper.Map(dto);
            account.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            account.SecurityStamp = Guid.NewGuid().ToString();
            await _userRepository.CreateAsync(account);
            await _userManager.AddToRoleAsync(account, "Vet");
        }
        catch (Exception e)
        {
            throw new AppException(ResponseCodeConstants.FAILED, e.Message, StatusCodes.Status400BadRequest);
        }
    }
}