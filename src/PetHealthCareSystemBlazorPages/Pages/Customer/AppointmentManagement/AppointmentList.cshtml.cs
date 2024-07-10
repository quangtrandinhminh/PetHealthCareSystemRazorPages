using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;
        [BindProperty(SupportsGet = true)]
        public string? SearchDate { get; set; }

        public async Task<IActionResult> OnGetAsync(int? pageNumber)
        {
            var userId = Int32.Parse(HttpContext.Session.GetString("UserId"));
            var role = HttpContext.Session.GetString("Role");

            if (role == null || !role.Contains(UserRole.Customer.ToString()))
            {
                return RedirectToPage("/Login");
            }

            // Convert search date string to DateOnly if provided
            var searchDateValue = string.IsNullOrEmpty(SearchDate) ? DateOnly.MinValue : DateOnly.Parse(SearchDate);

            Appointment = await _appointmentService.GetUserAppointmentsAsync(pageNumber ?? 1, PageSize, userId, searchDateValue.ToString());

            return Page();
        }
    }
}
