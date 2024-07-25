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
using PetHealthCareSystemRazorPages.Pages.Vet.MedicalRecord;

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
                // Get current DateTime
                var now = DateTime.Now;

                // Convert to DateOnly
                var today = DateOnly.FromDateTime(now);
                if (check.AppointmentDate != today)
                {
                    Response.Redirect("./Appointment");
                }
                AppointmentItem = check;
                var medicalItems = await _medical.GetAllMedicalItem();
                MedicalItems = medicalItems;
                ViewData["PetId"] = new SelectList(AppointmentItem.Pets, "Id", "Name");
                ViewData["MedicalItemsId"] = new SelectList(medicalItems, "Id", "Name");
                MedicalRecord = new MedicalRecordRequestDto()
                {
                    AppointmentId = AppointmentItem.Id,
                    PetId = AppointmentItem.Pets.First().Id
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
            ModelState.Remove("Transaction");
            ModelState.Remove("SelectedMedicalItemsQuantity");
            if (!ModelState.IsValid)
            {
                return await OnGetAsync(MedicalRecord.AppointmentId);
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
                            ModelState.AddModelError(string.Empty, "Medical item's quantity must > 0");
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
                    ModelState.AddModelError(string.Empty, "You must input quantity when using medical item");
                    return await OnGetAsync(MedicalRecord.AppointmentId);
                }
                MedicalRecord.MedicalItems = mergedList;
                DateTime admission;
                DateTime discharge;
                if (MedicalRecord.AdmissionDate.HasValue)
                {
                    admission = DateTime.ParseExact(MedicalRecord.AdmissionDate.Value.ToString("yyyy-MM-dd"), "yyyy-MM-dd", null);
                    if (admission < DateTime.Today)
                    {
                        ModelState.AddModelError(string.Empty, "Ban khong the nhap vien trong qua khu");
                        return await OnGetAsync(MedicalRecord.AppointmentId);
                    }
                }
                if (MedicalRecord.DischargeDate.HasValue)
                {
                    discharge = DateTime.ParseExact(MedicalRecord.DischargeDate.Value.ToString("yyyy-MM-dd"), "yyyy-MM-dd", null);
                    if (discharge < DateTime.Today)
                    {
                        ModelState.AddModelError(string.Empty, "Ban khong the nhap vien trong qua khu");
                        return await OnGetAsync(MedicalRecord.AppointmentId);
                    }
                }
                if (MedicalRecord.DischargeDate.HasValue && MedicalRecord.AdmissionDate.HasValue)
                {
                    admission = DateTime.ParseExact(MedicalRecord.AdmissionDate.Value.ToString("yyyy-MM-dd"), "yyyy-MM-dd", null);
                    discharge = DateTime.ParseExact(MedicalRecord.DischargeDate.Value.ToString("yyyy-MM-dd"), "yyyy-MM-dd", null);
                    if (discharge >= admission)
                    {
                        ModelState.AddModelError(string.Empty, "Ban khong the ra vien truoc khi nhap vien");
                        return await OnGetAsync(MedicalRecord.AppointmentId);
                    }

                }
        

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
