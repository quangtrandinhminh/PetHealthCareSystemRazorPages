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
            if (id == null)
            {
                return NotFound();
            }

            Pet = await _petService.GetPetByIdAsync(id.Value);
            if (Pet == null)
            {
                return NotFound();
            }
            
            return Page();
        }
    }
}
