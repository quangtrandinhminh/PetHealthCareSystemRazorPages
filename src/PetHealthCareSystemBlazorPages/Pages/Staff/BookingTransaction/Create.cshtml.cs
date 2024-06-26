using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.IServices;
using System.ComponentModel.DataAnnotations;
using BusinessObject.DTO.Transaction;
using Utility.Constants;
using Microsoft.AspNetCore.Http;
using Utility.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace PetHealthCareSystemRazorPages.Pages.Staff.BookingTransaction
{
    [Authorize(Roles = "Staff")]
    public class CreateModel : PageModel
    {
        private readonly ITransactionService _transactionService;

        public CreateModel(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [BindProperty]
        public TransactionRequestDto TransactionDto { get; set; }

        public IActionResult OnGet()
        {
            TransactionDto = new TransactionRequestDto
            {
                MedicalItems = new List<TransactionMedicalItemsDto>(),
                Services = new List<TransactionServicesDto>()
            };

            // Populate MedicalItems and Services from the database or other data source
            // Example:
            // TransactionDto.MedicalItems = _transactionService.GetMedicalItems();
            // TransactionDto.Services = _transactionService.GetServices();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Replace with your actual user ID retrieval logic
                var accountId = HttpContext.Session.GetString("UserId");
                var accountRole = HttpContext.Session.GetString("Role");
                int userId = int.Parse(accountId);

                await _transactionService.CreateTransactionAsync(TransactionDto, userId);

                // Redirect to a success page or another appropriate action
                return RedirectToPage("/Index");
            }
            catch (AppException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

        private bool IsStaffRole(string accountRole)
        {
            return !string.IsNullOrEmpty(accountRole) && accountRole.Split(',').Contains("Staff");
        }
    }
}
