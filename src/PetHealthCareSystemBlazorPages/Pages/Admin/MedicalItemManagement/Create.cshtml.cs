using BusinessObject.DTO.MedicalItem;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.IServices;

namespace PetHealthCareSystemRazorPages.Pages.Admin.MedicalItemManagement
{
    public class CreateModel : PageModel
    {
        private readonly IMedicalService _medicalService;

        public CreateModel(IMedicalService medicalService)
        {
            _medicalService = medicalService;
        }

        [BindProperty]
        public MedicalItemRequestDto MedicalItem { get; set; } = new MedicalItemRequestDto();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Replace this with actual logic to retrieve userId from session or context
            var userId = Int32.Parse(HttpContext.Session.GetString("UserId") ?? "0");

            try
            {
                await _medicalService.CreateMedicalItem(MedicalItem, userId);
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
