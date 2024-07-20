using System.Threading.Tasks;
using BusinessObject.DTO.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.IServices;
using Service.Services;

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

        public IList<RoleResponseDto> Roles { get; set; } = new List<RoleResponseDto>();
        [BindProperty]
        public int Role { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Roles = await _authService.GetAllRoles();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Roles = await _authService.GetAllRoles();
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
