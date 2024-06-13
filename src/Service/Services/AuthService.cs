using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using BusinessObject.DTO.User;
using BusinessObject.DTO.Vet;
using BusinessObject.Entities;
using BusinessObject.Entities.Identity;
using BusinessObject.Mapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using Repository.Interfaces;
using Repository.Repositories;
using Serilog;
using Service.IServices;
using Service.Utils;
using Utility.Config;
using Utility.Constants;
using Utility.Enum;
using Utility.Exceptions;
using Utility.Helpers;

namespace Service.Services
{
    public class AuthService(IServiceProvider serviceProvider) : IAuthService
    {
        private readonly IUserRepository _userRepository = serviceProvider.GetRequiredService<IUserRepository>();
        private readonly MapperlyMapper _mapper = serviceProvider.GetRequiredService<MapperlyMapper>();
        private readonly RoleManager<RoleEntity> _roleManager = serviceProvider.GetRequiredService<RoleManager<RoleEntity>>();
        private readonly UserManager<UserEntity> _userManager = serviceProvider.GetRequiredService<UserManager<UserEntity>>();
        private readonly ILogger _logger = Log.Logger;
        private readonly SignInManager<UserEntity> _signInManager = serviceProvider.GetRequiredService<SignInManager<UserEntity>>();
        private readonly IRefreshTokenRepository _refreshTokenRepository = serviceProvider.GetRequiredService<IRefreshTokenRepository>();

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
            var account = await GetUserByUserName(dto.Username);
            if (account == null || account.DeletedTime != null)
                throw new AppException(ErrorCode.UserInvalid, ReponseMessageIdentity.INVALID_USER, StatusCodes.Status401Unauthorized);

