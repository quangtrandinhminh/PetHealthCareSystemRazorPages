using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.IServices;
using BusinessObject.DTO.Appointment;
using Repository.Extensions;
using System.Threading.Tasks;

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

        public PaginatedList<AppointmentResponseDto> Appointments { get; set; }

        public async Task OnGetAsync(int pageNumber = 1, int pageSize = 10)
        {
            Appointments = await _appointmentService.GetAllAppointmentsAsync(pageNumber, pageSize);
        }
    }
}
