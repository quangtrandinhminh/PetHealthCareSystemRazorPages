using BusinessObject.DTO.Appointment;
using BusinessObject.DTO.Pet;
using BusinessObject.DTO.Transaction;
using BusinessObject.DTO.User;
using BusinessObject.DTO.Vet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.IServices;
using Service.Services;
using System.Text.Json;

namespace PetHealthCareSystemRazorPages.Pages.BookAppointment
{
    public class TransactionFormModel : PageModel
    {
        private readonly IPetService _petService;
        private readonly IAppointmentService _appointmentService;
        private readonly IService _service;
        private readonly ITransactionService _transactionService;
        private readonly IUserService _userService;

        public TransactionFormModel(IPetService petService, IAppointmentService appointmentService, IService service, ITransactionService transactionService, IUserService userService)
        {
            _petService = petService;
            _appointmentService = appointmentService;
            _service = service;
            _transactionService = transactionService;
            _userService = userService;
        }

        public void OnGet()
        {
        }

        public AppointmentBookRequestDto AppointmentBookRequestDto { get; set; }

        public List<PetResponseDto> SelectedPets {  get; set; }

        public UserResponseDto SelectedVet { get; set; }

        [BindProperty]
        public TransactionDropdownDto TransactionDropdownDto { get; set; }

        [BindProperty]
        public List<TransactionServicesDto> TransactionServices { get; set; }

        public void OnPost()
        {
        }

        public async void InitializeData()
        {
            AppointmentBookRequestDto = LoadObjectFromSession();

            try
            {
                TransactionDropdownDto = _transactionService.GetTransactionDropdownData();
                TransactionServices = CreateTransactionServices(AppointmentBookRequestDto.ServiceIdList);
                SelectedPets = await LoadSelectedPetList(AppointmentBookRequestDto.PetIdList);
                SelectedVet = await _userService.GetVetByIdAsync(AppointmentBookRequestDto.VetId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString(), ex);
            }
        }

        private async Task<List<PetResponseDto>> LoadSelectedPetList(List<int> petIdList)
        {
            var petList = new List<PetResponseDto>();
            foreach (var pet in petIdList)
            {
                PetResponseDto petResponseDto = await _petService.GetPetByIdAsync(pet);
                petList.Add(petResponseDto);
            }
            return petList;
        }


        private static List<TransactionServicesDto> CreateTransactionServices(List<int> serviceIdList)
        {
            var transactionServices = new List<TransactionServicesDto>();
            foreach (var serviceId in serviceIdList)
            {
                transactionServices.Add(new TransactionServicesDto { ServiceId = serviceId, Quantity = 1 });
            }
            return transactionServices;
        }

        public AppointmentBookRequestDto LoadObjectFromSession()
        {
            var tempString = HttpContext.Session.GetString("BookAppointmentRequest");
            if (tempString != null)
            {
                return JsonSerializer.Deserialize<AppointmentBookRequestDto>(tempString);
            }
            else
            {
                RedirectToPage("BookingForm");
                return null;
            }
        }
    }
}
