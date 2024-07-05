using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace PetHealthCareSystemRazorPages.Pages.Staff.Appointment
{
    public class DetailsModel : PageModel
    {
        private readonly DataAccessLayer.AppDbContext _context;

        public DetailsModel(DataAccessLayer.AppDbContext context)
        {
            _context = context;
        }

        public BusinessObject.Entities.Appointment Appointment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments.FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }
            else
            {
                Appointment = appointment;
            }
            return Page();
        }
    }
}
