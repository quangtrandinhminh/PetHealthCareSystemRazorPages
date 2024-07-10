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
            t => t.Customer).OrderByDescending(t => t.CreatedTime);
        if (transactions == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND, ResponseMessageConstantsTransaction.TRANSACTION_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        var response = _mapper.Map(transactions);
        var responsePages = await PaginatedList<TransactionResponseDto>.CreateAsync(response, pageNumber, pageSize);
        // for each response in reponse page set customer name
        foreach (var transaction in responsePages.Items)
        {
            var customer = await _userRepository.GetSingleAsync(u => u.Id == transaction.CustomerId);
            var createBy = await _userRepository.GetSingleAsync(u => u.Id == transaction.CreatedBy);
            var updateBy = await _userRepository.GetSingleAsync(u => u.Id == transaction.LastUpdatedBy);
            transaction.CustomerName = customer?.FullName ?? string.Empty;
            transaction.CreatedByName = createBy?.FullName ?? string.Empty;
            transaction.LastUpdatedByName = updateBy?.FullName ?? string.Empty;
        }

        return responsePages;
    }

    public async Task<PaginatedList<TransactionResponseDto>> GetTransactionsByCustomerIdAsync(int customerId, int pageNumber, int pageSize)
    {
        _logger.Information($"Get all transactions by customer {customerId}");
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

    public async Task<TransactionResponseDto> GetTransactionByAppointmentIdAsync(int appointmentId)
    {
        _logger.Information("Get transaction by appointment {@appointmentId}", appointmentId);
        var transaction = await _transactionRepository.GetSingleAsync(t => t.AppointmentId == appointmentId);
        if (transaction == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                               ResponseMessageConstantsTransaction.TRANSACTION_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        var response = _mapper.Map(transaction);
        return response;
    }

    public async Task<TransactionResponseDto> GetTransactionByMedicalRecordIdAsync(int medicalRecordId)
    {
        _logger.Information("Get transaction by medical record {@medicalRecordId}", medicalRecordId);
        var transaction = await _transactionRepository.GetSingleAsync(t => t.MedicalRecordId == medicalRecordId);
        if (transaction == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                                              ResponseMessageConstantsTransaction.TRANSACTION_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        var response = _mapper.Map(transaction);
        return response;
    }
    public async Task<TransactionResponseWithDetailsDto> GetTransactionByIdAsync(int transactionId)
    {
        _logger.Information($"Get transaction {transactionId}");

        var transaction = await _transactionRepository.GetSingleAsync(t => t.Id == transactionId,
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
        _logger.Information("Create transaction {@dto} by {@userId}", dto, userId);

        #region validate dto
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

        if (dto.AppointmentId == null && dto.MedicalRecordId == null)
        {
            throw new AppException(ResponseCodeConstants.BAD_REQUEST
                , ResponseMessageConstantsTransaction.INVALID_TRANSACTION, StatusCodes.Status400BadRequest);
        }

        if (dto.AppointmentId == null && dto.MedicalRecordId != null)
        {
            var transactionByMedicalRecord = await _transactionRepository.GetSingleAsync(t => t.MedicalRecordId == dto.MedicalRecordId);
            if (transactionByMedicalRecord != null)
            {
                throw new AppException(ResponseCodeConstants.EXISTED,
                                       ResponseMessageConstantsTransaction.TRANSACTION_EXISTED + $" cho lịch hẹn số {dto.AppointmentId}", 
                                       StatusCodes.Status400BadRequest);
            }
        }

        if (dto.AppointmentId != null && dto.MedicalRecordId == null)
        {
            var transactionByAppointment = await _transactionRepository.GetSingleAsync(t => t.AppointmentId == dto.AppointmentId);
            if (transactionByAppointment != null)
            {
                throw new AppException(ResponseCodeConstants.EXISTED,
                                                          ResponseMessageConstantsTransaction.TRANSACTION_EXISTED + $" cho hồ sơ bệnh án số {dto.MedicalRecordId}", 
                                                          StatusCodes.Status400BadRequest);
            }
        }

        var appointmentTask = dto.AppointmentId != null
            ? _appointmentRepository.GetSingleAsync(a => a.Id == dto.AppointmentId)
            : Task.FromResult<Appointment>(null);

        var medicalRecordTask = dto.MedicalRecordId != null
            ? _medicalRecordRepository.GetSingleAsync(mr => mr.Id == dto.MedicalRecordId)
            : Task.FromResult<MedicalRecord>(null);

        var appointment = await appointmentTask;
        var medicalRecord = await medicalRecordTask;

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

        if ((dto.MedicalItems == null || dto.MedicalItems.Count == 0) && (dto.Services == null || dto.Services.Count == 0))
        {
            throw new AppException(ResponseCodeConstants.BAD_REQUEST,
                ResponseMessageConstantsTransaction.TRANSACTION_DETAIL_REQUIRED,
                StatusCodes.Status400BadRequest);
        }
        

        #endregion
        

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
        var serviceDetails = new List<TransactionDetail>();
        if (dto.Services != null)
        {
            serviceDetails = await CheckServicesAsync(dto.Services, _serviceRepository);
        }

        // for each medical item in list, create transaction detail
        var medicalItemDetails = new List<TransactionDetail>();
        if (dto.MedicalItems != null)
        {
            medicalItemDetails = await CheckMedicalItemsAsync(dto.MedicalItems, _medicalItemRepository);
        }

        var transactionDetails = serviceDetails.Concat(medicalItemDetails).ToList();
        transactionEntity.TransactionDetails = transactionDetails;
        transactionEntity.Total = transactionDetails.Sum(detail => detail.SubTotal);

        await _transactionRepository.AddAsync(transactionEntity);
    }

    public async Task CreateTransactionForHospitalization(TransactionRequestDto dto, int staffId)
    {
        _logger.Information("Create transaction for hospitalization {@dto} by {@staffId}", dto, staffId);
        var userEntity = await _userManager.FindByIdAsync(staffId.ToString());
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

        if (dto.MedicalRecordId != null)
        {
            var transactionByMedicalRecord = await _transactionRepository.GetSingleAsync(t => t.MedicalRecordId == dto.MedicalRecordId);
            if (transactionByMedicalRecord != null)
            {
                throw new AppException(ResponseCodeConstants.EXISTED,
                                                          ResponseMessageConstantsTransaction.TRANSACTION_EXISTED + $" cho lịch hẹn số {dto.AppointmentId}", 
                                                                                                StatusCodes.Status400BadRequest);
            }
        }

        var medicalRecord = await _medicalRecordRepository.GetSingleAsync(mr => mr.Id == dto.MedicalRecordId, false,
            mr => mr.Appointment, mr => mr.Hospitalization);
        var days = 0;
        if (medicalRecord != null && medicalRecord.Hospitalization is not null)
        {
            foreach (var hospitalization in medicalRecord.Hospitalization)
            {
                days++;
            }
        }

        var transaction = _mapper.Map(dto);
        transaction.CreatedBy = userEntity.Id;
        transaction.CustomerId = (int)medicalRecord.Appointment.CreatedBy;
        transaction.PaymentStaffId = userEntity.Id;
        transaction.PaymentStaffName = userEntity.FullName;
        transaction.Total = transaction.TransactionDetails.Sum(detail => detail.SubTotal);
        /*transaction.TransactionDetails.Add(new TransactionDetail
        {
            Name = "Phí nhập viện",
            Quantity = days,
            Price = hospitalizationEntity.Price,
            SubTotal = hospitalizationEntity.Price * hospitalization.Quantity,
            TransactionId = dto.Id,
        });*/

        await _transactionRepository.AddAsync(transaction);
    }

    public async Task UpdatePaymentByStaffAsync(int transactionId, int updatedById)
    {
        _logger.Information($"Update payment status for Transaction {transactionId} by Staff {updatedById}");
        var userEntity = await _userManager.FindByIdAsync(updatedById.ToString());
        if (userEntity == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                               ResponseMessageIdentity.INVALID_USER, StatusCodes.Status404NotFound);
        }

        var transaction = await _transactionRepository.GetSingleAsync(t => t.Id == transactionId);
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

        transaction.PaymentDate = DateTime.Now;
        transaction.PaymentStaffId = userEntity.Id;
        transaction.PaymentStaffName = userEntity.FullName;
        transaction.Status = TransactionStatus.Paid;
        transaction.LastUpdatedBy = userEntity.Id;
        transaction.LastUpdatedTime = CoreHelper.SystemTimeNow;

        await _transactionRepository.UpdateAsync(transaction);
    }

    public static async Task<List<TransactionDetail>> CheckMedicalItemsAsync(List<TransactionMedicalItemsDto> medicalItems,
        IMedicalItemRepository medicalItemRepository)
    {
        var medicalItemDetails = new List<TransactionDetail>();
        if (medicalItems.Count > 0)
        {
            foreach (var medicalItem in medicalItems)
            {
                var medicalItemEntity = await medicalItemRepository.GetSingleAsync(m => m.Id == medicalItem.MedicalItemId);
                if (medicalItemEntity == null)
                {
                    throw new AppException(ResponseCodeConstants.NOT_FOUND,
                                               ResponseMessageConstantsMedicalItem.MEDICAL_ITEM_NOT_FOUND + $": {medicalItem.MedicalItemId}",
                                                                      StatusCodes.Status404NotFound);
                }
                medicalItemDetails.Add(new TransactionDetail
                {
                    MedicalItemId = medicalItem.MedicalItemId,
                    Name = medicalItemEntity.Name,
                    Quantity = medicalItem.Quantity,
                    Price = medicalItemEntity.Price,
                    SubTotal = medicalItemEntity.Price * medicalItem.Quantity,
                });
            }
        }
        return medicalItemDetails;
    }

    public static async Task<List<TransactionDetail>> CheckServicesAsync(List<TransactionServicesDto> services,
        IServiceRepository serviceRepository)
    {
        var serviceDetails = new List<TransactionDetail>();
        if (services.Count > 0)
        {
            foreach (var service in services)
            {
                var serviceEntity = await serviceRepository.GetSingleAsync(s => s.Id == service.ServiceId);
                if (serviceEntity == null)
                {
                    throw new AppException(ResponseCodeConstants.NOT_FOUND,
                                                                      ResponseMessageConstantsService.SERVICE_NOT_FOUND + $": {service.ServiceId}",
                                                                                                                                           StatusCodes.Status404NotFound);
                }
                serviceDetails.Add(new TransactionDetail
                {
                    ServiceId = service.ServiceId,
                    Name = serviceEntity.Name,
                    Quantity = service.Quantity,
                    Price = serviceEntity.Price,
                    SubTotal = serviceEntity.Price * service.Quantity,
                });
            }
        }
        return serviceDetails;
    }
}