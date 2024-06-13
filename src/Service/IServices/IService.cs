using BusinessObject.DTO.Pet;
using BusinessObject.DTO.Service;
using BusinessObject.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IServices
{
    public interface IService
    {
        Task<List<ServiceResponseDto>> GetAllService();
        Task CreateServiceAsync(ServiceResponseDto service);
        Task UpdateServiceAsync(ServiceResponseDto service);
        Task DeleteServiceAsync(int id, int deleteBy);
    }
}
