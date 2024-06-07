using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using BusinessObject.DTO.User;
using BusinessObject.DTO.Vet;
using BusinessObject.Entities.Identity;
using BusinessObject.Mapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using Repository.Interface;
using Repository.Repositories;
using Serilog;
using Service.IServices;
using Service.Utils;
using Utility.Constants;
using Utility.Enum;
using Utility.Exceptions;
using Utility.Helpers;

namespace Service.Services
{
    public class UserService(IServiceProvider serviceProvider) : IUserService
    {
        private readonly IUserRepository _userRepository = serviceProvider.GetRequiredService<IUserRepository>();
        private readonly MapperlyMapper _mapper = serviceProvider.GetRequiredService<MapperlyMapper>();
        private readonly RoleManager<RoleEntity> _roleManager = serviceProvider.GetRequiredService<RoleManager<RoleEntity>>();
        private readonly UserManager<UserEntity> _userManager = serviceProvider.GetRequiredService<UserManager<UserEntity>>();
        private readonly ILogger _logger = Log.Logger;
        private readonly SignInManager<UserEntity> _signInManager = serviceProvider.GetRequiredService<SignInManager<UserEntity>>();

        public async Task Register(RegisterDto dto)
        {
            _logger.Information("Register new user: {@dto}", dto);
            // get user by name
            var validateUser = await _userManager.FindByNameAsync(dto.UserName);
            if (validateUser != null)
            {
                throw new AppException(ResponseCodeConstants.EXISTED, ReponseMessageIdentity.EXISTED_USER, StatusCodes.Status400BadRequest);
            }

            var existingUserWithEmail = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUserWithEmail != null)
            {
                throw new AppException(ResponseCodeConstants.EXISTED, ReponseMessageIdentity.EXISTED_EMAIL, StatusCodes.Status400BadRequest);
            }

            var existingUserWithPhone = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == dto.PhoneNumber);
            if (existingUserWithPhone != null)
            {
                throw new AppException(ResponseCodeConstants.EXISTED, ReponseMessageIdentity.EXISTED_PHONE, StatusCodes.Status400BadRequest);
            }

            if (!string.IsNullOrEmpty(dto.PhoneNumber) && !Regex.IsMatch(dto.PhoneNumber, @"^\d{10}$"))
            {
                throw new AppException(ResponseCodeConstants.INVALID_INPUT, ReponseMessageIdentity.PHONENUMBER_INVALID, StatusCodes.Status400BadRequest);
            }

            if (dto.Password != dto.ConfirmPassword)
            {
                throw new AppException(ResponseCodeConstants.INVALID_INPUT, ReponseMessageIdentity.PASSWORD_NOT_MATCH, StatusCodes.Status400BadRequest);
            }

            try
            {
                var account = _mapper.Map(dto);
                account.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                account.SecurityStamp = Guid.NewGuid().ToString();
                await _userRepository.CreateAsync(account);
                await _userManager.AddToRoleAsync(account, "Customer");
            }
            catch (Exception e)
            {
                throw new AppException(ResponseCodeConstants.FAILED, e.Message, StatusCodes.Status400BadRequest);
            }


            // send sms to phone number here
        }

        public async Task<LoginResponseDto> Authenticate(LoginDto dto)
        {
            _logger.Information("Authenticate user: {@dto}", dto);
            var account = await _userRepository.GetUserByUserName(dto.Username);
            if (account == null || account.DeletedTime != null) 
                throw new AppException(ErrorCode.UserInvalid, ReponseMessageIdentity.INVALID_USER, StatusCodes.Status401Unauthorized);

            // check password
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, account.PasswordHash))
                throw new AppException(ErrorCode.UserPasswordWrong, ReponseMessageIdentity.PASSWORD_WRONG, StatusCodes.Status401Unauthorized);

            try
            {
                var roles = await _userManager.GetRolesAsync(account);
                var token = await GenerateJwtToken(account, roles, 1);
                var refreshToken = GenerateRefreshToken(account.Id, 12);
                var response = _mapper.Map(account);
                response.Token = token;
                response.RefreshToken = refreshToken.Token;
                response.Role = roles;
                return response;
            }
            catch (Exception e)
            {
                throw new AppException(ResponseCodeConstants.FAILED, e.Message, StatusCodes.Status400BadRequest);
            }
        }

        public Task<VetResponseDto> CreateVet(VetRequestDto dto)
        {
            throw new NotImplementedException();
        }

        private async Task<string> GenerateJwtToken(UserEntity loggedUser, IList<string> roles, int hour)
        {
            var claims = new List<Claim>();
            claims.AddRange(await _userManager.GetClaimsAsync(loggedUser));
            // Add role claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));

                // Use RoleManager to find the role and add its claims
                var roleEntity = await _roleManager.FindByNameAsync(role);
                if (roleEntity != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(roleEntity);
                    claims.AddRange(roleClaims);
                }
            }

            claims.AddRange(new[]
            {
                new Claim(ClaimTypes.Sid, loggedUser.Id.ToString()),
                new Claim("UserName", loggedUser.UserName),
                new Claim(ClaimTypes.Name, loggedUser.FullName),
                new Claim(ClaimTypes.Email, loggedUser.Email),
                new Claim(ClaimTypes.MobilePhone, loggedUser.PhoneNumber),
                new Claim(ClaimTypes.Expired, CoreHelper.SystemTimeNow.AddHours(hour).Date.ToShortDateString())
            });

            return JwtUtils.GenerateToken(claims.Distinct(), hour);
        }

        private static RefreshToken GenerateRefreshToken(int userId, int hour)
        {
            var randomByte = new byte[64];
            var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            rngCryptoServiceProvider.GetBytes(randomByte);
            var refreshToken = new RefreshToken
            {
                UserId = userId,
                Token = Convert.ToBase64String(randomByte),
                Expires = CoreHelper.SystemTimeNow.AddHours(hour),
            };
            return refreshToken;
        }
    }
}