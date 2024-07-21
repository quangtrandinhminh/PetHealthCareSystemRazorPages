using BusinessObject.DTO.MedicalItem;
using BusinessObject.DTO.MedicalRecord;
using Repository.Extensions;

namespace Service.IServices;

public interface IMedicalService
{
    // medical item
    Task<List<MedicalResponseDto>> GetAllMedicalItem();
    Task<PaginatedList<MedicalResponseDto>> GetAllMedicalItem(int pageNumber, int pageSize);
    Task<MedicalResponseDto> GetMedicalItemById(int medicalItemId);
    Task CreateMedicalItem(MedicalItemRequestDto medicalItem, int createdById);
    Task UpdateMedicalItem(MedicalItemUpdateDto dto, int updatedById);
    Task DeleteMedicalItem(int id, int deleteBy);
    Task UpdateMedicalRecord(MedicalRecordRequestDto dto, int updatedById);
    // medical record
    Task<PaginatedList<MedicalRecordResponseDto>> GetAllMedicalRecord(int pageNumber, int pageSize);
    Task<PaginatedList<MedicalRecordResponseDto>> GetAllMedicalRecordForHospitalization (int pageNumber, int pageSize);
    Task<PaginatedList<MedicalRecordResponseDto>> GetAllMedicalRecordByPetId(int petId, int pageNumber, int pageSize);
    Task<MedicalRecordResponseDtoWithDetails> GetMedicalRecordById(int medicalRecordId);
    Task<MedicalRecordResponseDtoWithDetails> GetMedicalRecordByPetIdAndAppointmentId(int petId, int appointmentId);
    Task<MedicalRecordResponseDtoWithDetails> CreateMedicalRecord(MedicalRecordRequestDto dto, int vetId);
}