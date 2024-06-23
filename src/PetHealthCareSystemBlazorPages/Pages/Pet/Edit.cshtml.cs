using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Entities;
using DataAccessLayer;
using BusinessObject.DTO.Pet;
using Service.IServices;
using System.Collections.Generic;

namespace PetHealthCareSystemRazorPages.Pages.Pet
{
    public class EditModel : PageModel
    {
        private readonly IPetService _petService;

        public EditModel(IPetService petService)
        {
            _petService = petService;
        }

        [BindProperty]
        public PetUpdateRequestDto Pet { get; set; }

        public List<SelectListItem> SpeciesOptions { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Dog", Text = "Dog" },
            new SelectListItem { Value = "Cat", Text = "Cat" },
            new SelectListItem { Value = "Other", Text = "Other" },
        };

        public List<SelectListItem> GenderOptions { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Male", Text = "Male" },
            new SelectListItem { Value = "Female", Text = "Female" },
        };

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var petId = id.GetValueOrDefault();

            // Load Pet details using PetService
            var petResponse = await _petService.GetPetForCustomerAsync(2002, petId);
            if (petResponse == null)
            {
                return NotFound();
            }

            // Map properties from PetResponseDto to PetUpdateRequestDto (Pet)
            Pet = new PetUpdateRequestDto
            {
                Id = petResponse.Id,
                Name = petResponse.Name,
                Species = petResponse.Species,
                Breed = petResponse.Breed,
                DateOfBirth = DateTimeOffset.Parse(petResponse.DateOfBirth.ToString()),
                IsNeutered = petResponse.IsNeutered,
                Gender = petResponse.Gender
                // Add other properties as needed
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Update the existing Pet entity using PetUpdateRequestDto (Pet)
                await _petService.UpdatePetAsync(Pet, 2002);
            }
            catch (Exception)
            {
                throw;
            }

            return RedirectToPage("./Index");
        }
    }
}
