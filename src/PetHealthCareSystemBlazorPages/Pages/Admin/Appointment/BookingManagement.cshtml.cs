using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.IServices;
using BusinessObject.DTO.Appointment;
using Repository.Extensions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Utility.Enum;

namespace PetHealthCareSystemRazorPages.Pages.Admin.Appointment
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

        public PaginatedList<AppointmentResponseDto> Appointments { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;
        public async Task OnGetAsync(int? pageNumber)
        {
            var userId = Int32.Parse(HttpContext.Session.GetString("UserId"));
            var role = HttpContext.Session.GetString("Role");

            if (role == null || !role.Contains(UserRole.Admin.ToString()))
            {
                RedirectToPage("/Login");
            }
            Appointments = await _appointmentService.GetAllAppointmentsAsync(pageNumber ?? 1, PageSize);
        }
    }
}
