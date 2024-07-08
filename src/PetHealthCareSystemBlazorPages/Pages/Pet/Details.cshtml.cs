using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Entities;
using DataAccessLayer;
using BusinessObject.DTO.Pet;
using Service.IServices;
using Utility.Enum;

namespace PetHealthCareSystemRazorPages.Pages.Pet
{
    public class DetailsModel : PageModel
    {
        private readonly IPetService _petService;

        public DetailsModel(IPetService petService)
        {
            _petService = petService;
        }

        public PetResponseDto Pet { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var role = HttpContext.Session.GetString("Role");

            if (role == null || !role.Contains(UserRole.Customer.ToString()))
            {
                Response.Redirect("/Login");
            }

            if (id == null)
            {
                return NotFound();
            }

            Pet = await _petService.GetPetForCustomerAsync(2002, id.GetValueOrDefault());
            if (Pet == null)
            {
                return NotFound();
            }
            
            return Page();
        }
    }
}
