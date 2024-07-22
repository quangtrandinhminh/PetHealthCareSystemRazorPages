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
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Utility.Enum;

namespace PetHealthCareSystemRazorPages.Pages.BookAppointment
{
    public class BookOfflineAppointmentModel : PageModel
    {
        private readonly IPetService _petService;
        private readonly IService _service;
        private readonly IAppointmentService _appointmentService;
        private readonly ILogger<BookOfflineAppointmentModel> _logger;
        private readonly IUserService _userService;

        public BookOfflineAppointmentModel(IPetService petService, IService service, IAppointmentService appointmentService, ILogger<BookOfflineAppointmentModel> logger, IUserService userService)
        {
            _petService = petService;
            _service = service;
            _appointmentService = appointmentService;
            _logger = logger;
            _userService = userService;
        }

        [BindProperty]
        public AppointmentBookRequestDto AppointmentBookRequest { get; set; }

        public List<PetResponseDto> DisplayedPetList { get; set; }
        public List<ServiceResponseDto> DisplayedServiceList { get; set; }
        public List<TimeTableResponseDto> DisplayedTimeTableList { get; set; }
        public List<UserResponseDto> DisplayedVetList { get; set; }
        public UserResponseDto Customer { get; set; }
        public IList<UserResponseDto> CustomerList { get; set; }


        public async Task OnGetAsync()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role == null || !role.Contains(UserRole.Staff.ToString()))
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
                CustomerList = await _userService.GetAllUsersByRoleAsync(UserRole.Customer);
                DisplayedServiceList = await _service.GetAllServiceAsync();
                DisplayedTimeTableList = await _appointmentService.GetAllTimeFramesForBookingAsync(1, new DateOnly());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing data.");
            }
        }

        public async Task<IActionResult> OnPost(int petId, string serviceIds, string appointmentDate, int timeTableId, int vetId, int customerId)
        {

            var testid = petId;

            var userId = int.Parse(HttpContext.Session.GetString("UserId"));

            AppointmentBookRequest = new AppointmentBookRequestDto
            {
                PetIdList = ConvertStringToIntList(petId.ToString()),
                ServiceIdList = ConvertStringToIntList(serviceIds),
                AppointmentDate = appointmentDate,
                TimeTableId = timeTableId,
                VetId = vetId,
                CustomerId = customerId
            };

            var appointmentResponse = await _appointmentService.BookAppointmentAsync(AppointmentBookRequest, userId);
            HttpContext.Session.SetString("appointment", JsonSerializer.Serialize(appointmentResponse));
            return RedirectToPage("/Staff/Appointment/BookingManagement");
        }

        public async Task<JsonResult> OnGetVetByDateAndTime(string date, int timeTableId)
        {
            try
            {
                var appointmentDate = DateOnly.Parse(date).ToString();
                var datetime = new DateTimeQueryDto { Date = appointmentDate, TimetableId = timeTableId };
                var vetList = await _appointmentService.GetFreeWithTimeFrameAndDateAsync(datetime);
                return new JsonResult(vetList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching vets.");
                return new JsonResult(new { success = false, message = "Error fetching vets." });
            }
        }

        public async Task<JsonResult> OnGetPetByCustomer(int customerId)
        {
            try
            {
                var petList = await _petService.GetAllPetsForCustomerAsync(customerId);
                return new JsonResult(petList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching pets.");
                return new JsonResult(new { success = false, message = "Error fetching pets." });
            }
        }

        private List<int> ConvertStringToIntList(string input)
        {
            return input.Split(',')
                        .Select(int.Parse)
                        .ToList();
        }
    }
}
