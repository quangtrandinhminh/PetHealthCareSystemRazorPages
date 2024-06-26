using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.DTO.Transaction;
using Service.IServices;
using Repository.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace PetHealthCareSystemRazorPages.Pages.Staff.BookingTransaction
{
    [Authorize(Roles = "Staff")]
    public class DetailsModel : PageModel
    {
        private readonly ITransactionService _transactionService;

        public DetailsModel(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public PaginatedList<TransactionResponseDto> Transactions { get; set; }
        public int CustomerId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? customerId, int pageNumber = 1, int pageSize = 10)
        {
            if (customerId == null)
            {
                return NotFound();
            }

            CustomerId = customerId.Value;
            Transactions = await _transactionService.GetTransactionsByCustomerIdAsync(CustomerId, pageNumber, pageSize);

            if (Transactions == null || Transactions.Items.Count == 0)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
