using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.DTO.Transaction;
using Service.IServices;

namespace PetHealthCareSystemRazorPages.Pages.Staff.BookingManagement
{
    public class IndexModel : PageModel
    {
        private readonly ITransactionService _transactionService;

        public IndexModel(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public IList<TransactionResponseDto> Transactions { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }

        public async Task OnGetAsync(int? pageNumber)
        {
            PageNumber = pageNumber ?? 1;

            var paginatedList = await _transactionService.GetAllTransactionsAsync(PageNumber, PageSize);

            Transactions = paginatedList.Items.ToList();
            TotalPages = paginatedList.TotalPages;
        }
    }
}
