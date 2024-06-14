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
using BusinessObject.DTO.Pet;

namespace PetHealthCareSystemRazorPages.Pages.PetManagementPage
{
    public class CreateModel : PageModel
    {
        private readonly IPetService _petService;
        //private readonly IUserService _userService;

        public CreateModel( IPetService petService/*, IUserService userService*/)
        {
            _petService = petService;
            //_userService = userService;
        }

        //public IActionResult OnGet()
        //{
        //ViewData["OwnerID"] = new SelectList(_petService.Users, "Id", "Id");
        //    return Page();
        //}

        [BindProperty]
        public PetRequestDto Pet { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _petService.CreatePetAsync(Pet, 2002);

            return RedirectToPage("./Index");
        }
    }
}
