using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.DTO.Transaction;
using Service.IServices;
using Utility.Exceptions;

namespace PetHealthCareSystemRazorPages.Pages.Staff.BookingTransaction
{
    public class DetailsModel : PageModel
    {
        private readonly ITransactionService _transactionService;

        public DetailsModel(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public TransactionResponseWithDetailsDto Transaction { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                Transaction = await _transactionService.GetTransactionByIdAsync(id.Value);
                if (Transaction == null)
                {
                    return NotFound();
                }

                return Page();
            }
            catch (AppException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}
