using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.IServices;
using BusinessObject.DTO.MedicalItem;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Utility.Enum;
using Repository.Extensions;

namespace PetHealthCareSystemRazorPages.Pages.Admin.MedicalItemManagement
{
    public class IndexModel : PageModel
    {
        private readonly IMedicalService _medicalService;

        public IndexModel(IMedicalService medicalService)
        {
            _medicalService = medicalService;
        }
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;
        public PaginatedList<MedicalResponseDto> MedicalItems { get; set; } = new PaginatedList<MedicalResponseDto>();

        public async Task OnGetAsync(int? pageNumber)
        {
            var userId = Int32.Parse(HttpContext.Session.GetString("UserId"));
            var role = HttpContext.Session.GetString("Role");

            if (role == null || !role.Contains(UserRole.Admin.ToString()))
            {
                Response.Redirect("/Login");
            }
            try
            {
                MedicalItems = await _medicalService.GetAllMedicalItem(pageNumber ?? 1, PageSize);
            }
            catch (Exception ex)
            {
                MedicalItems = new PaginatedList<MedicalResponseDto>();
                ModelState.AddModelError(string.Empty, ex.Message);
            }
        }
    }
}
