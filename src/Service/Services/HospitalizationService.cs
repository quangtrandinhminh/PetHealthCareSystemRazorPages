using BusinessObject.DTO;
using BusinessObject.DTO.Appointment;
using BusinessObject.DTO.Cage;
using BusinessObject.DTO.Hospitalization;
using BusinessObject.DTO.TimeTable;
using BusinessObject.DTO.User;
using BusinessObject.Entities;
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

        var timetables = _timeTableRepo.GetAllWithCondition(t => t.DeletedTime == null
                                                                 && t.Type == TimeTableType.Hospitalization);
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

        var hospitalizations = _hospitalizationRepo.GetAllWithCondition(e => e.DeletedTime == null
                                                                             && e.Date == date && e.TimeTableId == qo.TimetableId);

        var freeVetList = vetList.Where(e => !hospitalizations.Any(ee => ee.VetId == e.Id)).ToList();

        return freeVetList;
    }

    public async Task<List<CageResponseDto>> GetAvailableCage()
    {
        _logger.Information("Get available cages");
        var cages = _cageRepository.GetAllWithCondition(c => c.DeletedTime == null && c.IsAvailable);
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
    public async Task<List<HospitalizationResponseDto>> GetListHospitalizationByMRId(int medicalRecordId)
    {
        _logger.Information($"Get hospitalization by medicalrecordId {medicalRecordId}");
        var hospitalization = _hospitalizationRepo.GetAllWithCondition(h =>
            h.DeletedTime == null && h.MedicalRecordId == medicalRecordId).OrderByDescending(h => h.CreatedTime);
        if (hospitalization == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                               ResponseMessageConstantsHospitalization.HOSPITALIZATION_NOT_FOUND, StatusCodes.Status404NotFound);
        }
        var response = _mapper.Map(hospitalization);
        return await response.ToListAsync();
    }

    public async Task<PaginatedList<HospitalizationResponseDto>> GetAllHospitalizationWithFilters(HospitalizationFilterDto filter, int pageNumber, int pageSize)
    {
        _logger.Information("Get all hospitalization with filters {@filter}", filter);
        var hospitalization = _hospitalizationRepo.GetAllWithCondition(h =>
                       h.DeletedTime == null);
        if (filter.Date != null && DateOnly.TryParse(filter.Date, out DateOnly date))
        {
            hospitalization = hospitalization.Where(e => e.Date == date);
        }
        else if (filter.FromDate != null && filter.ToDate != null &&
                 DateOnly.TryParse(filter.FromDate, out DateOnly fromDate) &&
                 DateOnly.TryParse(filter.ToDate, out DateOnly toDate))
        {
            hospitalization = hospitalization.Where(e => e.Date >= fromDate && e.Date <= toDate);
        }

        if (hospitalization == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                               ResponseMessageConstantsHospitalization.HOSPITALIZATION_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        if (filter.MedicalRecordId != null)
        {
            hospitalization = hospitalization.Where(e => e.MedicalRecordId == filter.MedicalRecordId);
        }

        if (filter.CageId != null)
        {
            hospitalization = hospitalization.Where(e => e.CageId == filter.CageId);
        }

        if (filter.TimeTableId != null)
        {
            hospitalization = hospitalization.Where(e => e.TimeTableId == filter.TimeTableId);
        }

        if (filter.VetId != null)
        {
            hospitalization = hospitalization.Where(e => e.VetId == filter.VetId);
        }

        if (filter.IsDischarged == true)
        {
            hospitalization = hospitalization.Where(e => e.HospitalizationDateStatus == HospitalizationStatus.DischargeDate);
        }

        if (filter.IsMonitoring == true)
        {
            hospitalization = hospitalization.Where(e => e.HospitalizationDateStatus == HospitalizationStatus.Monitoring);
        }

        if (filter.IsAdmission == true)
        {
            hospitalization = hospitalization.Where(e => e.HospitalizationDateStatus == HospitalizationStatus.AdmissionDate);
        }

        if (filter.IsDecreasingByCreatedTime == true)
        {
            hospitalization = hospitalization.OrderByDescending(e => e.CreatedTime);
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

    public async Task CreateHospitalization(HospitalizationRequestDto dto, int staffId)
    {
        _logger.Information("Create hospitalization {@dto} by staff id {staffId}", dto, staffId);
        var (medicalRecord, cage) = await CheckHospitalizationRequestDto(dto);

        var hospitalization = _mapper.Map(dto);

        // make sure that there is only one admission date and discharge date for each medical record
        // , if this is the first hospitalization, set the HospitalizationDateStatus as admission date, else Monitoring
        hospitalization.HospitalizationDateStatus = medicalRecord.Hospitalization.Any(e =>
            e.HospitalizationDateStatus == HospitalizationStatus.AdmissionDate)
            ? HospitalizationStatus.Monitoring : HospitalizationStatus.AdmissionDate;

        hospitalization.CreatedBy = hospitalization.LastUpdatedBy = staffId;
        hospitalization.CreatedTime = hospitalization.LastUpdatedTime = CoreHelper.SystemTimeNow;
        cage.IsAvailable = false;
        cage.LastUpdatedBy = staffId;
        cage.LastUpdatedTime = CoreHelper.SystemTimeNow;
        await _hospitalizationRepo.AddAsync(hospitalization);
        await _cageRepository.UpdateAsync(cage);
    }

    public async Task UpdateHospitalization(HospitalizationUpdateRequestDto dto, int vetId)
    {
        _logger.Information("Create hospitalization {@dto} by staff id {vetId}", dto, vetId);
        var vet = await _userRepository.GetSingleAsync(e => e.Id == vetId);
        if (vet == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND, ResponseMessageConstantsUser.VET_NOT_FOUND);
        }

        var hospitalization = await _hospitalizationRepo.GetSingleAsync(h => h.Id == dto.Id,false,h => h.MedicalRecord);
        if (hospitalization == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                ResponseMessageConstantsHospitalization.HOSPITALIZATION_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        hospitalization.Reason = dto.Reason;
        hospitalization.Diagnosis = dto.Diagnosis;
        hospitalization.Treatment = dto.Treatment;
        hospitalization.Note = dto.Note;
        if (dto.IsDischarged == true)
        {
            hospitalization.HospitalizationDateStatus = HospitalizationStatus.DischargeDate;
        }
        hospitalization.LastUpdatedBy = vetId;
        hospitalization.LastUpdatedTime = CoreHelper.SystemTimeNow;
        hospitalization.MedicalRecord.DischargeDate = CoreHelper.SystemTimeNow;

        await _hospitalizationRepo.UpdateAsync(hospitalization);
    }

    public async Task DeleteHospitalization(int hospitalizationId, int deleteBy)
    {
        var hospitalization = await _hospitalizationRepo.GetSingleAsync(h => h.DeletedTime == null
                                                                             && h.Id == hospitalizationId);
        if (hospitalization == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                               ResponseMessageConstantsHospitalization.HOSPITALIZATION_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        hospitalization.DeletedTime = CoreHelper.SystemTimeNow;
        hospitalization.DeletedBy = deleteBy;

        await _hospitalizationRepo.UpdateAsync(hospitalization);
    }

    public async Task<CageResponseDto> GetCurrentCageByMedicalRecordId(int medicalRecordId)
    {
        var medicalRecord = await _medicalRecordRepository.GetSingleAsync(mr => mr.Id == medicalRecordId,
                       false, mr => mr.Hospitalization);
        if (medicalRecord == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                               ResponseMessageConstantsMedicalRecord.MEDICAL_RECORD_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        var currentHospitalization = medicalRecord.Hospitalization.OrderByDescending(h => h.CreatedTime).FirstOrDefault(e => e.DeletedTime == null);
        if (currentHospitalization == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                               ResponseMessageConstantsHospitalization.HOSPITALIZATION_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        var cage = await _cageRepository.GetSingleAsync(e => e.Id == currentHospitalization.CageId);
        if (cage == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                               ResponseMessageConstantsCage.CAGE_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        return _mapper.Map(cage);
    }

    public async Task ChangeCage(int hospitalizationId, int cageId, int staffId)
    {
        var hospitalization = await _hospitalizationRepo.GetSingleAsync(h => h.Id == hospitalizationId,
                       false, h => h.Cage);
        if (hospitalization == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                               ResponseMessageConstantsHospitalization.HOSPITALIZATION_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        var cage = await _cageRepository.GetSingleAsync(e => e.Id == cageId);
        if (cage == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                               ResponseMessageConstantsCage.CAGE_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        if (cage.IsAvailable == false)
        {
            throw new AppException(ResponseCodeConstants.FAILED,
                               ResponseMessageConstantsCage.CAGE_NOT_AVAILABLE, StatusCodes.Status400BadRequest);
        }

        var oldCage = hospitalization.Cage;
        oldCage.IsAvailable = true;
        hospitalization.CageId = cageId;
        hospitalization.LastUpdatedBy = staffId;
        hospitalization.LastUpdatedTime = CoreHelper.SystemTimeNow;

        cage.IsAvailable = false;
        cage.LastUpdatedBy = staffId;
        cage.LastUpdatedTime = CoreHelper.SystemTimeNow;

        await _hospitalizationRepo.UpdateAsync(hospitalization);
        await _cageRepository.UpdateAsync(cage);
        await _cageRepository.UpdateAsync(oldCage);
    }

    // check hospitalization dto is valid 
    private async Task<(MedicalRecord, Cage)> CheckHospitalizationRequestDto(
        HospitalizationRequestDto dto)
    {
        // check date is valid
        if (!DateTimeOffset.TryParse(dto.Date, out DateTimeOffset date))
        {
            throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsCommon.DATE_WRONG_FORMAT);
        }

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
        else if (medicalRecord.AdmissionDate != null && medicalRecord.AdmissionDate > date)
        {

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

        return (medicalRecord, cage);
    }
}