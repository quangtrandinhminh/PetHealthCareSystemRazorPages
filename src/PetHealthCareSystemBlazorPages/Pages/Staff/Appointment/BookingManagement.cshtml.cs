using BusinessObject.DTO.Appointment;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.IServices;
using BusinessObject.DTO.Appointment;
using Repository.Extensions;
using System.Threading.Tasks;

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

        public async Task OnGetAsync(int pageNumber = 1, int pageSize = 10)
        {
            Appointment = await _context.Appointments
                .Include(a => a.TimeTable).ToListAsync();
        }
    }
}
