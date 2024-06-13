using BusinessObject.DTO.Pet;
using BusinessObject.Entities;
using BusinessObject.Entities.Identity;
using BusinessObject.Mapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Repository.Interfaces;
using Serilog;
using Service.IServices;
using Utility.Constants;
using Utility.Exceptions;

namespace Service.Services;

public class PetService(IServiceProvider serviceProvider) : IPetService
{
    private readonly MapperlyMapper _mapper = serviceProvider.GetRequiredService<MapperlyMapper>();
    private readonly ILogger _logger = Log.Logger;
    private readonly IPetRepository _petRepo = serviceProvider.GetRequiredService<IPetRepository>();
    private readonly UserManager<UserEntity> _userManager = serviceProvider.GetRequiredService<UserManager<UserEntity>>();

    public async Task<List<PetResponseDto>> GetAllPetsForCustomerAsync(int id)
    {
        var list = await _petRepo.GetAllPetsByCustomerIdAsync(id);

        var listDto = _mapper.Map(list);

        return listDto.ToList();
    }

    public async Task CreatePetAsync(PetRequestDto pet, int ownerId)
    {
        var user = await _userManager.FindByIdAsync(ownerId.ToString());

        if (user == null)
        {
            throw new AppException(ResponseCodeConstants.FAILED, ReponseMessageConstantsPet.OWNER_NOT_FOUND,
                StatusCodes.Status400BadRequest);
        }

        var createPet = _mapper.Map(pet);
        createPet.OwnerID = ownerId;

        await _petRepo.CreatePetAsync(createPet);
    }

    public async Task UpdatePetAsync(PetUpdateRequestDto pet)
    {
        // Checking pet in the database
        var findPet = await _petRepo.FindByConditionAsync(e => e.Id == pet.Id);

        var existPet = findPet.FirstOrDefault();

        if (existPet == null)
        {
            throw new AppException(ResponseCodeConstants.FAILED, ReponseMessageConstantsPet.PET_NOT_FOUND,
                StatusCodes.Status400BadRequest);
        }

        // Checking the pet is to the updater
        var user = await _userManager.FindByIdAsync(existPet.OwnerID.ToString());

        if (user == null)
        {
            throw new AppException(ResponseCodeConstants.FAILED, ReponseMessageConstantsPet.OWNER_NOT_FOUND,
                StatusCodes.Status400BadRequest);
        }

        existPet.Breed = pet.Breed;
        existPet.Name = pet.Name;
        existPet.DateOfBirth = pet.DateOfBirth;
        existPet.Gender = pet.Gender;
        existPet.IsNeutered = pet.IsNeutered;
        existPet.Species = pet.Species;

        await _petRepo.UpdatePetAsync(existPet);
    }

    public async Task DeletePetAsync(int id)
    {
        // Checking pet in the database
        var findPet = await _petRepo.FindByConditionAsync(e => e.Id == id);

        var existPet = findPet.FirstOrDefault();

        if (existPet == null)
        {
            throw new AppException(ResponseCodeConstants.FAILED, ReponseMessageConstantsPet.PET_NOT_FOUND,
                StatusCodes.Status400BadRequest);
        }

        // Checking the pet is to the deleter
        var user = await _userManager.FindByIdAsync(existPet.OwnerID.ToString());

        if (user == null)
        {
            throw new AppException(ResponseCodeConstants.FAILED, ReponseMessageConstantsPet.OWNER_NOT_FOUND,
                StatusCodes.Status400BadRequest);
        }

        existPet.DeletedBy = existPet.OwnerID;
        await _petRepo.UpdatePetAsync(existPet);
    }
}