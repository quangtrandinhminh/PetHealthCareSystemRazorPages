using BusinessObject.DTO.Pet;

namespace Service.IServices;

public interface IPetService
{
    Task<List<PetResponseDto?>> GetAllPetsForCustomerAsync(int id);
    Task<PetResponseDto> GetPetForCustomerAsync(int ownerId, int petId);
    Task<PetResponseDto> GetPetByIdAsync(int id);
    Task CreatePetAsync(PetRequestDto pet, int ownerId);
    Task UpdatePetAsync(PetUpdateRequestDto pet, int ownerId);
    Task DeletePetAsync(int id, int ownerId);
}