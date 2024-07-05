using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace PetHealthCareSystemRazorPages.Pages.Staff.Appointment
{
    public class BookingManagement : PageModel
    {
        private readonly DataAccessLayer.AppDbContext _context;

        public BookingManagement(DataAccessLayer.AppDbContext context)
        {
            _context = context;
        }

        public IList<BusinessObject.Entities.Appointment> Appointment { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Appointment = await _context.Appointments
                .Include(a => a.TimeTable).ToListAsync();
        }
    }
}
