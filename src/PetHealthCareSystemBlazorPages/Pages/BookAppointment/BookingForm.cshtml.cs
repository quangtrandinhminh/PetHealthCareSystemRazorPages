using BusinessObject.DTO.Appointment;
using BusinessObject.DTO.Pet;
using BusinessObject.DTO.Service;
using BusinessObject.DTO.TimeTable;
using BusinessObject.DTO.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Service.IServices;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Utility.Enum;

namespace PetHealthCareSystemRazorPages.Pages.BookAppointment
{
    public class BookingFormModel : PageModel
    {
        private readonly IPetService _petService;
        private readonly IService _service;
        private readonly IAppointmentService _appointmentService;
        private readonly ILogger<BookingFormModel> _logger;

        public BookingFormModel(IPetService petService, IService service, IAppointmentService appointmentService, ILogger<BookingFormModel> logger)
        {
            _petService = petService;
            _service = service;
            _appointmentService = appointmentService;
            _logger = logger;
        }

        [BindProperty]
        public AppointmentBookRequestDto AppointmentBookRequest { get; set; }

        public List<PetResponseDto> DisplayedPetList { get; set; }
        public List<ServiceResponseDto> DisplayedServiceList { get; set; }
        public List<TimeTableResponseDto> DisplayedTimeTableList { get; set; }
        public List<UserResponseDto> DisplayedVetList { get; set; }


        public async Task OnGetAsync()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role == null || !role.Contains(UserRole.Customer.ToString()))
            {
                Response.Redirect("/Login");
                return;
            }

            AppointmentBookRequest = new AppointmentBookRequestDto();
            await InitializeData();
        }

        private async Task InitializeData()
        {
            try
            {
                var userId = int.Parse(HttpContext.Session.GetString("UserId"));
                DisplayedPetList = await _petService.GetAllPetsForCustomerAsync(userId);
                DisplayedServiceList = await _service.GetAllServiceAsync();
                DisplayedTimeTableList = await _appointmentService.GetAllTimeFramesForBookingAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing data.");
            }
        }

        public async Task<IActionResult> OnPost()
        {
            /*if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }*/

            // Process form submission (e.g., save data)

                var bookingAppointmentRequest = new AppointmentBookRequestDto
                {
                    ServiceIdList = AppointmentBookRequest.ServiceIdList.ToList(),
                    PetIdList = AppointmentBookRequest.PetIdList.ToList(),
                    AppointmentDate = AppointmentBookRequest.AppointmentDate,
                    TimetableId = AppointmentBookRequest.TimetableId,
                    VetId = AppointmentBookRequest.VetId
                };

                var userId = int.Parse(HttpContext.Session.GetString("UserId"));
                await _appointmentService.BookOnlineAppointmentAsync(bookingAppointmentRequest, userId);
                return RedirectToPage("TransactionPage");

        }

        public void SaveObjectToSession()
        {
            HttpContext.Session.SetString("BookAppointmentRequest", JsonSerializer.Serialize(AppointmentBookRequest));
        }

        public async Task<JsonResult> OnGetVetByDateAndTime(string date, int timeTableId)
        {
            try
            {
                var appointmentDate = DateOnly.Parse(date).ToString();
                var datetime = new AppointmentDateTimeQueryDto { Date = appointmentDate, TimetableId = timeTableId };
                var vetList = await _appointmentService.GetFreeWithTimeFrameAndDateAsync(datetime);
                return new JsonResult(vetList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching vets.");
                return new JsonResult(new { success = false, message = "Error fetching vets." });
            }
        }

    }
}
