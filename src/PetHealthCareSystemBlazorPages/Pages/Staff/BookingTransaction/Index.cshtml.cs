using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.DTO.Transaction;
using Service.IServices;
using Utility.Helpers;
using Repository.Extensions;

namespace PetHealthCareSystemRazorPages.Pages.Staff.BookingTransaction
{
    public class IndexModel : PageModel
    {
        private readonly ITransactionService _transactionService;

        public IndexModel(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public PaginatedList<TransactionResponseDto> Transactions { get; set; }

        public async Task OnGetAsync(int pageNumber = 1, int pageSize = 5)
        {
            Transactions = await _transactionService.GetAllTransactionsAsync(pageNumber, pageSize);
        }
    }
}
