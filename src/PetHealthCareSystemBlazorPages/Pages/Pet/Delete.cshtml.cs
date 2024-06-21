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
    public class DeleteModel : PageModel
    {
        private readonly IPetService _petService;

        public DeleteModel(IPetService petService)
        {
            _petService = petService;
        }

        [BindProperty]
        public PetResponseDto Pet { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Pet = await _petService.GetPetByID(id.Value);

            if (Pet == null)
            {
                return NotFound();
            }
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _petService.GetPetByID(id.Value);
            if (pet != null)
            {
                await _petService.DeletePetAsync(pet.Id, 2002);
            }

            return RedirectToPage("./Index");
        }
    }
}
