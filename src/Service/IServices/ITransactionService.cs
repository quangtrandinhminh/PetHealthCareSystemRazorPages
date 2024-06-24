using BusinessObject.DTO.Transaction;
using Repository.Extensions;

namespace Service.IServices;

public interface ITransactionService
{
    TransactionDropdownDto GetTransactionDropdownData();
    Task<PaginatedList<TransactionResponseDto>> GetAllTransactionsAsync(int pageNumber, int pageSize);
    Task<PaginatedList<TransactionResponseDto>> GetTransactionsByCustomerIdAsync(int customerId, int pageNumber,
        int pageSize);
    Task<TransactionResponseWithDetailsDto> GetTransactionByIdAsync(int id);
    Task CreateTransactionAsync(TransactionRequestDto dto, int userId);
    Task UpdatePaymentByStaffAsync(int id, int userId);
}