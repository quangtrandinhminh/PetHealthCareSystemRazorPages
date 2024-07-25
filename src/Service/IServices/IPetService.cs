using BusinessObject.DTO.Pet;
using Repository.Extensions;

namespace Service.IServices;

public interface IPetService
{
    Task<PaginatedList<PetResponseDto>> GetAllPetsAsync(int? customerId, int pageNumber, int pageSize);
    Task<List<PetResponseDto?>> GetAllPetsForCustomerAsync(int id);
    Task<PetResponseDto> GetPetForCustomerAsync(int ownerId, int petId);
    Task<PetResponseDto> GetPetByIdAsync(int id);
    Task CreatePetAsync(PetRequestDto pet, int ownerId);
    Task UpdatePetAsync(PetUpdateRequestDto pet, int ownerId);
    Task DeletePetAsync(int id, int ownerId);
}