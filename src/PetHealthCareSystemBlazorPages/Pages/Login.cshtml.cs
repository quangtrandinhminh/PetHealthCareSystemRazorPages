using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Utility.Exceptions;
using Service.IServices;
using BusinessObject.DTO.User;
using System.Threading.Tasks;
using System.Linq;

namespace PetHealthCareSystemBlazorPages.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IAuthService _authService;
        private const string ADMIN_PAGE = "/Admin/AdminDashboard/Index";
        private const string STAFF_PAGE = "/Staff/StaffDashboard";
        private const string VET_PAGE = "/Vet/VetDashBoard/Index";
        private const string HOME_PAGE = "/HomePage";

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

                // Store user session information
                HttpContext.Session.SetString("UserId", response.Id.ToString());
                if (response.UserName != null) HttpContext.Session.SetString("Username", response.UserName);
                HttpContext.Session.SetString("Role", string.Join(",", response.Role));

                // Redirect based on user role
                if (response.Role.Contains("Admin"))
                {
                    return RedirectToPage(ADMIN_PAGE);
                }
                else if (response.Role.Contains("Staff"))
                {
                    return RedirectToPage(STAFF_PAGE);
                }
                else if (response.Role.Contains("Vet")) // Check if "customer" role exists in the list
                {
                    return RedirectToPage(VET_PAGE);
                }
                else
                {
                    return RedirectToPage(HOME_PAGE);
                }
                return Page();
            }
            catch (AppException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., using a logging framework)
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                return Page();
            }
        }

        public IActionResult OnGet()
        {
            var accountId = HttpContext.Session.GetString("UserId");
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
