using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.DTO.Transaction;
using Service.IServices;
using Utility.Exceptions;
using BusinessObject.DTO.Hospitalization;
using Service.Services;
using Microsoft.Extensions.Configuration.UserSecrets;
using BusinessObject.DTO.MedicalRecord;

namespace PetHealthCareSystemRazorPages.Pages.Vet.TimeTable
{
    public class DischargeModel : PageModel
    {
        private readonly IHospitalizationService _hospi;
        private readonly IMedicalService _medical;
        public DischargeModel(IHospitalizationService hospitalization, IMedicalService medicalService)
        {
            _hospi = hospitalization;
            _medical = medicalService;
        }
        [BindProperty(SupportsGet = true)]
        public string? Check { get; set; }
        public HospitalizationResponseDtoWithDetails Hospi { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return Redirect("/");
            }
            var accountId = HttpContext.Session.GetString("UserId"); // Assuming UserId is stored in Session
            var accountRole = HttpContext.Session.GetString("Role");
            // Check if accountId is null or empty or if accountRole is not "admin" (assuming "admin" role is stored as such)
            if (string.IsNullOrEmpty(accountId) || !IsVetRole(accountRole))
            {
                return Redirect("/");
            }

            try
            {
                Hospi = await _hospi.GetHospitalizationById(id.Value);
                return Page();
            }
            catch (AppException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var accountId = HttpContext.Session.GetString("UserId");
            var userId = int.Parse(accountId);
            var hospi = await _hospi.GetHospitalizationById(id.Value);
            var hospiList = await _hospi.GetListHospitalizationByMRId(hospi.MedicalRecordId);
            foreach (var  hospitalization in hospiList)
            {
                var update = new HospitalizationUpdateRequestDto
                {
                    Id = hospitalization.Id,
                    Reason = hospitalization.Reason,
                    Diagnosis = hospitalization.Diagnosis,
                    IsDischarged = true,
                    Note = hospitalization.Note,
                    Treatment = hospitalization.Treatment
                };
                await _hospi.UpdateHospitalization(update, userId);
            }
            
            var mr = new MedicalRecordRequestDto
            {
                Id = hospi.MedicalRecordId,
                DischargeDate = DateTime.Now,
            };
            await _medical.UpdateMedicalRecord(mr, userId);
            return RedirectToPage("./Hospitalization");
        }
        private bool IsVetRole(string accountRole)
        {
            // Example check if "admin" is contained in the roles list
            // Adjust this logic based on how roles are stored in your application
            return !string.IsNullOrEmpty(accountRole) && accountRole.Split(',').Contains("Vet");
        }
    }
}
