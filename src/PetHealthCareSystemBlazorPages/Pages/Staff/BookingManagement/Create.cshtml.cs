using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject.DTO.Appointment;
using Service.IServices;
using BusinessObject.DTO.Pet;
using BusinessObject.DTO.Service;
using BusinessObject.DTO.TimeTable;
using BusinessObject.DTO.User;
using Utility.Exceptions;

namespace PetHealthCareSystemRazorPages.Pages.Staff.BookingManagement
{
    public class CreateModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IService _serviceService;
        private readonly IPetService _petService;

        public CreateModel(IAppointmentService appointmentService, IService serviceService, IPetService petService)
        {
            _appointmentService = appointmentService;
            _serviceService = serviceService;
            _petService = petService;
        }

        [BindProperty]
        public AppointmentBookRequestDto AppointmentBookRequestDto { get; set; }
        public List<PetResponseDto> DisplayedPetList { get; set; } = new List<PetResponseDto>();
        public List<ServiceResponseDto> DisplayedServiceList { get; set; } = new List<ServiceResponseDto>();
        public List<TimeTableResponseDto> DisplayedTimeTableList { get; set; } = new List<TimeTableResponseDto>();
        public List<UserResponseDto> DisplayedVetList { get; set; } = new List<UserResponseDto>();

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                ViewData["TimetableId"] = new SelectList(await _appointmentService.GetAllTimeFramesForBookingAsync(), "Id", "Id");
                ViewData["VetId"] = new SelectList(await _appointmentService.GetFreeWithTimeFrameAndDateAsync(DateOnly.FromDateTime(DateTime.Now), 0), "Id", "FullName");

                var services = await _serviceService.GetAllServiceAsync();
                ViewData["Services"] = new SelectList(services, "Id", "Name");

                var pets = await _petService.GetAllPetsForCustomerAsync(0); // Adjust the customer ID accordingly
                ViewData["Pets"] = new SelectList(pets, "Id", "Name");

                return Page();
            }
            catch (AppException ex)
            {
                // Log the exception if needed
                ViewData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    ViewData["TimetableId"] = new SelectList(await _appointmentService.GetAllTimeFramesForBookingAsync(), "Id", "Id", AppointmentBookRequestDto.TimetableId);
                    ViewData["VetId"] = new SelectList(await _appointmentService.GetFreeWithTimeFrameAndDateAsync(DateOnly.FromDateTime(DateTime.Now), AppointmentBookRequestDto.TimetableId), "Id", "FullName", AppointmentBookRequestDto.VetId);

                    var services = await _serviceService.GetAllServiceAsync();
                    ViewData["Services"] = new SelectList(services, "Id", "Name", AppointmentBookRequestDto.ServiceIdList);

                    var pets = await _petService.GetAllPetsForCustomerAsync(0); // Adjust the customer ID accordingly
                    ViewData["Pets"] = new SelectList(pets, "Id", "Name", AppointmentBookRequestDto.PetIdList);

                    return Page();
                }
                catch (AppException ex)
                {
                    // Log the exception if needed
                    ViewData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                    return Page();
                }
            }

            try
            {
                await _appointmentService.BookOnlineAppointmentAsync(AppointmentBookRequestDto);
                return RedirectToPage("./Index");
            }
            catch (AppException ex)
            {
                // Log the exception if needed
                ViewData["ErrorMessage"] = $"An error occurred while creating the appointment: {ex.Message}";
                return Page();
            }
        }
    }
}
