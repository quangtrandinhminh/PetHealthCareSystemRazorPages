using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http; // Ensure to include this namespace for HttpContext operations
using Utility.Exceptions;
using Service.IServices;
using BusinessObject.DTO.User;
using System.Threading.Tasks;

namespace PetHealthCareSystemBlazorPages.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IAuthService _authService;
        public LoginModel(IAuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public LoginDto LoginUser { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var response = await _authService.Authenticate(LoginUser);
                if (response == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }

                // Store user session information
                HttpContext.Session.SetString("UserId", response.Id);
                HttpContext.Session.SetString("Username", response.UserName);
                HttpContext.Session.SetString("Role", string.Join(",", response.Role));

                // Redirect based on user role
                // Redirect based on user role
                if (response.Role.Contains("Admin")) // Check if "admin" role exists in the list
                {
                    return RedirectToPage("/Admin/AdminDashboard/Index");
                }
                else if (response.Role.Contains("Staff")) // Check if "staff" role exists in the list
                {
                    return RedirectToPage("/Staff/StaffDashboard/Index");
                }
                /*else if (response.Role.Contains("Vet")) // Check if "customer" role exists in the list
                {
                    return RedirectToPage("/");
                }*/
                else
                {
                    return RedirectToPage("/HomePage");
                }
                return Page();
            }
            catch (AppException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
        public IActionResult OnGet()
        {
            var accountId = HttpContext.Session.GetString("UserId"); // Assuming UserId is stored in Session
            if (!string.IsNullOrEmpty(accountId))
            {
                var accountRole = HttpContext.Session.GetString("Role");
                if (accountRole != null && accountRole.Split(',').Contains("Admin"))
                {
                    return RedirectToPage("/Admin/AdminDashboard/Index");
                }
                else if (accountRole != null && accountRole.Split(',').Contains("Staff"))
                {
                    return RedirectToPage("/Staff/StaffDashboard/Index");
                }
            }
            return Page();
        }
    }
}
