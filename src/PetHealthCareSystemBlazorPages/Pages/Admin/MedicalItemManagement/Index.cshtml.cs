using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.IServices;
using BusinessObject.DTO.MedicalItem;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PetHealthCareSystemRazorPages.Pages.Admin.MedicalItemManagement
{
    public class IndexModel : PageModel
    {
        private readonly IMedicalService _medicalService;

        public IndexModel(IMedicalService medicalService)
        {
            _medicalService = medicalService;
        }

        public List<MedicalResponseDto> MedicalItems { get; set; } = new List<MedicalResponseDto>();

        public async Task OnGetAsync()
        {
            try
            {
                MedicalItems = await _medicalService.GetAllMedicalItem();
            }
            catch (Exception ex)
            {
                MedicalItems = new List<MedicalResponseDto>();
                ModelState.AddModelError(string.Empty, ex.Message);
            }
        }
    }
}
