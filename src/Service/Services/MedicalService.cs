using System.Numerics;
using BusinessObject;
using BusinessObject.DTO.MedicalItem;
using BusinessObject.DTO.MedicalRecord;
using BusinessObject.DTO.Transaction;
using BusinessObject.Entities;
using BusinessObject.Entities.Identity;
using BusinessObject.Mapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository.Extensions;
using Repository.Interfaces;
using Serilog;
using Service.IServices;
using Utility.Constants;
using Utility.Enum;
using Utility.Exceptions;
using Utility.Helpers;

namespace Service.Services;

public class MedicalService(IServiceProvider serviceProvider) : IMedicalService
{
    private readonly ILogger _logger = Log.Logger;
    private readonly MapperlyMapper _mapper = serviceProvider.GetRequiredService<MapperlyMapper>();
    private readonly IMedicalItemRepository _medicalItemRepository = serviceProvider.GetRequiredService<IMedicalItemRepository>();
    private readonly IMedicalRecordRepository _medicalRecordRepository = serviceProvider.GetRequiredService<IMedicalRecordRepository>();
    private readonly IUserRepository _userRepository = serviceProvider.GetRequiredService<IUserRepository>();
    private readonly UserManager<UserEntity> _userManager = serviceProvider.GetRequiredService<UserManager<UserEntity>>();
    private readonly IPetRepository _petRepository = serviceProvider.GetRequiredService<IPetRepository>();
    private readonly IAppointmentRepository _appointmentRepository = serviceProvider.GetRequiredService<IAppointmentRepository>();
    private readonly ITransactionRepository _transactionRepository = serviceProvider.GetRequiredService<ITransactionRepository>();
    private readonly ITransactionService _transactionService = serviceProvider.GetRequiredService<ITransactionService>();

    // medical item -----------------------------------------------------------------------------------------------------------------------------------------------------------
    public async Task<List<MedicalResponseDto>> GetAllMedicalItem()
    {
        var list = await _medicalItemRepository.GetAllWithCondition(m => m.DeletedTime == null).ToListAsync();
        if (list == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                               ResponseMessageConstantsMedicalItem.MEDICAL_ITEM_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        var listDto = _mapper.Map(list);

        return listDto.ToList();
    }

    public async Task<PaginatedList<MedicalResponseDto>> GetAllMedicalItem(int pageNumber, int pageSize)
    {
        _logger.Information("Get all medical item");
        var medicalItems = _medicalItemRepository.GetAllWithCondition(m => m.DeletedTime == null)
            .OrderByDescending(m => m.CreatedTime);
        if (medicalItems == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                                              ResponseMessageConstantsMedicalItem.MEDICAL_ITEM_NOT_FOUND, StatusCodes.Status404NotFound);
        }
        var response = _mapper.Map(medicalItems);
        var paginatedList = await PaginatedList<MedicalResponseDto>.CreateAsync(response, pageNumber, pageSize);
        return paginatedList;
    }

    public async Task<MedicalResponseDto> GetMedicalItemById(int medicalItemId)
    {
        _logger.Information($"Get medical item medicalItemId {medicalItemId}");
        var medicalItem = await _medicalItemRepository.GetSingleAsync(m => m.Id == medicalItemId);
        if (medicalItem == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                                              ResponseMessageConstantsMedicalItem.MEDICAL_ITEM_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        var createdBy = await _userRepository.GetSingleAsync(u => u.Id == medicalItem.CreatedBy);
        var lastUpdatedBy = await _userRepository.GetSingleAsync(u => u.Id == medicalItem.LastUpdatedBy);

        var response = _mapper.Map(medicalItem);
        response.CreatedByName = createdBy?.FullName ?? string.Empty;
        response.LastUpdatedByName = lastUpdatedBy?.FullName ?? string.Empty;
        return response;
    }

    public async Task CreateMedicalItem(MedicalItemRequestDto medicalItem, int createdById)
    {
        var user = await _userManager.FindByIdAsync(createdById.ToString());
        if (user == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                                              ResponseMessageIdentity.INVALID_USER, StatusCodes.Status404NotFound);
        }

        var medicalItemEntity = _mapper.Map(medicalItem);
        medicalItemEntity.CreatedBy = medicalItemEntity.LastUpdatedBy = createdById;
        medicalItemEntity.CreatedTime = medicalItemEntity.LastUpdatedTime = CoreHelper.SystemTimeNow;

        await _medicalItemRepository.AddAsync(medicalItemEntity);
    }

    public async Task DeleteMedicalItem(int id, int deleteBy)
    {
        var user = await _userManager.FindByIdAsync(deleteBy.ToString());
        if (user == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                               ResponseMessageIdentity.INVALID_USER, StatusCodes.Status404NotFound);
        }