            // check password
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, account.PasswordHash))
                throw new AppException(ErrorCode.UserPasswordWrong, ReponseMessageIdentity.PASSWORD_WRONG, StatusCodes.Status401Unauthorized);

            try
            {
                var roles = await _userManager.GetRolesAsync(account);
                var token = await GenerateJwtToken(account, roles, 48);
                var refreshToken = GenerateRefreshToken(account.Id, 12);
                await RemoveOldRefreshTokens(account.RefreshTokens);
                await _refreshTokenRepository.AddAsync(refreshToken);
                var response = _mapper.UserToLoginResponseDto(account);
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

        public async Task<LoginResponseDto> RefreshToken(string token)
        {
            var (refreshToken, account) = await GetRefreshToken(token);
            var newRefreshToken = GenerateRefreshToken(account.Id, 12);

            newRefreshToken.UserId = account.Id;
            await _refreshTokenRepository.AddAsync(newRefreshToken);

            await RemoveOldRefreshTokens(account.RefreshTokens);

            try
            {
                var roles = await _userManager.GetRolesAsync(account);
                var jwtToken = await GenerateJwtToken(account, roles, 48);
                var response = _mapper.UserToLoginResponseDto(account);
                response.Token = jwtToken;
                response.RefreshToken = newRefreshToken.Token;
                response.Role = roles;
                return response;
            }
            catch (Exception e)
            {
                throw new AppException(ResponseCodeConstants.FAILED, e.Message, StatusCodes.Status400BadRequest);
            }
        }

        public async Task VerifyEmail(VerifyEmailDto dto)
        {

            var account = await GetUserByUserName(dto.UserName);

            if (account == null || account.OTP != dto.Token)
                throw new AppException(ErrorCode.TokenInvalid, ReponseMessageIdentity.TOKEN_INVALID, StatusCodes.Status401Unauthorized);

            account.Verified = CoreHelper.SystemTimeNow;
            account.OTP = null;
            await _userRepository.UpdateAsync(account);
        }

        public async Task ForgotPassword(ForgotPasswordDto model)
        {
            var account = await GetUserByUserName(model.UserName);
            if (account == null) throw new AppException(ErrorCode.UserInvalid, ReponseMessageIdentity.INVALID_USER, StatusCodes.Status401Unauthorized);

            // create reset token that expires after 1 day
            /*account.OTP = StringHelper.Generate(6, false, false, true, false);
            account.OTPExpired = CoreHelper.SystemTimeNow.AddDays(1);
            
            await _userRepository.UpdateAsync(account);

            var mailRequest = new SendMailModel
            {
                Name = account.NormalizedUserName,
                Email = account.Email,
                Token = account.ResetToken,
                Type = MailTypeEnum.ResetPassword
            };
            _emailService.SendMail(mailRequest);*/
        }

        public async Task ResetPassword(ResetPasswordDto model)
        {
            var account = await GetUserByUserName(model.UserName);

            if (account == null) throw new AppException(ErrorCode.UserInvalid, ReponseMessageIdentity.INVALID_USER, StatusCodes.Status401Unauthorized);

            if (account.OTP != model.Token || account.OTPExpired < CoreHelper.SystemTimeNow)
            {
                throw new AppException(ErrorCode.TokenInvalid, ReponseMessageIdentity.TOKEN_INVALID_OR_EXPIRED, StatusCodes.Status401Unauthorized);
            }

            account.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
            account.OTP = null;
            account.OTPExpired = null;
            account.AccessFailedCount = 0;

            await _userRepository.UpdateAsync(account);
        }

        public async Task ChangePassword(ChangePasswordDto dto)
        {
            var account = await GetUserByUserName(dto.UserName);

            if (account == null || !account.IsActive) throw new AppException(ErrorCode.UserInvalid, ReponseMessageIdentity.INVALID_USER, StatusCodes.Status401Unauthorized);

            if (!BCrypt.Net.BCrypt.Verify(dto.OldPassword, account.PasswordHash))
            {
                throw new AppException(ErrorCode.UserPasswordWrong, ReponseMessageIdentity.UNAUTHENTICATED, StatusCodes.Status401Unauthorized);
            }

            account.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            await _userRepository.UpdateAsync(account);
        }

        public async Task ReSendEmail(ResendEmailDto model)
        {
            var account = await GetUserByUserName(model.UserName);
            if (account == null) throw new AppException(ErrorCode.UserInvalid, ReponseMessageIdentity.INVALID_USER, StatusCodes.Status400BadRequest);
            if (account.OTP == null) throw new AppException(ErrorCode.Validated, ReponseMessageIdentity.EMAIL_VALIDATE, StatusCodes.Status400BadRequest);

            /*account.OTP = StringHelper.Generate(6, false, false, true, false);
            await _userRepository.UpdateAsync(account);

            var mailRequest = new SendMailModel
            {
                Name = account.NormalizedUserName,
                Email = account.Email,
                Token = account.VerificationToken,
                Type = MailTypeEnum.Verify
            };
            _emailService.SendMail(mailRequest);*/
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

        private async Task RemoveOldRefreshTokens(ICollection<RefreshToken> refreshTokens)
        {
            var removeList = refreshTokens.Where(x => !x.IsActive 
                                                      && x.CreatedTime.AddDays(2) <= CoreHelper.SystemTimeNow).ToList();
            if (removeList.Any())
            {
                await _refreshTokenRepository.RemoveRangeAsync(removeList);
            }
        }

        private async Task<(RefreshToken, UserEntity)> GetRefreshToken(string token)
        {
            var account = await _userRepository.GetSingleAsync(y
                    => y.RefreshTokens.Any(t => t.Token == token)
                , _ => _.RefreshTokens);
            if (account == null || account.DeletedTime != null)
            {
                throw new AppException(ErrorCode.TokenInvalid, ReponseMessageIdentity.TOKEN_INVALID, StatusCodes.Status401Unauthorized);
            }

            var refreshToken = account.RefreshTokens.Single(x => x.Token == token);
            if (!refreshToken.IsActive)
            {
                throw new AppException(ErrorCode.TokenExpired, ReponseMessageIdentity.TOKEN_INVALID, StatusCodes.Status401Unauthorized);
            }

            return (refreshToken, account);
        }

        private async Task<UserEntity?> GetUserByUserName(string userName, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetSingleAsync(_ => _.UserName == userName, x => x.RefreshTokens);

            if (user != null && user.DeletedTime != null)
            {
                user = null;
            }

            return user;
        }

        private async Task<UserEntity?> GetUserByEmail(string email, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetSingleAsync(_ => _.Email == email, x => x.RefreshTokens);

            if (user != null && user.DeletedTime != null)
            {
                user = null;
            }

            return user;
        }
    }
}