using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Entities;
using DataAccessLayer;
using Service.IServices;
using BusinessObject.DTO.MedicalRecord;
using Repository.Extensions;

namespace PetHealthCareSystemRazorPages.Pages.Vet.MedicalRecord
{
    public class DetailsModel : PageModel
    {
        private readonly IMedicalService _medical;

        public DetailsModel(IMedicalService medical)
        {
            _medical = medical;
        }

        public MedicalRecordResponseDtoWithDetails MedicalRecord { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        { 
            var accountId = HttpContext.Session.GetString("UserId"); // Assuming UserId is stored in Session
            var accountRole = HttpContext.Session.GetString("Role");
            // Check if accountId is null or empty or if accountRole is not "admin" (assuming "admin" role is stored as such)
            if (string.IsNullOrEmpty(accountId) || !IsVetRole(accountRole))
            {
                return RedirectToPage("/Login");
            }
            if (id == null)
            {
                return RedirectToPage("./Index");
            }
            try
            {
                MedicalRecord = await _medical.GetMedicalRecordById((int)id);
                return Page();
            }
            catch (Exception ex)
            {
                MedicalRecord = new MedicalRecordResponseDtoWithDetails();
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToPage("./Index");
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
