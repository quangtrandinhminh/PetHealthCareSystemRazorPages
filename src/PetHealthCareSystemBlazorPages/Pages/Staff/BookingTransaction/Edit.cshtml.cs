using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject.Entities;
using DataAccessLayer;
using Service.IServices;
using Utility.Exceptions;

namespace PetHealthCareSystemRazorPages.Pages.Staff.BookingTransaction
{
    public class EditModel : PageModel
    {
        private readonly ITransactionService _transactionService;

        public EditModel(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [BindProperty]
        public Transaction Transaction { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _transactionService.GetTransactionByIdAsync(id.Value);

            if (transaction == null)
            {
                return NotFound();
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
                await _transactionService.UpdatePaymentByStaffAsync(Transaction.Id,userId);
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
