using BusinessObject.DTO.Appointment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.IServices;
using System.Threading.Tasks;
using Utility.Constants;
using Utility.Exceptions;

namespace PetHealthCareSystemRazorPages.Pages.Staff.Appointment
{
    public class DetailsModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;

        public DetailsModel(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public AppointmentResponseDto Appointment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                Appointment = await _appointmentService.GetAppointmentByAppointmentId(id.Value);
                if(Appointment == null)
                {
                    return NotFound();
                }

                return Page();
            }
            catch (AppException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }

            return Page();
        }
    }
}
