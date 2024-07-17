using System;
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

            await _service.CreateServiceAsync(Service, userid);

            return RedirectToPage("./Index");
        }
    }
}
