using BusinessObject.DTO;
using BusinessObject.DTO.Appointment;
using BusinessObject.DTO.Cage;
using BusinessObject.DTO.Hospitalization;
using BusinessObject.DTO.TimeTable;
using BusinessObject.DTO.User;
using BusinessObject.Entities.Identity;
using BusinessObject.Mapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository.Extensions;
using Repository.Interfaces;
using Serilog;
using Service.IServices;
using Utility.Constants;
using Utility.Enum;
using Utility.Exceptions;
using Utility.Helpers;

namespace Service.Services;

public class HospitalizationService(IServiceProvider serviceProvider) : IHospitalizationService
{
    private readonly ILogger _logger = Log.Logger;
    private readonly MapperlyMapper _mapper = serviceProvider.GetRequiredService<MapperlyMapper>();
    private readonly IUserRepository _userRepository = serviceProvider.GetRequiredService<IUserRepository>();
    private readonly IHospitalizationRepository _hospitalizationRepo = serviceProvider.GetRequiredService<IHospitalizationRepository>();
    private readonly IMedicalRecordRepository _medicalRecordRepository = serviceProvider.GetRequiredService<IMedicalRecordRepository>();
    private readonly UserManager<UserEntity> _userManager = serviceProvider.GetRequiredService<UserManager<UserEntity>>();
    private readonly IAppointmentRepository _appointmentRepo = serviceProvider.GetRequiredService<IAppointmentRepository>();
    private readonly ITimeTableRepository _timeTableRepo = serviceProvider.GetRequiredService<ITimeTableRepository>();
    private readonly IUserService _userService = serviceProvider.GetRequiredService<IUserService>();
    private readonly ICageRepository _cageRepository = serviceProvider.GetRequiredService<ICageRepository>();

    public async Task<List<TimeTableResponseDto>> GetAllTimeFramesForHospitalizationAsync()
    {
        _logger.Information("Get all time frames for hospitalization");

        var timetables = _timeTableRepo.GetAllWithCondition(t => t.Type == TimeTableType.Hospitalization);
        var response = _mapper.Map(timetables);

        return await response.ToListAsync();
    }

    public async Task<List<UserResponseDto>> GetFreeWithTimeFrameAndDateAsync(DateTimeQueryDto qo)
    {
        _logger.Information("Get free vet with time frame and date {@qo}", qo);
        if (!DateOnly.TryParse(qo.Date, out DateOnly date))
        {
            throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsCommon.DATE_WRONG_FORMAT);
        }

