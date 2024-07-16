using BusinessObject.DTO.Transaction;
using Repository.Extensions;

namespace Service.IServices;

public interface ITransactionService
{
    TransactionDropdownDto GetTransactionDropdownData();
    Task<PaginatedList<TransactionResponseDto>> GetAllTransactionsAsync(int pageNumber, int pageSize);
    Task<PaginatedList<TransactionResponseDto>> GetTransactionsByCustomerIdAsync(int customerId, int pageNumber,
        int pageSize);
    Task<TransactionResponseDto> GetTransactionByAppointmentIdAsync(int appointmentId);
    Task<TransactionResponseDto> GetTransactionByMedicalRecordIdAsync(int medicalRecordId);
    Task<TransactionResponseWithDetailsDto> GetTransactionByIdAsync(int transactionId);
    Task<PaginatedList<TransactionResponseDto>> GetTransactionsByFilterAsync(TransactionFilterDto filter,
        int pageNumber, int pageSize);
    Task CreateTransactionAsync(TransactionRequestDto dto, int userId);
    Task CreateTransactionForHospitalization(TransactionRequestDto dto, int staffId);
    Task UpdatePaymentByStaffAsync(int transactionId, int updatedById);
    Task UpdateTransactionToRefundAsync(TransactionRefundRequestDto dto, int updatedById);
    Task<RefundConditionsResponseDto> GetRefundConditionsAsync();
    Task<TransactionPayOsResponseDto> CreatePayOsTransaction();
    Task<HospitalizationPriceResponseDto> CalculateHospitalizationPriceAsync(
        int medicalRecordId);
}