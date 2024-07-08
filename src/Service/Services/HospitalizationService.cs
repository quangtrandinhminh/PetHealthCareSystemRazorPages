using BusinessObject.DTO.Hospitalization;
using BusinessObject.DTO.MedicalRecord;
using BusinessObject.Entities;
using BusinessObject.Entities.Identity;
using BusinessObject.Mapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Repository.Extensions;
using Repository.Interfaces;
using Serilog;
using Service.IServices;
using Utility.Constants;
using Utility.Exceptions;

namespace Service.Services;

public class HospitalizationService(IServiceProvider serviceProvider) : IHospitalizationService
{
    private readonly ILogger _logger = Log.Logger;
    private readonly MapperlyMapper _mapper = serviceProvider.GetRequiredService<MapperlyMapper>();
    private readonly IUserRepository _userRepository = serviceProvider.GetRequiredService<IUserRepository>();
    private readonly IHospitalizationRepository _hospitalizationRepo = serviceProvider.GetRequiredService<IHospitalizationRepository>();
    private readonly IMedicalRecordRepository _medicalRecordRepo = serviceProvider.GetRequiredService<IMedicalRecordRepository>();
    private readonly UserManager<UserEntity> _userManager = serviceProvider.GetRequiredService<UserManager<UserEntity>>();

    public async Task<PaginatedList<HospitalizationResponseDto>> GetAllHospitalization(int pageNumber, int pageSize)
    {
        _logger.Information("Get all hospitalization");
        var hospitalization = _hospitalizationRepo.GetAllWithCondition(h =>
            h.DeletedTime == null).OrderByDescending(h => h.CreatedTime);
        if (hospitalization == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                ResponseMessageConstantsHospitalization.HOSPITALIZATION_NOT_FOUND, StatusCodes.Status404NotFound);
        }
        var response = _mapper.Map(hospitalization);
        var paginatedList = await PaginatedList<HospitalizationResponseDto>.CreateAsync(response, pageNumber, pageSize);
        return paginatedList;
    }

    public async Task<PaginatedList<HospitalizationResponseDto>> GetAllHospitalizationByMedicalRecordId(int medicalRecordId, int pageNumber, int pageSize)
    {
        _logger.Information($"Get all hospitalization by medical record id {medicalRecordId}");
        var hospitalization = _hospitalizationRepo.GetAllWithCondition(h =>
            h.DeletedTime == null && h.MedicalRecordId == medicalRecordId).OrderByDescending(h => h.CreatedTime);
        if (hospitalization == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                ResponseMessageConstantsHospitalization.HOSPITALIZATION_NOT_FOUND, StatusCodes.Status404NotFound);
        }
        var response = _mapper.Map(hospitalization);
        var paginatedList = await PaginatedList<HospitalizationResponseDto>.CreateAsync(response, pageNumber, pageSize);
        return paginatedList;
    }

    public async Task<HospitalizationResponseDto> GetHospitalizationById(int id)
    {
        _logger.Information($"Get hospitalization by id {id}");
        var hospitalization = await _hospitalizationRepo.GetSingleAsync(h => h.Id == id);
        if (hospitalization == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                               ResponseMessageConstantsHospitalization.HOSPITALIZATION_NOT_FOUND, StatusCodes.Status404NotFound);
        }
        var response = _mapper.Map(hospitalization);
        return response;
    }

    public Task CreateHospitalization(/*HospitalizationResquestDto dto, */ int staffId)
    {
        _logger.Information("Create hospitalization {@dto} by staff id {staffId}"/*, dto*/, staffId);
        throw new NotImplementedException();
    }

    public Task UpdateHospitalization(/*HospitalizationResquestDto dto, */ int vetId)
    {
        _logger.Information("Create hospitalization {@dto} by staff id {vetId}"/*, dto*/, vetId);
        throw new NotImplementedException();
    }

    public Task DeleteHospitalization(int id, int deleteBy)
    {
        _logger.Information($"Delete hospitalization by id {id} by {deleteBy}");
        throw new NotImplementedException();
    }
}