using BusinessObject.DTO.Appointment;
using BusinessObject.DTO.Pet;
using BusinessObject.DTO.Service;
using BusinessObject.DTO.TimeTable;
using BusinessObject.DTO.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.IServices;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace PetHealthCareSystemRazorPages.Pages.BookAppointment
{
    public class BookingFormModel : PageModel
    {
        private readonly IPetService _petService;
        private readonly IService _service;
        private readonly IAppointmentService _appointmentService;
        private readonly IHttpContextAccessor _contextAccessor;

        public BookingFormModel(IPetService petService, IService service, IAppointmentService appointmentService, IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            _petService = petService;
            _service = service;
            _appointmentService = appointmentService;
        }

        [BindProperty]
        public AppointmentBookRequestDto AppointmentBookRequest { get; set; }

        public List<PetResponseDto> DisplayedPetList { get; set; }
        public List<ServiceResponseDto> DisplayedServiceList { get; set; }
        public List<TimeTableResponseDto> DisplayedTimeTableList { get; set; }
        public List<UserResponseDto> DisplayedVetList { get; set; }

        public async Task OnGetAsync()
        {
            AppointmentBookRequest = new AppointmentBookRequestDto();
            await InitializeData(); // Ensure to await this method
        }

        private async Task InitializeData()
        {
            DisplayedPetList = await _petService.GetAllPetsForCustomerAsync(2002);
            DisplayedServiceList = await _service.GetAllServiceAsync();
            DisplayedTimeTableList = await _appointmentService.GetAllTimeFramesForBookingAsync();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync(); // Reinitialize lists if validation fails
                return Page();
            }

            // Process form submission (e.g., save data)

            return RedirectToPage("TransactionPage");
        }

        public void SaveObjectToSession()
        {
            HttpContext.Session.SetString("BookAppointmentRequest", JsonSerializer.Serialize(AppointmentBookRequest));
        }

        public async Task<JsonResult> OnGetVetByDateAndTime(string date, int timeTableId)
        {
            var appointmentDate = DateOnly.Parse(date);
            var vetList = await _appointmentService.GetFreeWithTimeFrameAndDateAsync(appointmentDate, timeTableId);
            return new JsonResult(vetList);
        }
    }
}
