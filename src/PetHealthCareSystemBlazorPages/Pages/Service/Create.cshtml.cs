﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject.Entities;
using DataAccessLayer;
using Service.IServices;
using Service.Services;
using BusinessObject.DTO.Service;

namespace PetHealthCareSystemRazorPages.Pages.Service
{
    public class CreateModel : PageModel
    {
        private readonly IService _service;

        public CreateModel(IService service)
        {
            _service = service;
        }

        public IActionResult OnGet()
        {
            
            var accountId = HttpContext.Session.GetString("UserId"); // Assuming UserId is stored in Session
            var accountRole = HttpContext.Session.GetString("Role");
            // Check if accountId is null or empty or if accountRole is not "admin" (assuming "admin" role is stored as such)
            if (string.IsNullOrEmpty(accountId) || !IsAdminRole(accountRole))
            {
                Response.Redirect("/");
            }
            return Page();
        }

        [BindProperty]
        public ServiceRequestDto Service { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userid = Int32.Parse(HttpContext.Session.GetString("UserId"));
            var check = await _service.GetAllServiceAsync();
            if (check.FirstOrDefault(x => x.Name.Equals(Service.Name)) != null) { 
                ModelState.AddModelError("Service.Name", "ten service da ton tai");
                return Page();
            }
            await _service.CreateServiceAsync(Service, userid);

            return RedirectToPage("./Index");
        }
        private bool IsAdminRole(string accountRole)
        {
            // Example check if "admin" is contained in the roles list
            // Adjust this logic based on how roles are stored in your application
            return !string.IsNullOrEmpty(accountRole) && accountRole.Split(',').Contains("Admin");
        }
    }
}
