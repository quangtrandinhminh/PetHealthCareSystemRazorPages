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
    public interface IUserService
    {
        Task Register(RegisterDto dto);
        Task<LoginResponseDto> Authenticate(LoginDto dto);
        Task<VetResponseDto> CreateVet(VetRequestDto dto);
    }
}