﻿using BusinessObject.DTO.Pet;
using BusinessObject.DTO.Service;
using BusinessObject.DTO.User;
using Repository.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IServices
{
    public interface IService
    {
        Task<List<ServiceResponseDto>> GetAllServiceAsync();
        Task<PaginatedList<ServiceResponseDto>> GetAllServiceAsync(int pageNumber, int pageSize);
        Task<ServiceResponseDto> GetServiceBydId(int serviceId);
        Task CreateServiceAsync(ServiceRequestDto service, int createdById);
        Task UpdateServiceAsync(ServiceUpdateDto service, int updatedById);
        Task DeleteServiceAsync(int serviceId, int deleteBy);
    }
}
