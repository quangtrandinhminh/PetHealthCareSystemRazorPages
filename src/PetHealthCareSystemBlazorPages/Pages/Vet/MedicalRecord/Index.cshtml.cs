using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Entities;
using DataAccessLayer;
using BusinessObject.DTO.MedicalRecord;
using Repository.Extensions;
using Service.IServices;
using BusinessObject.DTO.Appointment;
using Service.Services;

namespace PetHealthCareSystemRazorPages.Pages.Vet.MedicalRecord
{
    public class IndexModel : PageModel
    {
        private readonly IMedicalService _medical;

        public IndexModel(IMedicalService medical)
        {
            _medical = medical;
        }

        public PaginatedList<MedicalRecordResponseDto> MedicalRecord { get; set; } = default!;

        public async Task OnGetAsync(int? currentPage)
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
                int pagenumber;
                int pagesize = 3;
                int id = int.Parse(accountId);
                var date = DateTime.Now.ToString("yyyy-MM-dd");
                if (currentPage == null)
                {
                    pagenumber = 1;
                }
                else
                {
                    pagenumber = (int)currentPage;
                }
                var medical = await _medical.GetAllMedicalRecord(pagenumber,pagesize);
                MedicalRecord = medical;
            }
            catch (Exception ex)
            {
                MedicalRecord = new PaginatedList<MedicalRecordResponseDto>();
                ModelState.AddModelError(string.Empty, ex.Message);
                Page();
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
