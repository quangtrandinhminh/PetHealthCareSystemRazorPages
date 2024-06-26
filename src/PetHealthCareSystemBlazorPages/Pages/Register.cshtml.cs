using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.DTO.User;
using Service.IServices;
using Utility.Exceptions;

namespace PetHealthCareSystemRazorPages.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IAuthService _authService;

        public RegisterModel(IAuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public RegisterDto RegisterDto { get; set; }

        public IActionResult OnGet()
        {
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
                await _authService.Register(RegisterDto);
                return RedirectToPage("./Login");
            }
            catch (AppException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}
