using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Entities;
using DataAccessLayer;
using BusinessObject.DTO.Pet;
using Service.IServices;
using Utility.Enum;
using BusinessObject.DTO.MedicalRecord;
using Repository.Extensions;

namespace PetHealthCareSystemRazorPages.Pages.Pet
{
    public class DetailsModel : PageModel
    {
        private readonly IPetService _petService;
        private readonly IMedicalService _medicalService;

        public DetailsModel(IPetService petService, IMedicalService medicalService)
        {
            _petService = petService;
            _medicalService = medicalService;
        }

        public PetResponseDto Pet { get; set; } = default!;
        public PaginatedList<MedicalRecordResponseDto> MedicalRecords { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var userId = Int32.Parse(HttpContext.Session.GetString("UserId"));


            var role = HttpContext.Session.GetString("Role");

            if (role == null || !role.Contains(UserRole.Customer.ToString()))
            {
                Response.Redirect("/Login");
            }

            if (id == null)
            {
                Response.Redirect("/Login");
            }

            Pet = await _petService.GetPetForCustomerAsync(userId, id.GetValueOrDefault());
            if (Pet == null)
            {
                Response.Redirect("/Login");
            }

            MedicalRecords = await _medicalService.GetAllMedicalRecordByPetId(id.GetValueOrDefault(), 1, 5);

            return Page();
        }
    }
}
