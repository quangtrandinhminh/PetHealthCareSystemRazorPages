using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

namespace PetHealthCareSystemRazorPages.Pages.Staff.StaffDashboard
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            var accountId = HttpContext.Session.GetString("UserId"); // Assuming UserId is stored in Session
            var accountRole = HttpContext.Session.GetString("Role");

            // Check if accountId is null or empty or if accountRole is not "staff" (assuming "staff" role is stored as such)
            if (string.IsNullOrEmpty(accountId) || !IsStaffRole(accountRole))
            {
                return RedirectToPage("/Login");
            }

            return Page();
        }

        private bool IsStaffRole(string accountRole)
        {
            // Example check if "staff" is contained in the roles list
            // Adjust this logic based on how roles are stored in your application
            return !string.IsNullOrEmpty(accountRole) && accountRole.Split(',').Contains("staff");
        }
    }
}
