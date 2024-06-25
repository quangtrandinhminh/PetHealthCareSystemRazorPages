using BusinessObject.DTO.Appointment;
using BusinessObject.DTO.Pet;
using BusinessObject.DTO.Service;
using BusinessObject.DTO.TimeTable;
using BusinessObject.DTO.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PetHealthCareSystemRazorPages.Pages.BookAppointment
{
    public class BookingFormModel : PageModel
    {
        private readonly IPetService _petService;
        private readonly IService _service;
        private readonly IAppointmentService _appointmentService;

        public BookingFormModel(IPetService petService, IService service, IAppointmentService appointmentService)
        {
            _petService = petService;
            _service = service;
            _appointmentService = appointmentService;
        }

        [BindProperty]
        public AppointmentBookRequestDto AppointmentBookRequest { get; set; }

        public List<PetResponseDto> DisplayedPetList { get; set; } = new List<PetResponseDto>();
        public List<ServiceResponseDto> DisplayedServiceList { get; set; } = new List<ServiceResponseDto>();
        public List<TimeTableResponseDto> DisplayedTimeTableList { get; set; } = new List<TimeTableResponseDto>();
        public List<UserResponseDto> DisplayedVetList { get; set; } = new List<UserResponseDto>();

        public async Task OnGetAsync()
        {
            AppointmentBookRequest = new AppointmentBookRequestDto(); // Initialize here
            DisplayedPetList = await _petService.GetAllPetsForCustomerAsync(2002);
            DisplayedServiceList = await _service.GetAllServiceAsync();
            DisplayedTimeTableList = await _appointmentService.GetAllTimeFramesForBookingAsync();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                OnGetAsync(); // Reinitialize lists if validation fails
                return Page();
            }

            // Process form submission (e.g., save data)

            return RedirectToPage("TransactionPage");
        }

        public async Task<IActionResult> OnGetVetsAsync()
        {
            if (AppointmentBookRequest != null && !string.IsNullOrEmpty(AppointmentBookRequest.AppointmentDate) && AppointmentBookRequest.TimetableId != 0)
            {
                // Parse the date string to DateOnly if needed
                DateOnly appointmentDate = DateOnly.Parse(AppointmentBookRequest.AppointmentDate);

                DisplayedVetList = await _appointmentService.GetFreeWithTimeFrameAndDateAsync(appointmentDate, AppointmentBookRequest.TimetableId);
            }
            else
            {
                DisplayedVetList.Clear(); // Clear the list if date or timetable is not selected
            }

            // Return the partial view with the updated model
            return Partial("_VetOptionsPartial", this);
        }

    }
}
