using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.DTO.Appointment;
using Service.IServices;
using Utility.Enum;
using Repository.Extensions;

namespace PetHealthCareSystemRazorPages.Pages.Customer.AppointmentManagement
{
    public class AppointmentListModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentListModel(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public PaginatedList<AppointmentResponseDto> Appointment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? pageNumber)
        {
            var userId = Int32.Parse(HttpContext.Session.GetString("UserId"));


            var role = HttpContext.Session.GetString("Role");

            if (role == null || !role.Contains(UserRole.Customer.ToString()))
            {
                Response.Redirect("/Login");
            }
                
            int pageSize = 5;
            Appointment = await _appointmentService.GetUserAppointmentsAsync(pageNumber ?? 1, pageSize, userId, DateOnly.MinValue.ToString());

            return Page();
        }
    }
}
