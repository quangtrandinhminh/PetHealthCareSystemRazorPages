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
            Service = await _service.GetAllServiceAsync();
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
    }
}
