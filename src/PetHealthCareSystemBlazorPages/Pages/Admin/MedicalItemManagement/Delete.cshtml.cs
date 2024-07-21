using System;
using System.Threading.Tasks;
using BusinessObject.DTO.MedicalItem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.IServices;

namespace PetHealthCareSystemRazorPages.Pages.Admin.MedicalItemManagement
{
    public class DeleteModel : PageModel
    {
        private readonly IMedicalService _medicalService;

        public DeleteModel(IMedicalService medicalService)
        {
            _medicalService = medicalService;
        }

        [BindProperty]
        public MedicalResponseDto MedicalItem { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index");
            }

            var medicalitem = await _medicalService.GetAllMedicalItem()
                .ContinueWith(task => task.Result.FirstOrDefault(m => m.Id == id));

            if (medicalitem == null)
            {
                return RedirectToPage("./Index");
            }
            MedicalItem = medicalitem;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index");
            }

            var userId = Int32.Parse(HttpContext.Session.GetString("UserId") ?? "0");

            try
            {
                await _medicalService.DeleteMedicalItem(id.Value, userId);
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