        var medicalItem = await _medicalItemRepository.GetSingleAsync(m => m.Id == id);
        if (medicalItem == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                               ResponseMessageConstantsMedicalItem.MEDICAL_ITEM_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        medicalItem.DeletedBy = deleteBy;
        medicalItem.DeletedTime = CoreHelper.SystemTimeNow;
        await _medicalItemRepository.UpdateAsync(medicalItem);
    }

    public async Task UpdateMedicalItem(MedicalItemUpdateDto dto, int updatedById)
    {
        var user = await _userManager.FindByIdAsync(updatedById.ToString());
        if (user == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                ResponseMessageIdentity.INVALID_USER, StatusCodes.Status404NotFound);
        }

        var medicalItem = await _medicalItemRepository.GetSingleAsync(m => m.Id == dto.Id);
        if (medicalItem == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                ResponseMessageConstantsMedicalItem.MEDICAL_ITEM_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        medicalItem.Name = dto.Name;
        medicalItem.Description = dto.Description;
        medicalItem.Price = dto.Price;
        medicalItem.LastUpdatedBy = updatedById;
        medicalItem.LastUpdatedTime = CoreHelper.SystemTimeNow;

        await _medicalItemRepository.UpdateAsync(medicalItem);
    }

    // medical record -----------------------------------------------------------------------------------------------------------------------------------------------------------
    public async Task UpdateMedicalRecord(MedicalRecordRequestDto dto, int updatedById)
    {
        var user = await _userManager.FindByIdAsync(updatedById.ToString());
        if (user == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                ResponseMessageIdentity.INVALID_USER, StatusCodes.Status404NotFound);
        }

