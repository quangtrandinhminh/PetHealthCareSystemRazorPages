using BusinessObject.DTO.MedicalItem;
using BusinessObject.DTO.Service;

namespace Service.IServices
{
    public interface IMedicalItemService
    {
        Task<List<MedicalResponseDto>> GetAllMedicalItem();
        Task CreateMedicalItem(MedicalResponseDto medicalItem);
        Task UpdateMedicalItem(MedicalResponseDto medicalItem);
        Task DeleteMedicalItem(int id, int deleteBy);
    }
}
