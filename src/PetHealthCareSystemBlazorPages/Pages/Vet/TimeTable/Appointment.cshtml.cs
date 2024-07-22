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

namespace PetHealthCareSystemRazorPages.Pages.Vet.TimeTable
{
    public class AppointmentModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentModel(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [BindProperty]
        public PaginatedList<AppointmentResponseDto> Appointment { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;
        [BindProperty(SupportsGet = true)]
        public string? SearchDate { get; set; }
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
                var searchDateValueFrom = string.IsNullOrEmpty(SearchDate) ? DateOnly.MinValue : DateOnly.Parse(SearchDate);
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
                if (string.IsNullOrEmpty(SearchDate))
                {
                    Appointment = await _appointmentService.GetVetAppointmentsAsync(id, date, pagenumber, PageSize);
                }
                else
                {
                    Appointment = await _appointmentService.GetVetAppointmentsAsync(id, searchDateValueFrom.ToString(), pagenumber, PageSize);
                }
                
                return Page();
                
            }
            catch (Exception ex)
            {
                Appointment = new PaginatedList<AppointmentResponseDto>();
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
