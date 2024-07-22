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

        public PaginatedList<ServiceResponseDto> Service { get;set; } = default!;
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;
        public async Task OnGetAsync(int? currentPage)
        {
            try
            {
                int pagenumber;
                if (currentPage == null)
                {
                    pagenumber = 1;
                }
                else
                {
                    pagenumber = (int)currentPage;
                }
                Service = await _service.GetAllServiceAsync(pagenumber, PageSize);
            }
            catch (Exception ex)
            {
                Service = new PaginatedList<ServiceResponseDto>();
                ModelState.AddModelError(string.Empty, ex.Message);
            }
        }
    }
}
