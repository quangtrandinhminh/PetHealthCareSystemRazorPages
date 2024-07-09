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
using BusinessObject.DTO.User;
using Utility.Enum;
using Utility.Exceptions;

namespace PetHealthCareSystemRazorPages.Pages
{
    public class HomePageModel : PageModel
    {
        private readonly IUserService _userService;

        public HomePageModel(IUserService userService)
        {
            _userService = userService;
        }

        public IList<UserResponseDto> List { get;set; } = new List<UserResponseDto>();

        public async Task OnGetAsync()
        {
            try
            {
                List = await _userService.GetAllUsersByRoleAsync(UserRole.Vet);
            }
            catch (AppException ex) 
            {
                List = new List<UserResponseDto>();
                ModelState.AddModelError(string.Empty, ex.Message);
            }
        }
    }
}
