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
using BusinessObject.DTO.Appointment;
using System.Configuration;
using Repository.Extensions;
using BusinessObject.DTO.User;
using Azure;
using Microsoft.Identity.Client;
using BusinessObject.DTO.Hospitalization;

namespace PetHealthCareSystemRazorPages.Pages.Vet.TimeTable
{
    public class HospitalizationModel : PageModel
    {
        private readonly IHospitalizationService _hospital;

        public HospitalizationModel(IHospitalizationService hospitalization)
        {
            _hospital = hospitalization;
        }

        [BindProperty]
        public PaginatedList<HospitalizationResponseDto> Hospitalize { get; set; } = default!;
        public HospitalizationFilterDto HospitalizationFilter { get; set;}
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;
        [BindProperty(SupportsGet = true)]
        public string? SearchDateFrom { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? SearchDateTo { get; set; }
        public async Task<IActionResult> OnGetAsync(int? currentPage)
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
                var searchDateValueFrom = string.IsNullOrEmpty(SearchDateFrom) ? DateOnly.MinValue : DateOnly.Parse(SearchDateFrom);
                var searchDateValueTo = string.IsNullOrEmpty(SearchDateTo) ? DateOnly.MinValue : DateOnly.Parse(SearchDateTo);

                int pagenumber;
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
                var filter = new HospitalizationFilterDto
                {
                    VetId = id
                };
                if (!string.IsNullOrEmpty(SearchDateFrom))
                {
                    filter.FromDate = searchDateValueFrom.ToString();
                }
                if (!string.IsNullOrEmpty(SearchDateTo))
                {
                    filter.ToDate = searchDateValueTo.ToString();
                }
                if (!string.IsNullOrEmpty(SearchDateFrom) && !string.IsNullOrEmpty(SearchDateTo))
                {
                    if (searchDateValueFrom > searchDateValueTo)
                    {
                        filter.FromDate = searchDateValueTo.ToString();
                        filter.ToDate = searchDateValueFrom.ToString();
                        string test = SearchDateFrom;
                        SearchDateFrom = SearchDateTo; 
                        SearchDateTo = test;
                    }
                }
                var hos = await _hospital.GetAllHospitalizationWithFilters(filter, pagenumber, PageSize);
                Hospitalize = hos;
                return Page();
                
            }
            catch (Exception ex)
            {
                Hospitalize = new PaginatedList<HospitalizationResponseDto>();
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
            
        }
        //public async Task OnPostNextPage() // Use OnPost to handle form submission
        //{
        //    string check = "next";
        //    await OnGetAsync(check);
        //}
        private bool IsVetRole(string accountRole)
        {
            // Example check if "admin" is contained in the roles list
            // Adjust this logic based on how roles are stored in your application
            return !string.IsNullOrEmpty(accountRole) && accountRole.Split(',').Contains("Vet");
        }
    }
}
