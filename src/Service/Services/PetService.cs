﻿using BusinessObject.DTO.Pet;
using BusinessObject.Entities;
using BusinessObject.Entities.Identity;
using BusinessObject.Mapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Repository.Extensions;
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
    private readonly IUserService _userService = serviceProvider.GetRequiredService<IUserService>();

    public async Task<PaginatedList<PetResponseDto>> GetAllPetsAsync(int? customerId, int pageNumber, int pageSize)
    {
        _logger.Information($"Get all pets for customer {customerId}");
        var pets = _petRepo.GetAllWithCondition(p => p.DeletedTime == null, p => p.Owner);
        if (customerId != null)
        {
            var customer = await _userService.GetByIdAsync(customerId.Value);
            if (customer == null)
            {
                throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsPet.OWNER_NOT_FOUND,
                                       StatusCodes.Status400BadRequest);
            }

            pets = pets.Where(p => p.OwnerID == customerId);
        }

        if (pets == null)
        {
            throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsPet.PET_NOT_FOUND,
                StatusCodes.Status404NotFound);
        }

        var response = _mapper.Map(pets);
        var paginatedResponse = await PaginatedList<PetResponseDto>.CreateAsync(response, pageNumber, pageSize);
        foreach (var pet in paginatedResponse.Items)
        {
            pet.OwnerName = pet.Owner.FullName;
        }
        return paginatedResponse;
    }

    public async Task<List<PetResponseDto?>> GetAllPetsForCustomerAsync(int id)
    {
        _logger.Information($"Get all pet for customer {id}");

        var list = await _petRepo.GetAllPetsByCustomerIdAsync(id);

        var listDto = _mapper.Map(list);

        return listDto.ToList();
    }

    public async Task<PetResponseDto> GetPetForCustomerAsync(int ownerId, int petId)
    {
        _logger.Information($"Get pet {petId} for customer by id {ownerId}");

        var list = await _petRepo.GetAllPetsByCustomerIdAsync(ownerId);

        var pet = list.Where(e => e.Id == petId).FirstOrDefault();

        if (pet == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND, ResponseMessageConstantsPet.PET_NOT_FOUND,
                StatusCodes.Status404NotFound);
        }

        return _mapper.Map(pet);
    }

    public async Task<PetResponseDto> GetPetByIdAsync(int id)
    {
        _logger.Information("Get pet by id");

        var pet = await _petRepo.GetSingleAsync(p => p.Id == id);
        if (pet == null)
        {
            throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsPet.PET_NOT_FOUND,
                StatusCodes.Status400BadRequest);
        }

        var petDto = _mapper.Map(pet);

        var owner = await _userService.GetByIdAsync(pet.OwnerID);

        petDto.OwnerName = owner.FullName;

        return petDto;
    }

    public async Task CreatePetAsync(PetRequestDto pet, int ownerId)
    {
        _logger.Information("Create pet {@dto} by user {ownerId}", pet, ownerId);
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

    public async Task UpdatePetAsync(PetUpdateRequestDto pet, int ownerId)
    {
        _logger.Information("Update pet {@dto} by user {ownerId}", pet, ownerId);
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

    public async Task DeletePetAsync(int id, int ownerId)
    {
        _logger.Information("Delete pet by id {id} by user {ownerId}", id, ownerId);
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
}