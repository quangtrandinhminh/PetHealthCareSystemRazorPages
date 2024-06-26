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
    public class IndexModel : PageModel
    {
        private readonly DataAccessLayer.AppDbContext _context;

        public IndexModel(DataAccessLayer.AppDbContext context)
        {
            _context = context;
        }

        public IList<UserEntity> UserEntity { get;set; } = default!;

        public async Task OnGetAsync()
        {
            UserEntity = await _context.Users.ToListAsync();
        }
    }
}
