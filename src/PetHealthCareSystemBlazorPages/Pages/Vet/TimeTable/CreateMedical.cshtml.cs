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
using BusinessObject.DTO.Pet;
using BusinessObject.DTO.MedicalItem;
using BusinessObject.DTO.Transaction;
using System.Collections;

namespace PetHealthCareSystemRazorPages.Pages.Vet.TimeTable
{
    public class CreateMedicalModel : PageModel
    {
        private readonly IMedicalService _medical;
        private readonly ITransactionService _transactionService;
        private readonly IAppointmentService _appointment;
        private readonly IPetService _pet;
        [BindProperty]
        public AppointmentResponseDto AppointmentItem { get; set; }
        [BindProperty]
        public MedicalRecordRequestDto MedicalRecord { get; set; } = default!;
        public List<MedicalResponseDto> MedicalItems { get; set; }

        public CreateMedicalModel(IMedicalService medical, ITransactionService transactionService, IAppointmentService appointment
            , IPetService pet)
        {
            _medical = medical;
            _transactionService = transactionService;
            _appointment = appointment;
            _pet = pet;
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
                var medicalItems = await _medical.GetAllMedicalItem();
                MedicalItems = medicalItems;
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
        public async Task<IActionResult> OnPostAsync(List<int> SelectedMedicalItemsId, List<int> SelectedMedicalItemsQuantity)
        {
            ModelState.Remove("Vet");
            ModelState.Remove("Pets");
            ModelState.Remove("Customer");
            ModelState.Remove("Status");
            ModelState.Remove("Services");
            ModelState.Remove("Timetable");
            ModelState.Remove("BookingType");
            ModelState.Remove("SelectedMedicalItemsQuantity");
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                var accountId = HttpContext.Session.GetString("UserId");
                int id = int.Parse(accountId);
                List<TransactionMedicalItemsDto> mergedList = new List<TransactionMedicalItemsDto>();
                if (SelectedMedicalItemsId.Count != 0 && SelectedMedicalItemsQuantity.Count != 0 && SelectedMedicalItemsId.Count == SelectedMedicalItemsQuantity.Count)
                {
                    for (int i = 0; i < SelectedMedicalItemsId.Count; i++)
                    {
                        if (SelectedMedicalItemsQuantity[i] == 0)
                        {
                            ModelState.AddModelError("MedicalRecord.MedicalItems", "Medical item's quantity must > 0");
                            return await OnGetAsync(MedicalRecord.AppointmentId);
                        }
                        mergedList.Add(new TransactionMedicalItemsDto()
                        {
                            MedicalItemId = SelectedMedicalItemsId[i],
                            Quantity = SelectedMedicalItemsQuantity[i]
                        });
                    }
                }
                else if (SelectedMedicalItemsId.Count > 0 && SelectedMedicalItemsId.Count != SelectedMedicalItemsQuantity.Count)
                {
                    ModelState.AddModelError("MedicalRecord.MedicalItems","You must input quantity when using medical item");
                    return await OnGetAsync(MedicalRecord.AppointmentId);
                }
                MedicalRecord.MedicalItems = mergedList;
                await _medical.CreateMedicalRecord(MedicalRecord, id);
                List<int>petList = new List<int>();
                var appointment = await _appointment.GetAppointmentByAppointmentId(MedicalRecord.AppointmentId);
                foreach (var item in appointment.Pets)
                {
                    var pet = (await _medical.GetAllMedicalRecord(1, 100)).Items.Where(x=>x.PetId == item.Id && x.AppointmentId == MedicalRecord.AppointmentId).FirstOrDefault();
                    if (pet != null)
                    {
                        petList.Add(pet.PetId);
                    }
                }
                if (petList.Count == appointment.Pets.Count)
                {
                    await _appointment.UpdateStatusToDone(MedicalRecord.AppointmentId,id);
                }

                return RedirectToPage("/Vet/TimeTable/Appointment");
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return await OnGetAsync(MedicalRecord.AppointmentId);
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
