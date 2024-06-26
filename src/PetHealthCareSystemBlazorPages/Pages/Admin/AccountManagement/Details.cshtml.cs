using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Entities.Identity;
using DataAccessLayer;

namespace PetHealthCareSystemRazorPages.Pages.Admin.AccountManagement
{
    public class DetailsModel : PageModel
    {
        private readonly DataAccessLayer.AppDbContext _context;

        public DetailsModel(DataAccessLayer.AppDbContext context)
        {
            _context = context;
        }

        public UserEntity UserEntity { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userentity = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (userentity == null)
            {
                return NotFound();
            }
            else
            {
                UserEntity = userentity;
            }
            return Page();
        }
    }
}
