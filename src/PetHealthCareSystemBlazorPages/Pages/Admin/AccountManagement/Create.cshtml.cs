using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject.Entities.Identity;
using DataAccessLayer;

namespace PetHealthCareSystemRazorPages.Pages.Admin.AccountManagement
{
    public class CreateModel : PageModel
    {
        private readonly DataAccessLayer.AppDbContext _context;

        public CreateModel(DataAccessLayer.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public UserEntity UserEntity { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Users.Add(UserEntity);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
