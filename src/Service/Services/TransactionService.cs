using System.Text.Json.Nodes;
using BusinessObject.DTO;
using BusinessObject.DTO.Transaction;
using BusinessObject.Entities;
using BusinessObject.Entities.Identity;
using BusinessObject.Mapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
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

public class TransactionService(IServiceProvider serviceProvider) : ITransactionService
{
    private readonly IUserRepository _userRepository = serviceProvider.GetRequiredService<IUserRepository>();
    private readonly MapperlyMapper _mapper = serviceProvider.GetRequiredService<MapperlyMapper>();
    private readonly UserManager<UserEntity> _userManager = serviceProvider.GetRequiredService<UserManager<UserEntity>>();
    private readonly ILogger _logger = Log.Logger;
    private readonly ITransactionRepository _transactionRepository = serviceProvider.GetRequiredService<ITransactionRepository>();
    private readonly IAppointmentRepository _appointmentRepository = serviceProvider.GetRequiredService<IAppointmentRepository>();
    private readonly IMedicalRecordRepository _medicalRecordRepository = serviceProvider.GetRequiredService<IMedicalRecordRepository>();
    private readonly IHospitalizationRepository _hospitalizationRepository = serviceProvider.GetRequiredService<IHospitalizationRepository>();
    private readonly IServiceRepository _serviceRepository = serviceProvider.GetRequiredService<IServiceRepository>();
    private readonly IMedicalItemRepository _medicalItemRepository = serviceProvider.GetRequiredService<IMedicalItemRepository>();

