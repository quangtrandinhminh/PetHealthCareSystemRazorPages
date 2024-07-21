using System;
using System.Threading.Tasks;
using BusinessObject.DTO.MedicalItem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.IServices;

namespace PetHealthCareSystemRazorPages.Pages.Admin.MedicalItemManagement
{
    public class EditModel : PageModel
    {
        private readonly IMedicalService _medicalService;

        public EditModel(IMedicalService medicalService)
        {
            _medicalService = medicalService;
        }

        [BindProperty]
        public MedicalItemUpdateDto MedicalItem { get; set; } = new MedicalItemUpdateDto();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index");
            }

            var medicalItem = await _medicalService.GetAllMedicalItem()
                .ContinueWith(task => task.Result.FirstOrDefault(m => m.Id == id));

            if (medicalItem == null)
            {
                return RedirectToPage("./Index");
            }

            MedicalItem = new MedicalItemUpdateDto
            {
                Id = medicalItem.Id,
                Name = medicalItem.Name,
                Description = medicalItem.Description,
                Price = medicalItem.Price
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userId = Int32.Parse(HttpContext.Session.GetString("UserId") ?? "0");

            try
            {
                await _medicalService.UpdateMedicalItem(MedicalItem, userId);
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}
