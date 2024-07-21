using BusinessObject.DTO;
using BusinessObject.DTO.Appointment;
using BusinessObject.DTO.Cage;
using BusinessObject.DTO.Hospitalization;
using BusinessObject.DTO.MedicalRecord;
using BusinessObject.DTO.TimeTable;
using BusinessObject.DTO.User;
using Repository.Extensions;
using Utility.Enum;

namespace Service.IServices;

public interface IHospitalizationService
{
    Task<List<TimeTableResponseDto>> GetAllTimeFramesForHospitalizationAsync();
    Task<List<UserResponseDto>> GetFreeWithTimeFrameAndDateAsync(DateTimeQueryDto qo);
    Task<List<CageResponseDto>> GetAvailableCage();
    Task<PaginatedList<HospitalizationResponseDto>> GetAllHospitalization(int pageNumber, int pageSize);
    Task<PaginatedList<HospitalizationResponseDto>> GetAllHospitalizationByMedicalRecordId(int medicalRecordId, int pageNumber, int pageSize);
    Task<HospitalizationResponseDtoWithDetails> GetHospitalizationById(int hospitalizationId);
    Task<PaginatedList<HospitalizationResponseDto>> GetAllHospitalizationWithFilters(HospitalizationFilterDto filter,
        int pageNumber, int pageSize);
    Task<CageResponseDto> GetCurrentCageByMedicalRecordId(int medicalRecordId);
    Task CreateHospitalization(HospitalizationRequestDto dto ,int staffId);
    Task UpdateHospitalization(HospitalizationUpdateRequestDto dto, int vetId);
    Task DeleteHospitalization(int hospitalizationId, int deleteBy);
    Task<List<HospitalizationResponseDto>> GetListHospitalizationByMRId(int medicalRecordId);
}