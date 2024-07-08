using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject.Entities;
using DataAccessLayer;
using BusinessObject.DTO.MedicalRecord;
using Service.IServices;
using BusinessObject.DTO.Appointment;
using Azure;
using Microsoft.Identity.Client;
using Repository.Extensions;
using Service.Services;

namespace PetHealthCareSystemRazorPages.Pages.Vet.TimeTable
{
    public class CreateMedicalModel : PageModel
    {
        private readonly DataAccessLayer.AppDbContext _context;
        private readonly IMedicalService _medical;
        private readonly ITransactionService _transactionService;
        private readonly IAppointmentService _appointment;
        [BindProperty]
        public AppointmentResponseDto AppointmentItem { get; set; }
        public CreateMedicalModel(DataAccessLayer.AppDbContext context, IMedicalService medical, ITransactionService transactionService, IAppointmentService appointment)
        {
            _context = context;
            _medical = medical;
            _transactionService = transactionService;
            _appointment = appointment;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            
            try
            {
                var check = await _appointment.GetAppointmentByAppointmentId(id);
                AppointmentItem = check;
            }
            catch (Exception ex)
            {
                AppointmentItem = new AppointmentResponseDto();
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }

            return Page();
        }

        [BindProperty]
        public MedicalRecordRequestDto MedicalRecord { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Vet");
            ModelState.Remove("Pets");
            ModelState.Remove("Status");
            ModelState.Remove("Services");
            ModelState.Remove("Timetable");
            ModelState.Remove("BookingType");
            if (!ModelState.IsValid && AppointmentItem != null)
            {
                return Page();
            }
            foreach (var pet in AppointmentItem.Pets)
            {
                MedicalRecord.PetId = pet.Id;
                await _medical.CreateMedicalRecord(MedicalRecord, AppointmentItem.Vet.Id);
            }

            return RedirectToPage("./Index");
        }
    }
}
