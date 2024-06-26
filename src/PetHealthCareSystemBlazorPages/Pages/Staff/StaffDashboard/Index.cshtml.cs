using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PetHealthCareSystemRazorPages.Pages.Staff.StaffDashboard
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            var accountId = HttpContext.Session.GetString("UserId");
            var accountRole = HttpContext.Session.GetString("Role");

            // Check if accountId is null or empty or if accountRole is not "Staff"
            if (string.IsNullOrEmpty(accountId) || !IsStaffRole(accountRole))
            {
                return RedirectToPage("/Login");
            }

            return Page();
        }

        private bool IsStaffRole(string accountRole)
        {
            return !string.IsNullOrEmpty(accountRole) && accountRole.Split(',').Contains("Staff");
        }
    }
}
