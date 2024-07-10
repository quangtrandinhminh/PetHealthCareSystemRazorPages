﻿using System;
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
using BusinessObject.DTO.Pet;

namespace PetHealthCareSystemRazorPages.Pages.Vet.TimeTable
{
    public class CreateMedicalModel : PageModel
    {
        private readonly DataAccessLayer.AppDbContext _context;
        private readonly IMedicalService _medical;
        private readonly ITransactionService _transactionService;
        private readonly IAppointmentService _appointment;
        private readonly IMedicalItemService _medicalItem;
        [BindProperty]
        public AppointmentResponseDto AppointmentItem { get; set; }
        [BindProperty]
        public MedicalRecordRequestDto MedicalRecord { get; set; } = default!;
        public CreateMedicalModel(DataAccessLayer.AppDbContext context, IMedicalService medical, ITransactionService transactionService, IAppointmentService appointment
            , IMedicalItemService medicalItem)
        {
            _context = context;
            _medical = medical;
            _transactionService = transactionService;
            _appointment = appointment;
            _medicalItem = medicalItem;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var accountId = HttpContext.Session.GetString("UserId"); // Assuming UserId is stored in Session
            var accountRole = HttpContext.Session.GetString("Role");
            // Check if accountId is null or empty or if accountRole is not "admin" (assuming "admin" role is stored as such)
            if (string.IsNullOrEmpty(accountId) || !IsVetRole(accountRole))
            {
                Response.Redirect("/Login");
            }
            try
            {
                var check = await _appointment.GetAppointmentByAppointmentId(id);
                AppointmentItem = check;
                var medicalItems = await _medicalItem.GetAllMedicalItem();
                ViewData["PetId"] = new SelectList(AppointmentItem.Pets, "Id", "Name");
                ViewData["MedicalItemsId"] = new SelectList(medicalItems, "Id", "Name");
                MedicalRecord = new MedicalRecordRequestDto()
                {
                    AppointmentId = AppointmentItem.Id,
                    
                };
            }
            catch (Exception ex)
            {
                AppointmentItem = new AppointmentResponseDto();
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }

            return Page();
        }

        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Vet");
            ModelState.Remove("Pets");
            ModelState.Remove("Customer");
            ModelState.Remove("Status");
            ModelState.Remove("Services");
            ModelState.Remove("Timetable");
            ModelState.Remove("BookingType");
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                var accountId = HttpContext.Session.GetString("UserId");
                int id = int.Parse(accountId);

                await _medical.CreateMedicalRecord(MedicalRecord, id);


                return RedirectToPage("/Vet/TimeTable/Appointment");
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return Page();
            }
            
        }

        private bool IsVetRole(string accountRole)
        {
            // Example check if "admin" is contained in the roles list
            // Adjust this logic based on how roles are stored in your application
            return !string.IsNullOrEmpty(accountRole) && accountRole.Split(',').Contains("Vet");
        }
    }
}
