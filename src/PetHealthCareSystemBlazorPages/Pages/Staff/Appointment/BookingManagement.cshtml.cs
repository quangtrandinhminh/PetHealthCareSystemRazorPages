using BusinessObject.DTO.Appointment;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using Service.IServices;

namespace PetHealthCareSystemRazorPages.Pages.Staff.Appointment
{
    public class BookingManagement : PageModel
    {
        private readonly IAppointmentService _appoint;

        public BookingManagement(IAppointmentService appoint)
        {
            _appoint = appoint;
        }

        public PaginatedList<AppointmentResponseDto> Appointment { get;set; } = default!;

        public async Task OnGetAsync()
        {
            int pageSize = 5;
            int pageNumber = 1;
            Appointment = await _appoint.GetAllAppointmentsAsync(pageNumber,pageSize);
        }
    }
}