        if (qo.Date == null)
        {
            throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsCommon.DATA_NOT_ENOUGH);
        }

        var vetList = await _userService.GetAllUsersByRoleAsync(UserRole.Vet);

        var hospitalizations = _hospitalizationRepo.GetAllWithCondition(e => e.Date == date && e.TimeTableId == qo.TimetableId);

        var freeVetList = vetList.Where(e => !hospitalizations.Any(ee => ee.VetId == e.Id)).ToList();

        return freeVetList;
    }

    public async Task<List<CageResponseDto>> GetAvailableCageByDate(DateTimeQueryDto qo)
    {
        _logger.Information("Get available cages with time frame and date {@qo}", qo);
        if (!DateOnly.TryParse(qo.Date, out DateOnly date))
        {
            throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsCommon.DATE_WRONG_FORMAT);
        }

        if (qo.Date == null)
        {
            throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsCommon.DATA_NOT_ENOUGH);
        }

        var cages = _cageRepository.GetAllWithCondition(c => c.DeletedTime == null);
        var hospitalizations = _hospitalizationRepo.GetAllWithCondition(e => e.Date == date && e.TimeTableId == qo.TimetableId);

        cages = cages.Where(e => !hospitalizations.Any(ee => ee.CageId == e.Id));
        var response = _mapper.Map(cages);

        return await response.ToListAsync();
    }

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
        foreach (var item in paginatedList.Items)
        {
            var vet = await _userRepository.GetSingleAsync(e => e.Id == item.VetId);
            if (vet != null) item.Vet = _mapper.UserToUserResponseDto(vet);
        }
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

    public async Task<HospitalizationResponseDtoWithDetails> GetHospitalizationById(int hospitalizationId)
    {
        _logger.Information($"Get hospitalization by hospitalizationId {hospitalizationId}");
        var hospitalization = await _hospitalizationRepo.GetSingleAsync(h => h.Id == hospitalizationId, 
            false, h => h.TimeTable);
        if (hospitalization == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                               ResponseMessageConstantsHospitalization.HOSPITALIZATION_NOT_FOUND, StatusCodes.Status404NotFound);
        }
        var response = _mapper.HospitalizationToHospitalizationResponseDtoWithDetails(hospitalization);
        var vet = await _userRepository.GetSingleAsync(e => e.Id == response.VetId);
        if (vet != null) response.Vet = _mapper.UserToUserResponseDto(vet);
        return response;
    }

    public List<EnumResponseDto> GetHospitalizationStatus()
    {
        _logger.Information("Get hospitalization status");
        var hospitalizationStatus = Enum.GetValues<HospitalizationStatus>()
            .Select(e => new EnumResponseDto() { Id = (int)e, Value = e.ToString() }).ToList();
        return hospitalizationStatus;
    }

    public async Task CreateHospitalization(HospitalizationRequestDto dto, int staffId)
    {
        _logger.Information("Create hospitalization {@dto} by staff id {staffId}", dto, staffId);
        // check medical record
        var medicalRecord = await _medicalRecordRepository.GetSingleAsync(mr => mr.Id == dto.MedicalRecordId,
            false, mr => mr.MedicalItems, mr => mr.Pet,
            mr => mr.Hospitalization);
        if (medicalRecord == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                ResponseMessageConstantsMedicalRecord.MEDICAL_RECORD_NOT_FOUND, StatusCodes.Status404NotFound);
        }
        if (medicalRecord.AdmissionDate == null)
        {
            throw new AppException(ResponseCodeConstants.FAILED,
                ResponseMessageConstantsHospitalization.MEDICAL_RECORD_NOT_ADMITTED, StatusCodes.Status400BadRequest);
        }
        if (medicalRecord.DischargeDate != null)
        {
            throw new AppException(ResponseCodeConstants.FAILED,
                ResponseMessageConstantsHospitalization.MEDICAL_RECORD_ALREADY_DISCHARGED, StatusCodes.Status400BadRequest);
        }

        // check vet
        var vet = await _userService.GetVetByIdAsync(dto.VetId);
        if (vet == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND, ResponseMessageConstantsUser.VET_NOT_FOUND);
        }

        if (!DateOnly.TryParse(dto.Date, out DateOnly date))
        {
            throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsCommon.DATE_WRONG_FORMAT);
        }

        if (dto.Date == null)
        {
            throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsCommon.DATA_NOT_ENOUGH);
        }

        // check timetable
        var timeTable = await _timeTableRepo.GetSingleAsync(e => e.Id == dto.TimeTableId && e.Type == TimeTableType.Hospitalization);
        if (timeTable == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                ResponseMessageConstantsTimetable.TIMETABLE_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        // check cage
        var cage = await _cageRepository.GetSingleAsync(e => e.Id == dto.CageId);
        if (cage == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                               ResponseMessageConstantsCage.CAGE_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        var hospitalization = _mapper.Map(dto);

        // make sure that there is only one admission date and discharge date for each medical record
        // , if this is the first hospitalization, set the HospitalizationDateStatus as admission date, else Monitoring
        hospitalization.HospitalizationDateStatus = medicalRecord.Hospitalization.Any(e =>
            e.HospitalizationDateStatus == HospitalizationStatus.AdmissionsDate)
            ? HospitalizationStatus.Monitoring : HospitalizationStatus.AdmissionsDate;

        hospitalization.CreatedBy = staffId;
        hospitalization.CreatedTime = CoreHelper.SystemTimeNow;
        _hospitalizationRepo.Add(hospitalization);
    }

    public Task UpdateHospitalization(HospitalizationRequestDto dto, int vetId)
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