    public async Task<PaginatedList<TransactionResponseDto>> GetAllTransactionsAsync(int pageNumber, int pageSize)
    {
        _logger.Information("Get all transactions");
        var transactions = _transactionRepository.GetAllWithCondition(
            t => t.DeletedTime == null,
            t => t.Customer);
        if (transactions == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND, ResponseMessageConstantsTransaction.TRANSACTION_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        var response = _mapper.Map(transactions);
        var reponsePages = await PaginatedList<TransactionResponseDto>.CreateAsync(response, pageNumber, pageSize);
        // for each response in reponse page set customer name
        foreach (var transaction in reponsePages.Items)
        {
            var customer = await _userRepository.GetSingleAsync(u => u.Id == transaction.CustomerId);
            var createBy = await _userRepository.GetSingleAsync(u => u.Id == transaction.CreatedBy);
            var updateBy = await _userRepository.GetSingleAsync(u => u.Id == transaction.LastUpdatedBy);
            transaction.CustomerName = customer?.FullName ?? string.Empty;
            transaction.CreatedByName = createBy?.FullName ?? string.Empty;
            transaction.LastUpdatedByName = updateBy?.FullName ?? string.Empty;
        }

        return reponsePages;
    }

    public async Task<PaginatedList<TransactionResponseDto>> GetTransactionsByCustomerIdAsync(int customerId, int pageNumber, int pageSize)
    {
        _logger.Information("Get all transactions by customer id");
        var transactions = _transactionRepository.GetAllWithCondition(t =>
            t.CustomerId == customerId && t.DeletedTime == null,
            t => t.Customer);
        if (transactions == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                ResponseMessageConstantsTransaction.TRANSACTION_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        var response = _mapper.Map(transactions);
        return await PaginatedList<TransactionResponseDto>.CreateAsync(response, pageNumber, pageSize);
    }

    public async Task<TransactionResponseWithDetailsDto> GetTransactionByIdAsync(int id)
    {
        _logger.Information("Get transaction by id");

        var transaction = await _transactionRepository.GetSingleAsync(t => t.Id == id,
            false, t => t.TransactionDetails,
            t => t.Customer);
        if (transaction == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND
                , ResponseMessageConstantsTransaction.TRANSACTION_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        var response = _mapper.TransactionToTransactionResponseWithDetails(transaction);
        response.TransactionDetails = _mapper.Map(transaction.TransactionDetails.ToList());
        var customer = await _userRepository.GetSingleAsync(u => u.Id == transaction.CustomerId);
        var createBy = await _userRepository.GetSingleAsync(u => u.Id == transaction.CreatedBy);
        var updateBy = await _userRepository.GetSingleAsync(u => u.Id == transaction.LastUpdatedBy);
        response.CustomerName = customer?.FullName ?? string.Empty;
        response.CreatedByName = createBy?.FullName ?? string.Empty;
        response.LastUpdatedByName = updateBy?.FullName ?? string.Empty;

        return response;
    }

    // get all dropdown data for transaction
    public TransactionDropdownDto GetTransactionDropdownData()
    {
        _logger.Information("Get all dropdown data for transaction");
        var paymentMethods = Enum.GetValues(typeof(PaymentMethod))
            .Cast<PaymentMethod>()
            .Select(e => new EnumResponseDto() { Id = (int)e, Value = e.ToString() })
            .ToList();

        var transactionStatuses = Enum.GetValues(typeof(TransactionStatus))
            .Cast<TransactionStatus>()
            .Select(e => new EnumResponseDto { Id = (int)e, Value = e.ToString() })
            .ToList();

        var response = new TransactionDropdownDto
        {
            PaymentMethods = paymentMethods,
            TransactionStatus = transactionStatuses
        };

        return response;
    }

    public async Task CreateTransactionAsync(TransactionRequestDto dto, int userId)
    {
        _logger.Information("Create transaction {@dto}", dto);
        var userEntity = await _userManager.FindByIdAsync(userId.ToString());
        if (userEntity == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                ResponseMessageIdentity.INVALID_USER, StatusCodes.Status404NotFound);
        }

        if (dto.Status == (int)TransactionStatus.Cancelled || dto.Status == (int)TransactionStatus.Refund)
        {
            throw new AppException(ResponseCodeConstants.BAD_REQUEST,
                ResponseMessageConstantsTransaction.INVALID_TRANSACTION_STATUS,
                StatusCodes.Status400BadRequest);
        }

        if (dto.Status == (int)TransactionStatus.Paid)
        {
            if (dto.PaymentMethod != (int)PaymentMethod.Cash)
            {
                throw new AppException(ResponseCodeConstants.BAD_REQUEST,
                    ResponseMessageConstantsTransaction.INVALID_PAYMENT_METHOD,
                    StatusCodes.Status400BadRequest);
            }

            if (string.IsNullOrEmpty(dto.PaymentId) || string.IsNullOrEmpty(dto.PaymentDate.ToString()))
            {
                throw new AppException(ResponseCodeConstants.BAD_REQUEST,
                                       ResponseMessageConstantsTransaction.PAYMENT_REQUIRED,
                                                          StatusCodes.Status400BadRequest);
            }

        }

        /*if (dto.AppointmentId == null && dto.MedicalRecordId == null &&
            dto.HospitalizationId == null)
        {
            throw new AppException(ResponseCodeConstants.BAD_REQUEST
                , ResponseMessageConstantsTransaction.INVALID_TRANSACTION, StatusCodes.Status400BadRequest);
        }*/

        var appointmentTask = dto.AppointmentId != null
            ? _appointmentRepository.GetSingleAsync(a => a.Id == dto.AppointmentId)
            : Task.FromResult<Appointment>(null);

        var medicalRecordTask = dto.MedicalRecordId != null
            ? _medicalRecordRepository.GetSingleAsync(mr => mr.Id == dto.MedicalRecordId)
            : Task.FromResult<MedicalRecord>(null);

        var hospitalizationTask = dto.HospitalizationId != null
            ? _hospitalizationRepository.GetSingleAsync(h => h.Id == dto.HospitalizationId)
            : Task.FromResult<Hospitalization>(null);

        var appointment = await appointmentTask;
        var medicalRecord = await medicalRecordTask;
        var hospitalization = await hospitalizationTask;

        if (dto.AppointmentId != null && appointment == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                ResponseMessageConstantsAppointment.APPOINTMENT_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        if (dto.MedicalRecordId != null && medicalRecord == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                ResponseMessageConstantsMedicalRecord.MEDICAL_RECORD_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        if (dto.HospitalizationId != null && hospitalization == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                ResponseMessageConstantsHospitalization.HOSPITALIZATION_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        if ((dto.MedicalItems == null || dto.MedicalItems.Count == 0) && (dto.Services == null || dto.Services.Count == 0))
        {
            throw new AppException(ResponseCodeConstants.BAD_REQUEST,
                ResponseMessageConstantsTransaction.TRANSACTION_DETAIL_REQUIRED,
                StatusCodes.Status400BadRequest);
        }

        var transactionEntity = _mapper.Map(dto);
        transactionEntity.CreatedBy = userEntity.Id;
        // use userManager to check if list of roles contains staff or customer
        var roles = await _userManager.GetRolesAsync(userEntity);
        if (roles.Contains(UserRole.Staff.ToString()))
        {
            transactionEntity.PaymentStaffId = userEntity.Id;
            transactionEntity.PaymentStaffName = userEntity.FullName;
        }
        else if (roles.Contains(UserRole.Customer.ToString()))
        {
            transactionEntity.CustomerId = userEntity.Id;
        }

        // for each service in list, create transaction detail
        var serviceTasks = dto.Services?.Select(async service =>
        {
            var serviceEntity = await _serviceRepository.GetSingleAsync(s => s.Id == service.ServiceId);
            if (serviceEntity == null)
            {
                throw new AppException(ResponseCodeConstants.NOT_FOUND,
                    ResponseMessageConstantsService.SERVICE_NOT_FOUND + $": {service.ServiceId}",
                    StatusCodes.Status404NotFound);
            }
            return new TransactionDetail
            {
                ServiceId = service.ServiceId,
                Name = serviceEntity.Name,
                Quantity = service.Quantity,
                Price = serviceEntity.Price,
                SubTotal = serviceEntity.Price * service.Quantity,
                TransactionId = transactionEntity.Id,
            };
        }).ToArray() ?? [];

        // for each medical item in list, create transaction detail
        var medicalItemTasks = dto.MedicalItems?.Select(async medicalItem =>
        {
            var medicalItemEntity = await _medicalItemRepository.GetSingleAsync(m => m.Id == medicalItem.MedicalItemId);
            if (medicalItemEntity == null)
            {
                throw new AppException(ResponseCodeConstants.NOT_FOUND,
                    ResponseMessageConstantsMedicalItem.MEDICAL_ITEM_NOT_FOUND + $": {medicalItem.MedicalItemId}",
                    StatusCodes.Status404NotFound);
            }
            return new TransactionDetail
            {
                MedicalItemId = medicalItem.MedicalItemId,
                Name = medicalItemEntity.Name,
                Quantity = medicalItem.Quantity,
                Price = medicalItemEntity.Price,
                SubTotal = medicalItemEntity.Price * medicalItem.Quantity,
                TransactionId = transactionEntity.Id,
            };
        }).ToArray() ?? [];

        var transactionDetails = (await Task.WhenAll(serviceTasks))
            .Concat(await Task.WhenAll(medicalItemTasks)).ToList();
        transactionEntity.TransactionDetails = transactionDetails;
        transactionEntity.Total = transactionDetails.Sum(detail => detail.SubTotal);

        await _transactionRepository.AddAsync(transactionEntity);
    }

    public async Task UpdatePaymentByStaffAsync(int id, int userId)
    {
        _logger.Information($"Update payment status for Transaction {id} by Staff {userId}");
        var userEntity = await _userManager.FindByIdAsync(userId.ToString());
        if (userEntity == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                               ResponseMessageIdentity.INVALID_USER, StatusCodes.Status404NotFound);
        }

        var transaction = await _transactionRepository.GetSingleAsync(t => t.Id == id);
        if (transaction == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                               ResponseMessageConstantsTransaction.TRANSACTION_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        if (transaction.Status == TransactionStatus.Paid)
        {
            throw new AppException(ResponseCodeConstants.BAD_REQUEST,
                               ResponseMessageConstantsTransaction.TRANSACTION_PAID,
                                              StatusCodes.Status400BadRequest);
        }

        transaction.PaymentDate = CoreHelper.SystemTimeNow;
        transaction.PaymentStaffId = userEntity.Id;
        transaction.PaymentStaffName = userEntity.FullName;
        transaction.Status = TransactionStatus.Paid;

        await _transactionRepository.UpdateAsync(transaction);
    }
}