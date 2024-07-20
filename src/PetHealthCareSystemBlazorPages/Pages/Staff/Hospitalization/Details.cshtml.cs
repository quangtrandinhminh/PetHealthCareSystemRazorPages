using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.DTO.Transaction;
using Service.IServices;
using Utility.Exceptions;
using BusinessObject.DTO.Hospitalization;
using Service.Services;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace PetHealthCareSystemRazorPages.Pages.Staff.Hospitalization
{
    public class DetailsModel : PageModel
    {
        private readonly IHospitalizationService _hospi;

        public DetailsModel(IHospitalizationService hospitalization)
        {
            _hospi = hospitalization;
        }

        public HospitalizationResponseDtoWithDetails Hospi { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return Redirect("/");
            }
            var accountId = HttpContext.Session.GetString("UserId"); // Assuming UserId is stored in Session
            var accountRole = HttpContext.Session.GetString("Role");
            // Check if accountId is null or empty or if accountRole is not "admin" (assuming "admin" role is stored as such)
            if (string.IsNullOrEmpty(accountId) || !IsStaffRole(accountRole))
            {
                return Redirect("/");
            }

            try
            {
                Hospi = await _hospi.GetHospitalizationById(id.Value);
                return Page();
            }
            catch (AppException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var accountId = HttpContext.Session.GetString("UserId");
            var userId = int.Parse(accountId);
            var hospi = await _hospi.GetHospitalizationById(id.Value);
            if (hospi != null)
            {
                await _hospi.DeleteHospitalization(hospi.Id, userId);
            }

            return RedirectToPage("./Index");
        }
        private bool IsStaffRole(string accountRole)
        {
            // Example check if "admin" is contained in the roles list
            // Adjust this logic based on how roles are stored in your application
            return !string.IsNullOrEmpty(accountRole) && accountRole.Split(',').Contains("Staff");
        }
    }
}
