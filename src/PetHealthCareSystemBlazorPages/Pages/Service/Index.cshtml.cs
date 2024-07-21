using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Entities;
using DataAccessLayer;
using BusinessObject.DTO.Service;
using Service.IServices;
using Azure;
using BusinessObject.DTO.Appointment;
using Microsoft.Identity.Client;
using Repository.Extensions;
using Service.Services;

namespace PetHealthCareSystemRazorPages.Pages.Service
{
    public class IndexModel : PageModel
    {
        private readonly IService _service;

        public IndexModel(IService service)
        {
            _service = service;
        }

        public List<ServiceResponseDto> Service { get;set; } = default!;

        public async Task OnGetAsync()
        {
            var accountId = HttpContext.Session.GetString("UserId"); // Assuming UserId is stored in Session
            var accountRole = HttpContext.Session.GetString("Role");
            // Check if accountId is null or empty or if accountRole is not "admin" (assuming "admin" role is stored as such)
            if (string.IsNullOrEmpty(accountId) || !IsAdminRole(accountRole))
            {
                Response.Redirect("/");
            }
            try
            {
                Service = await _service.GetAllServiceAsync();
            }
            catch (Exception ex)
            {
                Service = new List<ServiceResponseDto>();
                ModelState.AddModelError(string.Empty, ex.Message);
            }
        }
        private bool IsAdminRole(string accountRole)
        {
            // Example check if "admin" is contained in the roles list
            // Adjust this logic based on how roles are stored in your application
            return !string.IsNullOrEmpty(accountRole) && accountRole.Split(',').Contains("Admin");
        }
    }
}
