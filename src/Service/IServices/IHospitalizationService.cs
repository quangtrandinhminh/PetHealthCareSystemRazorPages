using BusinessObject.DTO.Hospitalization;
using Repository.Extensions;

namespace Service.IServices;

public interface IHospitalizationService
{
    Task<PaginatedList<HospitalizationResponseDto>> GetAllHospitalization(int pageNumber, int pageSize);
    Task<PaginatedList<HospitalizationResponseDto>> GetAllHospitalizationByMedicalRecordId(int medicalRecordId, int pageNumber, int pageSize);
    Task<HospitalizationResponseDto> GetHospitalizationById(int id);
    Task CreateHospitalization(/*HospitalizationResquestDto dto ,*/int staffId);
    Task UpdateHospitalization(/*HospitalizationResquestDto dto, */ int vetId);
    Task DeleteHospitalization(int id, int deleteBy);
}