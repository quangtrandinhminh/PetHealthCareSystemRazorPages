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
using BusinessObject.DTO.Service;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace PetHealthCareSystemRazorPages.Pages.Service
{
    public class DeleteModel : PageModel
    {
        private readonly IService _service;

        public DeleteModel(IService service)
        {
            _service = service;
        }

        [BindProperty]
        public ServiceResponseDto Service { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var accountId = HttpContext.Session.GetString("UserId"); // Assuming UserId is stored in Session
            var accountRole = HttpContext.Session.GetString("Role");
            // Check if accountId is null or empty or if accountRole is not "admin" (assuming "admin" role is stored as such)
            if (string.IsNullOrEmpty(accountId) || !IsAdminRole(accountRole))
            {
                Response.Redirect("/");
            }
            if (id == null)
            {
                return RedirectToPage("./Index");
            }

            var service = await _service.GetServiceBydId((int)id);

            if (service == null)
            {
                return RedirectToPage("./Index");
            }
            else
            {
                Service = service;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index");
            }
            try
            {
                var accountId = HttpContext.Session.GetString("UserId"); // Assuming UserId is stored in Session
                int accid = int.Parse(accountId);
                await _service.DeleteServiceAsync(id.Value, accid);
                return RedirectToPage("./Index");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToPage("./Index");
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