        var medicalRecord = await _medicalRecordRepository.GetSingleAsync(m => m.Id == dto.Id);
        if (medicalRecord == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                ResponseMessageConstantsMedicalRecord.MEDICAL_RECORD_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        medicalRecord.DischargeDate = dto.DischargeDate;
        medicalRecord.LastUpdatedBy = updatedById;
        medicalRecord.LastUpdatedTime = CoreHelper.SystemTimeNow;

        await _medicalRecordRepository.UpdateAsync(medicalRecord);
    }
    public async Task<PaginatedList<MedicalRecordResponseDto>> GetAllMedicalRecord(int pageNumber, int pageSize)
    {
        _logger.Information("Get all medical record");
        var medicalRecords = _medicalRecordRepository.GetAllWithCondition(mr =>
            mr.DeletedTime == null, mr => mr.Pet).OrderByDescending(mr => mr.CreatedTime);
        if (medicalRecords == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                ResponseMessageConstantsMedicalRecord.MEDICAL_RECORD_NOT_FOUND, StatusCodes.Status404NotFound);
        }
        var response = _mapper.Map(medicalRecords);
        var paginatedList = await PaginatedList<MedicalRecordResponseDto>.CreateAsync(response, pageNumber, pageSize);
        foreach (var item in paginatedList.Items)
        {
            var vet = await _userRepository.GetSingleAsync(u => u.Id == item.VetId);
            var createBy = await _userRepository.GetSingleAsync(u => u.Id == item.CreatedBy);
            var updateBy = await _userRepository.GetSingleAsync(u => u.Id == item.LastUpdatedBy);
            item.VetName = vet?.FullName ?? string.Empty;
            item.CreatedByName = createBy?.FullName ?? string.Empty;
            item.LastUpdatedByName = updateBy?.FullName ?? string.Empty;
        }
        return paginatedList;
    }

    public async Task<PaginatedList<MedicalRecordResponseDto>> GetAllMedicalRecordForHospitalization(int pageNumber, int pageSize)
    {
        _logger.Information("Get all medical record for hospitalization");
        var medicalRecords = _medicalRecordRepository.GetAllWithCondition(mr =>
                       mr.AdmissionDate != null && mr.DischargeDate != null, mr => mr.Pet).OrderByDescending(mr => mr.CreatedTime);
        if (medicalRecords == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                               ResponseMessageConstantsMedicalRecord.MEDICAL_RECORD_NOT_FOUND, StatusCodes.Status404NotFound);
        }
        var response = _mapper.Map(medicalRecords);
        var paginatedList = await PaginatedList<MedicalRecordResponseDto>.CreateAsync(response, pageNumber, pageSize); foreach (var item in paginatedList.Items)
        {
            var vet = await _userRepository.GetSingleAsync(u => u.Id == item.VetId);
            var createBy = await _userRepository.GetSingleAsync(u => u.Id == item.CreatedBy);
            var updateBy = await _userRepository.GetSingleAsync(u => u.Id == item.LastUpdatedBy);
            item.VetName = vet?.FullName ?? string.Empty;
            item.CreatedByName = createBy?.FullName ?? string.Empty;
            item.LastUpdatedByName = updateBy?.FullName ?? string.Empty;
        }
        return paginatedList;
    }

    public async Task<PaginatedList<MedicalRecordResponseDto>> GetAllMedicalRecordByPetId(int petId, int pageNumber, int pageSize)
    {
        _logger.Information($"Get all medical record by pet id {petId}");
        var medicalRecords = _medicalRecordRepository.GetAllWithCondition(mr =>
                       mr.PetId == petId, mr => mr.Pet).OrderByDescending(mr => mr.CreatedTime);
        if (medicalRecords == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                                              ResponseMessageConstantsMedicalRecord.MEDICAL_RECORD_NOT_FOUND, StatusCodes.Status404NotFound);
        }
        var response = _mapper.Map(medicalRecords);
        var paginatedList = await PaginatedList<MedicalRecordResponseDto>.CreateAsync(response, pageNumber, pageSize);
        foreach (var item in paginatedList.Items)
        {
            var vet = await _userRepository.GetSingleAsync(u => u.Id == item.VetId);
            var createBy = await _userRepository.GetSingleAsync(u => u.Id == item.CreatedBy);
            var updateBy = await _userRepository.GetSingleAsync(u => u.Id == item.LastUpdatedBy);
            item.VetName = vet?.FullName ?? string.Empty;
            item.CreatedByName = createBy?.FullName ?? string.Empty;
            item.LastUpdatedByName = updateBy?.FullName ?? string.Empty;
        }
        return paginatedList;
    }

    public async Task<MedicalRecordResponseDtoWithDetails> GetMedicalRecordById(int medicalRecordId)
    {
        _logger.Information($"Get medical record id {medicalRecordId}");
        var medicalRecord = await _medicalRecordRepository.GetSingleAsync(mr => mr.Id == medicalRecordId,
            false, mr => mr.MedicalItems, mr => mr.Pet);
        if (medicalRecord == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                               ResponseMessageConstantsMedicalRecord.MEDICAL_RECORD_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        var response = await MedicalRecordToMedicalRecordResponseDtoWithDetails(medicalRecord);
        return response;
    }

    public async Task<MedicalRecordResponseDtoWithDetails> GetMedicalRecordByPetIdAndAppointmentId(int petId, int appointmentId)
    {
        _logger.Information($"Get medical record with {petId} and {appointmentId}");
        var medicalRecord = await _medicalRecordRepository.GetSingleAsync(mr => mr.AppointmentId == appointmentId && mr.PetId == petId,
            false, mr => mr.MedicalItems, mr => mr.Pet);
        if (medicalRecord == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                ResponseMessageConstantsMedicalRecord.MEDICAL_RECORD_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        var response = await MedicalRecordToMedicalRecordResponseDtoWithDetails(medicalRecord);

        return response;
    }

    public async Task<MedicalRecordResponseDtoWithDetails> CreateMedicalRecord(MedicalRecordRequestDto dto, int vetId)
    {
        _logger.Information("Create medical record {@dto} by vet id {@vetId}", dto, vetId);

        #region validate dto
        var vet = await _userManager.FindByIdAsync(vetId.ToString());
        if (vet == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                ResponseMessageIdentity.INVALID_USER, StatusCodes.Status404NotFound);
        }

        var appointment = await _appointmentRepository.GetSingleAsync(u => u.Id == dto.AppointmentId, false, 
            a => a.AppointmentPets, a => a.TimeTable);
        if (appointment == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                               ResponseMessageConstantsAppointment.APPOINTMENT_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        if (appointment.Status == AppointmentStatus.Completed)
        {
            throw new AppException(ResponseCodeConstants.BAD_REQUEST,
                                              ResponseMessageConstantsAppointment.APPOINTMENT_COMPLETED, StatusCodes.Status400BadRequest);
        }

        var pet = await _petRepository.GetSingleAsync(p => p.Id == dto.PetId);
        if (pet == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                                              ResponseMessageConstantsPet.PET_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        // check if pet is in the appointment
        var appointmentPet = appointment.AppointmentPets.FirstOrDefault(ap => ap.PetId == pet.Id);
        if (appointmentPet == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                                   ResponseMessageConstantsAppointment.APPOINTMENT_PET_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        // check if pet have medical record for this appointment
        var existedMedicalRecord = await _medicalRecordRepository.GetSingleAsync(mr =>
                       mr.AppointmentId == dto.AppointmentId && mr.PetId == dto.PetId);
        if (existedMedicalRecord != null)
        {
            throw new AppException(ResponseCodeConstants.BAD_REQUEST,
                                              ResponseMessageConstantsMedicalRecord.MEDICAL_RECORD_EXISTED, StatusCodes.Status400BadRequest);
        }

        // check next appointment valid
        if (dto.NextAppointment != null && dto.NextAppointment < DateTime.Now)
        {
            throw new AppException(ResponseCodeConstants.BAD_REQUEST,
                               ResponseMessageConstantsMedicalRecord.NEXT_APPOINTMENT_INVALID, StatusCodes.Status400BadRequest);
        }

        // check pet weight valid
        if (dto.PetWeight <= 0)
        {
            throw new AppException(ResponseCodeConstants.BAD_REQUEST,
                                              ResponseMessageConstantsMedicalRecord.PET_WEIGHT_INVALID, StatusCodes.Status400BadRequest);
        }

        // parse appointment date and time in timetable to datetimeoffset
        var appointmentDate = appointment.AppointmentDate;
        var appointmentTime = appointment.TimeTable.StartTime;
        var appointmentDateTime = new DateTimeOffset(appointmentDate.Year, appointmentDate.Month, appointmentDate.Day,
                       appointmentTime.Hour, appointmentTime.Minute, appointmentTime.Second, TimeSpan.Zero);
        if (dto.AdmissionDate != null && dto.AdmissionDate < appointmentDateTime)
        {
            throw new AppException(ResponseCodeConstants.BAD_REQUEST,
                                              ResponseMessageConstantsMedicalRecord.ADMISSION_DATE_INVALID, StatusCodes.Status400BadRequest);
        }

        if (dto.DischargeDate != null && dto.DischargeDate < appointmentDateTime)
        {
            throw new AppException(ResponseCodeConstants.BAD_REQUEST,
                                                             ResponseMessageConstantsMedicalRecord.DISCHARGE_DATE_INVALID, StatusCodes.Status400BadRequest);
        }


        #endregion
        var customerId = pet.OwnerID;

        var medicalRecord = _mapper.Map(dto);
        medicalRecord.PetId = pet.Id;
        medicalRecord.VetId = vetId;
        medicalRecord.CreatedBy = medicalRecord.LastUpdatedBy = vetId;
        medicalRecord.CreatedTime = medicalRecord.LastUpdatedTime = medicalRecord.Date = CoreHelper.SystemTimeNow;
        medicalRecord.MedicalItems = new List<MedicalItem>();
        Transaction? transaction = null;
        if (dto.MedicalItems is { Count: > 0 })
        {
            transaction = new Transaction
            {
                CustomerId = customerId,
                CreatedBy = vetId,
                LastUpdatedBy = vetId,
                CreatedTime = CoreHelper.SystemTimeNow,
                LastUpdatedTime = CoreHelper.SystemTimeNow,
                Status = TransactionStatus.Pending,
                TransactionDetails = new List<TransactionDetail>()
            };

            foreach (var item in dto.MedicalItems)
            {
                transaction.TransactionDetails = await TransactionService.CheckMedicalItemsAsync(dto.MedicalItems, _medicalItemRepository);
                transaction.Total = transaction.TransactionDetails.Sum(detail => detail.SubTotal);

                medicalRecord.MedicalItems.Add(new MedicalItem()
                {
                    Id = item.MedicalItemId,
                });
            }
        }

        var addedMedicalRecord = await _medicalRecordRepository.AddMedicalRecordAsync(medicalRecord);

        if (transaction != null)
        {
            transaction.MedicalRecordId = addedMedicalRecord.Id;
            await _transactionRepository.AddAsync(transaction);
        }
        return await GetMedicalRecordById(addedMedicalRecord.Id);
    }

    private async Task<MedicalRecordResponseDtoWithDetails> MedicalRecordToMedicalRecordResponseDtoWithDetails(MedicalRecord medicalRecord)
    {
        var response = _mapper.MedicalRecordToMedicalRecordResponseDtoWithDetails(medicalRecord);
        var vet = await _userRepository.GetSingleAsync(u => u.Id == response.VetId);
        var createBy = await _userRepository.GetSingleAsync(u => u.Id == response.CreatedBy);
        var updateBy = await _userRepository.GetSingleAsync(u => u.Id == response.LastUpdatedBy);
        response.VetName = vet?.FullName ?? string.Empty;
        response.CreatedByName = createBy?.FullName ?? string.Empty;
        response.LastUpdatedByName = updateBy?.FullName ?? string.Empty;
        if (response.MedicalItems != null)
        {
            var transaction = await _transactionRepository.GetSingleAsync(t => t.MedicalRecordId == medicalRecord.Id, false, t => t.TransactionDetails);
            foreach (var item in response.MedicalItems)
            {
                item.Quantity = (int)(transaction.TransactionDetails.FirstOrDefault(td => td.MedicalItemId == item.Id)?.Quantity ?? null)!;
            }
        }

        return response;
    }
}