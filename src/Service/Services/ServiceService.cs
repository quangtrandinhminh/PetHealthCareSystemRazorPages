using BusinessObject.DTO.Service;
using BusinessObject.Entities;
using BusinessObject.Entities.Identity;
using BusinessObject.Mapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Repository.Interfaces;
using Repository.Repositories;
using Service.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Utility.Constants;
using Utility.Exceptions;
using Utility.Helpers;

namespace Service.Services
{
    public class ServiceService(IServiceProvider serviceProvider) : IService
    {
        private readonly MapperlyMapper _mapper = serviceProvider.GetRequiredService<MapperlyMapper>();
        private readonly IServiceRepository _serviceRepo = serviceProvider.GetRequiredService<IServiceRepository>();

        private readonly UserManager<UserEntity> _userManager =
            serviceProvider.GetRequiredService<UserManager<UserEntity>>();

        public async Task CreateServiceAsync(ServiceRequestDto service, int createdById)
        {
            var user =  await _userManager.FindByIdAsync(createdById.ToString());
            if (user == null)
            {
                throw new AppException(ResponseCodeConstants.NOT_FOUND, ResponseMessageConstantsUser.USER_NOT_FOUND
                                   , StatusCodes.Status404NotFound);
            }

            var serviceEntity = _mapper.Map(service);
            serviceEntity.CreatedBy = createdById;
            serviceEntity.CreatedTime = serviceEntity.LastUpdatedTime = CoreHelper.SystemTimeNow;

            await _serviceRepo.AddAsync(serviceEntity);
        }

        public async Task DeleteServiceAsync(int serviceId, int deleteBy)
        {
            var user = await _userManager.FindByIdAsync(deleteBy.ToString());
            if (user == null)
            {
                throw new AppException(ResponseCodeConstants.NOT_FOUND, ResponseMessageConstantsUser.USER_NOT_FOUND
                                                  , StatusCodes.Status404NotFound);
            }

            var service = await _serviceRepo.GetSingleAsync(s => s.Id == serviceId);
            if (service == null)
            {
                throw new AppException(ResponseCodeConstants.NOT_FOUND, ResponseMessageConstantsService.SERVICE_NOT_FOUND
                                                                 , StatusCodes.Status404NotFound);
            }

            service.DeletedBy = deleteBy;
            service.DeletedTime = CoreHelper.SystemTimeNow;
            await _serviceRepo.UpdateAsync(service);
        }

        public async Task<List<ServiceResponseDto>> GetAllServiceAsync()
        {
            var list = await _serviceRepo.GetAllWithCondition(s => s.DeletedTime == null).ToListAsync();
            if (list == null)
            {
                throw new AppException(ResponseCodeConstants.NOT_FOUND, ResponseMessageConstantsService.SERVICE_NOT_FOUND
                                                                                , StatusCodes.Status404NotFound);
            }

            var listDto = _mapper.Map(list);

            return listDto.ToList();
        }

        public async Task<ServiceResponseDto> GetServiceBydId(int serviceId)
        {
            var service = await _serviceRepo.GetSingleAsync(s => s.Id == serviceId);
            if (service == null)
            {
                throw new AppException(ResponseCodeConstants.NOT_FOUND, ResponseMessageConstantsService.SERVICE_NOT_FOUND
                                                                                , StatusCodes.Status404NotFound);
            }

            var response = _mapper.Map(service);

            return response;
        }

        public async Task UpdateServiceAsync(ServiceUpdateDto service, int updatedById)
        {
            var user = await _userManager.FindByIdAsync(updatedById.ToString());
            if (user == null)
            {
                throw new AppException(ResponseCodeConstants.NOT_FOUND, ResponseMessageConstantsUser.USER_NOT_FOUND
                                                                 , StatusCodes.Status404NotFound);
            }

            var serviceEntity = await _serviceRepo.GetSingleAsync(s => s.Id == service.Id);
            if (serviceEntity == null)
            {
                throw new AppException(ResponseCodeConstants.NOT_FOUND, ResponseMessageConstantsService.SERVICE_NOT_FOUND
                                                                                               , StatusCodes.Status404NotFound);
            }

            _mapper.Map(service, serviceEntity);
            serviceEntity.LastUpdatedBy = updatedById;
            serviceEntity.LastUpdatedTime = CoreHelper.SystemTimeNow;

            await _serviceRepo.UpdateAsync(serviceEntity);
        }
    }
}
