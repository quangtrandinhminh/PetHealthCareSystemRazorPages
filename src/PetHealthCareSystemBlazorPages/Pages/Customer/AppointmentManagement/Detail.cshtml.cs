using BusinessObject.DTO.Appointment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.IServices;
using Service.Services;
using Utility.Enum;

namespace PetHealthCareSystemRazorPages.Pages.Customer.AppointmentManagement
{
    public class DetailModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;

        public DetailModel(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public AppointmentResponseDto AppointmentResponseDto { get; set; }

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

            AppointmentResponseDto = await _appointmentService.GetAppointmentByAppointmentId(id.GetValueOrDefault());
            if (AppointmentResponseDto == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
