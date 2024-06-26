using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Entities;

namespace PetHealthCareSystemRazorPages.Pages.Staff.BookingManagement
{
    public class IndexModel : PageModel
    {
        private readonly DataAccessLayer.AppDbContext _context;

        public IndexModel(DataAccessLayer.AppDbContext context)
        {
            _context = context;
        }

        public IList<Appointment> Appointment { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Appointment = await _context.Appointments
                .Include(a => a.TimeTable).ToListAsync();
        }
    }
}
