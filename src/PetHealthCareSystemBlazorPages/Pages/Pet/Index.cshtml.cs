using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Entities;
using Service.IServices;
using Service.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using BusinessObject.DTO.Pet;
using System.Data;
using Utility.Enum;

namespace PetHealthCareSystemRazorPages.Pages.Pet
{
    public class IndexModel : PageModel
    {
        private readonly IPetService _petService;

        public IndexModel(IPetService petService)
        {
            _petService = petService;
        }

        public IList<PetResponseDto> Pet { get; set; }

        public async Task OnGetAsync()
        {
            var userId = Int32.Parse(HttpContext.Session.GetString("UserId"));

            var role = HttpContext.Session.GetString("Role");

            if (role == null || !role.Contains(UserRole.Customer.ToString()))
            {
                Response.Redirect("/Login");
                return;
            }

            Pet = await _petService.GetAllPetsForCustomerAsync(userId);

        }
    }
}
