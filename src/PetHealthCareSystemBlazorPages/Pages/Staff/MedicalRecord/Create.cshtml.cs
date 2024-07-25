using BusinessObject.DTO.Appointment;
using BusinessObject.DTO.Cage;
using BusinessObject.DTO.Hospitalization;
using BusinessObject.DTO.MedicalRecord;
using BusinessObject.DTO.Pet;
using BusinessObject.DTO.Service;
using BusinessObject.DTO.TimeTable;
using BusinessObject.DTO.User;
using BusinessObject.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using Service.IServices;
using Utility.Enum;
using Utility.Exceptions;

namespace PetHealthCareSystemRazorPages.Pages.Staff.MedicalRecord
{
    public class CreateModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IService _serviceService;
        private readonly IPetService _petService;
        private readonly IHospitalizationService _hospital;
        public readonly IMedicalService _medicalService;


        public CreateModel(IAppointmentService appointmentService, IService serviceService, IPetService petService, IHospitalizationService hospital, IMedicalService medicalService)
        {
            _appointmentService = appointmentService;
            _serviceService = serviceService;
            _petService = petService;
            _hospital = hospital;
            _medicalService = medicalService;
        }

        [BindProperty]
        public HospitalizationRequestDto Hospitalization { get; set; }
        public List<CageResponseDto> DisplayedCageList { get; set; }
        public List<TimeTableResponseDto> DisplayedTimeTableList { get; set; }
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
                var check = await _medicalService.GetMedicalRecordById((int)id);
                
                DisplayedCageList = await _hospital.GetAvailableCage();
                ViewData["TimetableId"] = new SelectList(await _hospital.GetAllTimeFramesForHospitalizationAsync(), "Id", "Id");
                DisplayedTimeTableList = await _hospital.GetAllTimeFramesForHospitalizationAsync();
                //ViewData["CageId"] = new SelectList(cage, "Id", "Id");
                try{
                    var test = await _hospital.GetCurrentCageByMedicalRecordId(check.Id);
                    if (test != null)
                    {
                        DisplayedCageList = new List<CageResponseDto> { test };
                    }
                }
                catch (Exception e)
                {

                }

                Hospitalization = new HospitalizationRequestDto()
                {
                    Date = check.Date.ToString("yyyy-MM-dd"),
                    Diagnosis = check.Diagnosis,
                    Note = check.Note,
                    MedicalRecordId = (int)id,
                    Treatment = check.Treatment,
                    VetId = check.VetId,
                    IsDischarged = false,
                };
                
                    return Page();
               
                
            }
            catch (Exception ex)
            {
                Hospitalization = new HospitalizationRequestDto();
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Hospitalization.Date");
            if (!ModelState.IsValid)
            {
                return await OnGetAsync(Hospitalization.MedicalRecordId);
            }
            try
            {
                var accountId = HttpContext.Session.GetString("UserId");
                int id = int.Parse(accountId);

                if (Hospitalization.Date.IsNullOrEmpty())
                {
                    ModelState.AddModelError(string.Empty, "Ban phai chon ngay");
                    return await OnGetAsync(Hospitalization.MedicalRecordId);
                }
                else
                {
                    DateTime dateTime = DateTime.ParseExact(Hospitalization.Date, "yyyy-MM-dd", null);
                    if (dateTime < DateTime.Today) 
                    {
                        ModelState.AddModelError(string.Empty, "Ban khong the nhap vien trong qua khu");
                        return await OnGetAsync(Hospitalization.MedicalRecordId);
                    }
                }
                var check = new DateTimeQueryDto
                {
                    Date = Hospitalization.Date,
                    TimetableId = Hospitalization.TimeTableId,
                };
                
                var test = await _hospital.GetFreeWithTimeFrameAndDateAsync(check);
                if (test.FirstOrDefault(x =>x.Id == Hospitalization.VetId) == null)
                {
                    ModelState.AddModelError(string.Empty, "Bac si da co lich trong thoi gian nay");
                    return await OnGetAsync(Hospitalization.MedicalRecordId);
                }
                
                await _hospital.CreateHospitalization(Hospitalization, id);
                return RedirectToPage("/Staff/Hospitalization/Hospitalization");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return await OnGetAsync(Hospitalization.MedicalRecordId);
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
