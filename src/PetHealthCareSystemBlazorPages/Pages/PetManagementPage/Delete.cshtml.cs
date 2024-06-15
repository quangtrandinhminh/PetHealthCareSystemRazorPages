using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Entities;
using DataAccessLayer;

namespace PetHealthCareSystemRazorPages.Pages.PetManagementPage
{
    public class DeleteModel : PageModel
    {
        private readonly DataAccessLayer.AppDbContext _context;

        public DeleteModel(DataAccessLayer.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Pet Pet { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets.FirstOrDefaultAsync(m => m.Id == id);

            if (pet == null)
            {
                return NotFound();
            }
            else
            {
                Pet = pet;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets.FindAsync(id);
            if (pet != null)
            {
                Pet = pet;
                _context.Pets.Remove(Pet);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
