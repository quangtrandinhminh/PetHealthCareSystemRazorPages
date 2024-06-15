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

namespace PetHealthCareSystemRazorPages.Pages
{
    public class HomePageModel : PageModel
    {
        private readonly IUserService _userService;

        public HomePageModel(IUserService userService)
        {
            _userService = userService;
        }

        public IList<UserResponseDto> List { get;set; } = default!;

        public async Task OnGetAsync()
        {
            List = await _userService.GetVetsAsync();
        }
    }
}
