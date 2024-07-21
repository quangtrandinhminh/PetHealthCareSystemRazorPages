using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Entities;
using DataAccessLayer;
using Service.IServices;
using BusinessObject.DTO.Service;

namespace PetHealthCareSystemRazorPages.Pages.Service
{
    public class EditModel : PageModel
    {
        private readonly IService _service;

        public EditModel(IService service)
        {
            _service = service;
        }

        [BindProperty]
        public ServiceResponseDto Service { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var accountId = HttpContext.Session.GetString("UserId"); // Assuming UserId is stored in Session
            var accountRole = HttpContext.Session.GetString("Role");
            // Check if accountId is null or empty or if accountRole is not "admin" (assuming "admin" role is stored as such)
            if (string.IsNullOrEmpty(accountId) || !IsAdminRole(accountRole))
            {
                Response.Redirect("/");
            }

            var service =  await _service.GetServiceBydId(id.Value);
            if (service == null)
            {
                return NotFound();
            }
            Service = service;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var accountId = HttpContext.Session.GetString("UserId");
            int id = int.Parse(accountId);

            try
            {
                var test = new ServiceUpdateDto{
                    Id = Service.Id,
                    Description = Service.Description,
                    Name = Service.Name,
                    Price = Service.Price
                };
                var check = await _service.GetAllServiceAsync();
                var check2 = await _service.GetServiceBydId(Service.Id);
                if (check2.Name != Service.Name)
                {
                    if (check.FirstOrDefault(x => x.Name.Equals(Service.Name)) != null)
                    {
                        ModelState.AddModelError("Service.Name", "ten service da ton tai");
                        return Page();
                    }
                }
                
                await _service.UpdateServiceAsync(test, id);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty,ex.Message);
                return Page();
            }

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
