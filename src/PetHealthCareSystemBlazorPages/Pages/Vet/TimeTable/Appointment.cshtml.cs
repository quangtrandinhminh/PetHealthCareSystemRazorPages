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

namespace PetHealthCareSystemRazorPages.Pages.Vet.TimeTable
{
    public class AppointmentModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentModel(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public PaginatedList<AppointmentResponseDto> Appointment { get;set; } = default!;

        public async Task OnGetAsync()
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
                int pagenumber = 1;
                int pagesize = 5;
                int id = int.Parse(accountId);
                var date = DateTime.Now.ToString("yyyy-MM-dd");
                Appointment = await _appointmentService.GetVetAppointmentsAsync(id, date, pagenumber, pagesize);
            }
            catch (Exception ex)
            {
                Appointment = new PaginatedList<AppointmentResponseDto>();
                ModelState.AddModelError(string.Empty, ex.Message);
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
