using BusinessObject.DTO.Appointment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.IServices;
using Utility.Enum;

namespace PetHealthCareSystemRazorPages.Pages.Customer.AppointmentManagement
{
    public class CancelModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;

        public CancelModel(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [BindProperty]
        public int AppointmentId { get; set; }

        public AppointmentResponseDto AppointmentResponse { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var role = HttpContext.Session.GetString("Role");

            if (role == null || !role.Contains(UserRole.Customer.ToString()))
            {
                Response.Redirect("/Login");
            }

            if (id == null)
            {
                return NotFound();
            }

            AppointmentId = id.Value;
            AppointmentResponse = await _appointmentService.GetAppointmentByAppointmentId(AppointmentId);
            if (AppointmentResponse == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostCancelAppointmentAsync()
        {
            var userId = Int32.Parse(HttpContext.Session.GetString("UserId"));

            await _appointmentService.UpdateStatusToCancel(AppointmentId, userId);

            return RedirectToPage("/Customer/AppointmentManagement/AppointmentList");
        }
    }
}
