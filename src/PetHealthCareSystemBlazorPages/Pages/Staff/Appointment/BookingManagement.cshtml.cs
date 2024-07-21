using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.IServices;
using BusinessObject.DTO.Appointment;
using Repository.Extensions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Utility.Enum;

namespace PetHealthCareSystemRazorPages.Pages.Staff.Appointment
{
    public class BookingManagement : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IUserService _userService;

        public BookingManagement(IAppointmentService appointmentService, IUserService userService)
        {
            _appointmentService = appointmentService;
            _userService = userService;
        }

        public PaginatedList<AppointmentResponseDto> Appointment { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;
        [BindProperty(SupportsGet = true)]
        public string? SearchDate { get; set; }

        public async Task<IActionResult> OnGetAsync(int? pageNumber)
        {
            var role = HttpContext.Session.GetString("Role");

            if (role == null || !role.Contains(UserRole.Staff.ToString()))
            {
                return RedirectToPage("/Login");
            }

            // Convert search date string to DateOnly if provided

            Appointment = await _appointmentService.GetAllAppointmentsAsync(pageNumber ?? 1, PageSize);

            return Page();
        }
    }
}
