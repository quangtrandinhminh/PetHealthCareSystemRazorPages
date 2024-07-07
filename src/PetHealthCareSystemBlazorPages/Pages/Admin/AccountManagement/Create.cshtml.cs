using System.Threading.Tasks;
using BusinessObject.DTO.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.IServices;

namespace PetHealthCareSystemRazorPages.Pages.Admin.AccountManagement
{
    public class CreateModel : PageModel
    {
        private readonly IAuthService _authService;

        public CreateModel(IAuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public RegisterDto RegisterDto { get; set; } = default!;

        [BindProperty]
        public int Role { get; set; }

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
                await _authService.RegisterByAdmin(RegisterDto, Role);
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}
