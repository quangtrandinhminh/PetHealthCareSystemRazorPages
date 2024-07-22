using System.Text.Json.Nodes;
using BusinessObject.DTO;
using BusinessObject.DTO.Configuration;
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
using Microsoft.IdentityModel.Tokens;
using Net.payOS;
using Net.payOS.Types;
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
    private readonly IConfigurationRepository _configurationRepository = serviceProvider.GetRequiredService<IConfigurationRepository>();
    private readonly IConfigurationService _configurationService = serviceProvider.GetRequiredService<IConfigurationService>();

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

    public async Task<PaginatedList<TransactionResponseDto>> GetTransactionsByFilterAsync(TransactionFilterDto filter,
        int pageNumber, int pageSize)
    {
        _logger.Information("Get all transactions by filter {@filter}", filter);
        var transactions = _transactionRepository.GetAllWithCondition(t => t.DeletedTime == null);
        if (transactions == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                               ResponseMessageConstantsTransaction.TRANSACTION_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        if (filter.CustomerId != null)
        {
            transactions = transactions.Where(t => t.CustomerId == filter.CustomerId);
        }

        if (filter.AppointmentId != null)
        {
            transactions = transactions.Where(t => t.AppointmentId == filter.AppointmentId);
        }

        if (filter.MedicalRecordId != null)
        {
            transactions = transactions.Where(t => t.MedicalRecordId == filter.MedicalRecordId);
        }

        if (filter.PaymentDate != null && DateTimeOffset.TryParse(filter.PaymentDate, out var date))
        {
            transactions = transactions.Where(t => t.PaymentDate == date);
        }
        else if (filter.FromPaymentDate != null && DateTimeOffset.TryParse(filter.FromPaymentDate, out var fromDate)
                                                &&
                                                filter.ToPaymentDate != null &&
                                                DateTimeOffset.TryParse(filter.ToPaymentDate, out var toDate))
        {
            transactions = transactions.Where(t => t.PaymentDate >= fromDate && t.PaymentDate <= toDate);
        }

        if (filter.PaymentMethod != null)
        {
            transactions = transactions.Where(t => t.PaymentMethod == (PaymentMethod)filter.PaymentMethod);
        }

        if (filter.PaymentStaffId != null)
        {
            transactions = transactions.Where(t => t.PaymentStaffId == filter.PaymentStaffId);
        }
        else if (filter.PaymentStaffName != null)
        {
            transactions = transactions.Where(t => t.PaymentStaffName != null && t.PaymentStaffName.Contains(filter.PaymentStaffName));
        }

        if (filter.IsPending != null)
        {
            transactions = transactions.Where(t => t.Status == TransactionStatus.Pending);
        }

        if (filter.IsPaid != null)
        {
            transactions = transactions.Where(t => t.Status == TransactionStatus.Paid);
        }

        if (filter.IsRefunded != null)
        {
            transactions = transactions.Where(t => t.Status == TransactionStatus.Refund);
        }

        if (filter.IsDecreasingByCreatedTime != null)
        {
            transactions = transactions.OrderByDescending(t => t.CreatedTime);
        }

        var response = _mapper.Map(transactions);
        return await PaginatedList<TransactionResponseDto>.CreateAsync(response, pageNumber, pageSize);
    }

    public async Task CreateTransactionAsync(TransactionRequestDto dto, int userId)
    {
        _logger.Information("Create transaction {@dto} by user {@userId}", dto, userId);

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

        // if status is paid then payment method must be an online method, here is payos
        if (dto.Status == (int)TransactionStatus.Paid)
        {
            if (dto.PaymentMethod != (int)PaymentMethod.Cash && dto.PaymentMethod != (int)PaymentMethod.VnPay)
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
            transactionEntity.CustomerId = userEntity.Id;
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

        if(dto.Status == 2)
        {
            transactionEntity.Status = TransactionStatus.Paid;
        }

        if (dto.PaymentMethod == 2)
        {
            transactionEntity.PaymentMethod = PaymentMethod.VnPay;
        }

        var transactionDetails = serviceDetails.Concat(medicalItemDetails).ToList();
        transactionEntity.TransactionDetails = transactionDetails;
        transactionEntity.Total = transactionDetails.Sum(detail => detail.SubTotal);

        // if paymentId != null CreatePayOsTransaction(price, id)
        // transactionEntity.checkoutUrl = await CreatePayOsTransaction(transactionEntity.Total, transactionEntity.paymentId);
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

        if (dto.MedicalRecordId == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                               ResponseMessageConstantsMedicalRecord.MEDICAL_RECORD_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        var medicalRecord = await _medicalRecordRepository.GetSingleAsync(mr => mr.Id == dto.MedicalRecordId, false,
            mr => mr.Appointment, mr => mr.Hospitalization);
        if (medicalRecord == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                ResponseMessageConstantsMedicalRecord.MEDICAL_RECORD_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        var hospitalizationDays = medicalRecord.Hospitalization.Count;
        if (hospitalizationDays == 0)
        {
            throw new AppException(ResponseCodeConstants.BAD_REQUEST,
                ResponseMessageConstantsHospitalization.MEDICAL_RECORD_NOT_ADMITTED, StatusCodes.Status400BadRequest);
        }

        var hospitalizationPrice = await _configurationRepository.GetValueByKey(ConfigurationKey.HospitalizationPrice);
        if (hospitalizationPrice == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                               ResponseMessageConstantsConfiguration.CONFIGURATION_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        var transaction = _mapper.Map(dto);
        transaction.CreatedBy = transaction.LastUpdatedBy = userEntity.Id;
        transaction.CreatedTime = transaction.LastUpdatedTime = CoreHelper.SystemTimeNow;
        transaction.CustomerId = medicalRecord.Appointment.CustomerId;
        transaction.PaymentStaffId = userEntity.Id;
        transaction.PaymentStaffName = userEntity.FullName;
        transaction.TransactionDetails.Add(new TransactionDetail
        {
            Name = "Phí lưu chuồng",
            Quantity = hospitalizationDays,
            Price = decimal.Parse(hospitalizationPrice.Value),
            SubTotal = hospitalizationDays * decimal.Parse(hospitalizationPrice.Value),
        });
        transaction.Total = transaction.TransactionDetails.Sum(detail => detail.SubTotal);
        // if paymentId != null CreatePayOsTransaction(price, id)
        // transactionEntity.checkoutUrl = await CreatePayOsTransaction(transactionEntity.Total, transactionEntity.paymentId);
        await _transactionRepository.AddAsync(transaction);
    }

    public async Task<HospitalizationPriceResponseDto> CalculateHospitalizationPriceAsync(int medicalRecordId)
    {
        _logger.Information($"Calculate hospitalization price for medical record {medicalRecordId}");
        var medicalRecord = await _medicalRecordRepository.GetSingleAsync(mr => mr.Id == medicalRecordId, false,
                       mr => mr.Hospitalization);
        if (medicalRecord == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                               ResponseMessageConstantsMedicalRecord.MEDICAL_RECORD_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        var hospitalizationDays = medicalRecord.Hospitalization.Count;
        if (hospitalizationDays == 0)
        {
            throw new AppException(ResponseCodeConstants.BAD_REQUEST,
                               ResponseMessageConstantsHospitalization.MEDICAL_RECORD_NOT_ADMITTED, StatusCodes.Status400BadRequest);
        }

        var hospitalizationPrice = await _configurationRepository.GetValueByKey(ConfigurationKey.HospitalizationPrice);
        if (hospitalizationPrice == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                                              ResponseMessageConstantsConfiguration.CONFIGURATION_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        return new HospitalizationPriceResponseDto
        {
            MedicalRecordId = medicalRecordId,
            PricePerDay = decimal.Parse(hospitalizationPrice.Value),
            Days = hospitalizationDays,
            AdmissionDate = medicalRecord.AdmissionDate,
            DischargeDate = medicalRecord.DischargeDate,
            TotalPrice = hospitalizationDays * decimal.Parse(hospitalizationPrice.Value),
        };
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

    public async Task UpdateTransactionToRefundAsync(TransactionRefundRequestDto dto, int updatedById)
    {
        _logger.Information("Update transaction {@dto} to refund by user {updatedById}", dto, updatedById);
        var userEntity = await _userManager.FindByIdAsync(updatedById.ToString());
        if (userEntity == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                               ResponseMessageIdentity.INVALID_USER, StatusCodes.Status404NotFound);
        }

        var transaction = await _transactionRepository.GetSingleAsync(t => t.Id == dto.TransactionId);
        if (transaction == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                                              ResponseMessageConstantsTransaction.TRANSACTION_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        if (transaction.Status == TransactionStatus.Cancelled || transaction.Status == TransactionStatus.Refund)
        {
            throw new AppException(ResponseCodeConstants.BAD_REQUEST,
                                              ResponseMessageConstantsTransaction.INVALID_TRANSACTION_STATUS,
                                                                                           StatusCodes.Status400BadRequest);
        }

        transaction.RefundReason = dto.RefundReason;
        transaction.RefundPercentage = dto.RefundPercentage;
        transaction.RefundPaymentId = dto.RefundPaymentId;
        transaction.RefundDate = dto.RefundDate;
        transaction.Status = TransactionStatus.Refund;
        transaction.LastUpdatedBy = userEntity.Id;
        transaction.LastUpdatedTime = CoreHelper.SystemTimeNow;

        await _transactionRepository.UpdateAsync(transaction);
    }

    public async Task<RefundConditionsResponseDto> GetRefundConditionsAsync()
    {
        throw new NotImplementedException();
        /*_logger.Information("Get refund conditions");
        var refundPercentage = await _configurationRepository.GetValueByKey(ConfigurationKey.RefundPercentage);
        if (refundPercentage == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                               ResponseMessageConstantsConfiguration.CONFIGURATION_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        var refundForDays = await _configurationRepository.GetValueByKey(ConfigurationKey.RefundForDays);
        if (refundForDays == null)
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND,
                ResponseMessageConstantsConfiguration.CONFIGURATION_NOT_FOUND, StatusCodes.Status404NotFound);
        }

        var response = new RefundConditionsResponseDto
        {
            RefundPercentage = decimal.Parse(refundPercentage.Value),
            RefundForDays = int.Parse(refundForDays.Value),
        };

        return response;*/
    }

    public async Task<TransactionPayOsResponseDto> CreatePayOsTransaction()
    {
        var clientId = (await _configurationRepository.GetValueByKey(ConfigurationKey.PayOsClientId)).Value;
        var apiKey = (await _configurationRepository.GetValueByKey(ConfigurationKey.PayOsApiKey)).Value;
        var checksumKey = (await _configurationRepository.GetValueByKey(ConfigurationKey.PayOsChecksumKey)).Value;
        var bookPriceString = (await _configurationRepository.GetValueByKey(ConfigurationKey.BookPrice)).Value;
        var orderIdString = (await _configurationRepository.GetValueByKey(ConfigurationKey.PayOsOrderId)).Value;
        bool bookPriceSuccess = int.TryParse(bookPriceString, out int bookPrice);
        bool OrderIdSuccess = long.TryParse(orderIdString, out long orderId);

        if (bookPrice == 0)
        {
            throw new AppException(ResponseCodeConstants.INTERNAL_SERVER_ERROR, ResponseMessageConstantsCommon.DATA_NOT_ENOUGH);
        }

        if (orderId == 0)
        {
            throw new AppException(ResponseCodeConstants.INTERNAL_SERVER_ERROR, ResponseMessageConstantsCommon.DATA_NOT_ENOUGH);
        }

        if (clientId.IsNullOrEmpty() || apiKey.IsNullOrEmpty() || checksumKey.IsNullOrEmpty())
        {
            throw new AppException(ResponseCodeConstants.INTERNAL_SERVER_ERROR, ResponseMessageConstantsCommon.DATA_NOT_ENOUGH);
        }

        PayOS payOs = new PayOS(clientId, apiKey, checksumKey);

        var itemDataName = $"Thanh toan cuoc hen";
        var itemDataQuantity = 1;
        var itemDataPrice = bookPrice == 0 ? 10000 : bookPrice;

        List<ItemData> items = new()
        {
            new(itemDataName,itemDataQuantity,itemDataPrice),
        };

        PaymentData paymentData = new PaymentData(orderId, bookPrice,
            $"Thanh toan lich hen", items, "", "");

        CreatePaymentResult createPayment = await payOs.createPaymentLink(paymentData);

        await _configurationService.UpdateConfiguration(new ConfigurationUpdateRequestDto()
        {
            Value = (orderId + 1).ToString(),
            Key = ConfigurationKey.PayOsOrderId,
        });

        var response = new TransactionPayOsResponseDto()
        {
            CheckoutUrl = createPayment.checkoutUrl,
            OrderId = orderId,
        };

        return response;
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