using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessObject.DTO.User;
using BusinessObject.DTO.Vet;
using BusinessObject.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Service.IServices
{
    public interface IAuthService
    {
        Task<IList<RoleResponseDto>> GetAllRoles();
        Task Register(RegisterDto dto);
        Task RegisterByAdmin(RegisterDto dto, int role);
        Task<LoginResponseDto> Authenticate(LoginDto dto);
        Task<LoginResponseDto> RefreshToken(string token);
        Task VerifyEmail(VerifyEmailDto dto);
        Task ForgotPassword(ForgotPasswordDto model);
        Task ResetPassword(ResetPasswordDto model);
        Task ChangePassword(ChangePasswordDto dto);
        Task ReSendEmail(ResendEmailDto model);
        Task StaffRegistor(RegisterDto dto);
    }
}