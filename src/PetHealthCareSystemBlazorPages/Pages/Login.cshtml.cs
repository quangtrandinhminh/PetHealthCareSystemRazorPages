using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject.Entities.Identity;
using DataAccessLayer;
using Utility.Exceptions;
using Service.IServices;
using BusinessObject.DTO.User;
using NuGet.Protocol.Plugins;

namespace PetHealthCareSystemBlazorPages.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IAuthService _authService;

        public LoginModel(IAuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public LoginDto LoginUser { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var response = await _authService.Authenticate(LoginUser);
                if (response == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
                else
                {
                    HttpContext.Session.SetString("Token", response.Token);
                    HttpContext.Session.SetString("UserId", response.Id);
                    HttpContext.Session.SetString("Username", response.UserName);
                    HttpContext.Session.SetString("Role", string.Join(",", response.Role));
                }



                return RedirectToPage("/");

            }
            catch (AppException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

    }
}
