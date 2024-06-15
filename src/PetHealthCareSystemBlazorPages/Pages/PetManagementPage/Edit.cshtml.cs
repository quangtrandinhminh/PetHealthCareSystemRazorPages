using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.DTO.Pet;
using Service.IServices;

namespace PetHealthCareSystemRazorPages.Pages.PetManagementPage
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

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Load Pet details using PetService
            var petResponse = await _petService.GetPetByID(id.Value);
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
                DateOfBirth = DateTime.Parse(petResponse.DateOfBirth), // Example assuming DateOfBirth is string
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
                //await _petService.UpdatePetAsync(Pet);
            }
            catch (Exception)
            {
                throw;
            }

            return RedirectToPage("./Index");
        }
    }
}
