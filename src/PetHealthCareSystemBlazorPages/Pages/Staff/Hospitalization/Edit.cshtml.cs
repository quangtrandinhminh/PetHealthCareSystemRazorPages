using BusinessObject.DTO.Appointment;
using BusinessObject.DTO.Hospitalization;
using BusinessObject.DTO.MedicalRecord;
using BusinessObject.DTO.Pet;
using BusinessObject.DTO.Service;
using BusinessObject.DTO.TimeTable;
using BusinessObject.DTO.User;
using BusinessObject.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.IServices;
using Utility.Enum;
using Utility.Exceptions;

namespace PetHealthCareSystemRazorPages.Pages.Staff.Hospitalization
{
    public class EditModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IService _serviceService;
        private readonly IPetService _petService;
        private readonly IHospitalizationService _hospital;
        public readonly IMedicalService _medicalService;



        public EditModel(IAppointmentService appointmentService, IService serviceService, IPetService petService, IHospitalizationService hospital, IMedicalService medicalService)
        {
            _appointmentService = appointmentService;
            _serviceService = serviceService;
            _petService = petService;
            _hospital = hospital;
            _medicalService = medicalService;
        }

        [BindProperty]
        public HospitalizationUpdateRequestDto Hospitalization { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var accountId = HttpContext.Session.GetString("UserId"); // Assuming UserId is stored in Session
            var accountRole = HttpContext.Session.GetString("Role");
            // Check if accountId is null or empty or if accountRole is not "admin" (assuming "admin" role is stored as such)
            if (string.IsNullOrEmpty(accountId) || !IsStaffRole(accountRole))
            {
                return Redirect("/");
            }
            if (id == null)
            {
                return Redirect("/");
            }
            try
            {
                var check = await _hospital.GetHospitalizationById((int)id);
                var cage = await _hospital.GetAvailableCage();
                Hospitalization = new HospitalizationUpdateRequestDto()
                {
                    Id = id.Value,
                    Diagnosis = check.Diagnosis,
                    Note = check.Note,
                    Treatment = check.Treatment,
                    IsDischarged = false,
                    Reason = check.Reason,
                    
                };
                
                    return Page();
               
                
            }
            catch (Exception ex)
            {
                Hospitalization = new HospitalizationUpdateRequestDto();
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Hospitalization.Date");
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                var accountId = HttpContext.Session.GetString("UserId");
                int id = int.Parse(accountId);
                await _hospital.UpdateHospitalization(Hospitalization, id);
                return RedirectToPage("/Staff/Hospitalization/Hospitalization");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return await OnGetAsync(Hospitalization.Id);
            }
        }
        private bool IsStaffRole(string accountRole)
        {
            // Example check if "admin" is contained in the roles list
            // Adjust this logic based on how roles are stored in your application
            return !string.IsNullOrEmpty(accountRole) && accountRole.Split(',').Contains("Staff");
        }
    }
}
