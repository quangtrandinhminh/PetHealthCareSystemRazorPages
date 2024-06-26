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
        try
        {
            _logger.Information("Get all pet for customer");

            var list = await _petRepo.GetAllPetsByCustomerIdAsync(id);

            var listDto = _mapper.Map(list);

            return listDto.ToList();
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            throw new AppException(ResponseCodeConstants.INTERNAL_SERVER_ERROR,
                ResponseMessageConstantsCommon.SERVER_ERROR);
        }
    }

    public async Task<PetResponseDto> GetPetForCustomerAsync(int ownerId, int petId)
    {
        try
        {
            _logger.Information("Get pet for customer by id");

            var list = await _petRepo.GetAllPetsByCustomerIdAsync(ownerId);

            var pet = list.Where(e => e.Id == petId).FirstOrDefault();

            if (pet == null)
            {
                throw new AppException(ResponseCodeConstants.NOT_FOUND, ResponseMessageConstantsPet.PET_NOT_FOUND,
                    StatusCodes.Status404NotFound);
            }

            return _mapper.Map(pet);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            throw new AppException(ResponseCodeConstants.INTERNAL_SERVER_ERROR,
                ResponseMessageConstantsCommon.SERVER_ERROR);
        }
    }

    public async Task<PetResponseDto> GetPetByIdAsync(int id)
    {
        try
        {
            _logger.Information("Get pet by id");

            var pet = await _petRepo.GetSingleAsync(p => p.Id == id);
            if (pet == null)
            {
                throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsPet.PET_NOT_FOUND,
                    StatusCodes.Status400BadRequest);
            }

            return _mapper.Map(pet);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            throw new AppException(ResponseCodeConstants.INTERNAL_SERVER_ERROR,
                ResponseMessageConstantsCommon.SERVER_ERROR);
        }
    }

    public async Task CreatePetAsync(PetRequestDto pet, int ownerId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(ownerId.ToString());

            if (user == null)
            {
                throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsPet.OWNER_NOT_FOUND,
                    StatusCodes.Status400BadRequest);
            }

            var createPet = _mapper.Map(pet);
            createPet.OwnerID = ownerId;

            await _petRepo.CreatePetAsync(createPet);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            throw new AppException(ResponseCodeConstants.INTERNAL_SERVER_ERROR,
                ResponseMessageConstantsCommon.SERVER_ERROR);
        }
    }

    public async Task UpdatePetAsync(PetUpdateRequestDto pet, int ownerId)
    {
        try
        {
            // Checking pet in the database
            var findPet = await _petRepo.FindByConditionAsync(e => e.Id == pet.Id);

            var existPet = findPet.FirstOrDefault();

            if (existPet == null || existPet.DeletedBy != null)
            {
                throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsPet.PET_NOT_FOUND,
                    StatusCodes.Status400BadRequest);
            }

            if (existPet.OwnerID != ownerId)
            {
                throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsPet.NOT_YOUR_PET,
                    StatusCodes.Status400BadRequest);
            }

            var updatePet = _mapper.Map(pet);
            updatePet.OwnerID = existPet.OwnerID;

            await _petRepo.UpdatePetAsync(updatePet);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            throw new AppException(ResponseCodeConstants.INTERNAL_SERVER_ERROR,
                ResponseMessageConstantsCommon.SERVER_ERROR);
        }
    }

    public async Task DeletePetAsync(int id, int ownerId)
    {
        try
        {
            // Checking pet in the database
            var findPet = await _petRepo.FindByConditionAsync(e => e.Id == id);

            var existPet = findPet.FirstOrDefault();

            if (existPet == null || existPet.DeletedBy != null)
            {
                throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsPet.PET_NOT_FOUND,
                    StatusCodes.Status400BadRequest);
            }

            if (existPet.OwnerID != ownerId)
            {
                throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsPet.NOT_YOUR_PET,
                    StatusCodes.Status400BadRequest);
            }

            existPet.DeletedBy = existPet.OwnerID;
            await _petRepo.UpdatePetAsync(existPet);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            throw new AppException(ResponseCodeConstants.INTERNAL_SERVER_ERROR,
                ResponseMessageConstantsCommon.SERVER_ERROR);
        }
    }
}