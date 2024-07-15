using BusinessObject.DTO.Transaction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.IServices;
using Utility.Exceptions;

namespace PetHealthCareSystemRazorPages.Pages.Staff.Appointment
{
    public class EditModel : PageModel
    {
        private readonly ITransactionService _transactionService;

        public EditModel(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [BindProperty]
        public TransactionResponseDto Transaction { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                Transaction = await _transactionService.GetTransactionByAppointmentIdAsync(id.Value);
            }
            catch (AppException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }

            string userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToPage("/Login");
            }

            int userId = int.Parse(userIdString);
            ViewData["UserId"] = new SelectList(new List<int> { userId }, "Id", "Id");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToPage("/Login");
            }

            int userId = int.Parse(userIdString);

            try
            {
                await _transactionService.UpdatePaymentByStaffAsync(Transaction.Id, userId);
            }
            catch (AppException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
