using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.IServices;
using BusinessObject.DTO.Transaction;
using Utility.Exceptions;

namespace PetHealthCareSystemRazorPages.Pages.Staff.BookingManagement
{
    public class DetailsModel : PageModel
    {
        private readonly ITransactionService _transactionService;

        public DetailsModel(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public TransactionResponseWithDetailsDto Transaction { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                Transaction = await _transactionService.GetTransactionByIdAsync(id.Value);
            }
            catch (AppException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return NotFound();
            }

            return Page();
        }
    }
}
