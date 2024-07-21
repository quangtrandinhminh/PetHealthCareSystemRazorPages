using BusinessObject.DTO;
using BusinessObject.DTO.Appointment;
using BusinessObject.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.IServices;
using Utility.Enum;
using Utility.Exceptions;

namespace PetHealthCareSystemRazorPages.Pages.Customer.AppointmentManagement
{
    public class FeedBackModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;

        public FeedBackModel(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [BindProperty]
        public int AppointmentId { get; set; }

        public AppointmentResponseDto AppointmentResponse { get; set; }

        [BindProperty]
        public AppointmentFeedbackRequestDto FeedbackRequest { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var role = HttpContext.Session.GetString("Role");

            if (role == null || !role.Contains(UserRole.Customer.ToString()))
            {
                return RedirectToPage("/Login");
            }

            if (id == null)
            {
                return NotFound();
            }

            AppointmentId = id.GetValueOrDefault();

            try
            {
                AppointmentResponse = await _appointmentService.GetAppointmentByAppointmentId(AppointmentId);

                if (AppointmentResponse.Status != AppointmentStatus.Completed.ToString())
                {
                    return RedirectToPage("/Customer/AppointmentManagement/AppointmentList");
                }
            }
            catch (AppException ex)
            {
                return RedirectToPage("/Customer/AppointmentManagement/AppointmentList");
            }

            FeedbackRequest = new AppointmentFeedbackRequestDto { AppointmentId = AppointmentId };
            return Page();
        }

        public async Task<IActionResult> OnPostFeedbackAppointmentAsync()
        {
            var userId = int.Parse(HttpContext.Session.GetString("UserId"));

            FeedbackRequest.AppointmentId = AppointmentId;

            await _appointmentService.FeedbackAppointmentAsync(FeedbackRequest, userId);

            return RedirectToPage("/Customer/AppointmentManagement/AppointmentList");
        }
    }
}
