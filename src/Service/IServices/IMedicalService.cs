using BusinessObject.DTO.MedicalItem;
using BusinessObject.DTO.MedicalRecord;
using Repository.Extensions;

namespace Service.IServices;

public interface IMedicalService
{
    // medical item
    Task<List<MedicalResponseDto>> GetAllMedicalItem();
    Task CreateMedicalItem(MedicalResponseDto medicalItem);
    Task UpdateMedicalItem(MedicalResponseDto medicalItem);
    Task DeleteMedicalItem(int id, int deleteBy);

    // medical record
    Task<PaginatedList<MedicalRecordResponseDto>> GetAllMedicalRecord(int pageNumber, int pageSize);
    Task<PaginatedList<MedicalRecordResponseDto>> GetAllMedicalRecordForHospitalization (int pageNumber, int pageSize);
    Task<PaginatedList<MedicalRecordResponseDto>> GetAllMedicalRecordByPetId(int petId, int pageNumber, int pageSize);
    Task<MedicalRecordResponseDtoWithDetails> GetMedicalRecordById(int medicalRecordId);
    Task<MedicalRecordResponseDtoWithDetails> CreateMedicalRecord(MedicalRecordRequestDto dto, int vetId);
    Task UpdateMedicalRecord(MedicalRecordResponseDto dto, int staffId);
    Task DeleteMedicalRecord(int id, int deleteBy);
}