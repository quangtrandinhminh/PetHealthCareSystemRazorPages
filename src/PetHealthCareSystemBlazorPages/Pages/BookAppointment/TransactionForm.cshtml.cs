using Azure;
using BusinessObject.DTO.Appointment;
using BusinessObject.DTO.Pet;
using BusinessObject.DTO.Transaction;
using BusinessObject.DTO.User;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.IServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using Utility.Enum;

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

        public AppointmentResponseDto AppointmentResponseDto { get; set; }
        public List<PetResponseDto> SelectedPets { get; set; }
        public UserResponseDto SelectedVet { get; set; }
        public TransactionDropdownDto TransactionDropdownDto { get; set; }
        public List<TransactionServicesDto> TransactionServices { get; set; }

        public async Task OnGet()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role == null || !role.Contains(UserRole.Customer.ToString()))
            {
                Response.Redirect("/Login");
                return;
            }

            await InitializeData();
        }

        public async Task OnPost(int paymentMethod, string appointmentResponseDtoJson)
        {
            AppointmentResponseDto = JsonSerializer.Deserialize<AppointmentResponseDto>(appointmentResponseDtoJson);
            TransactionDropdownDto = _transactionService.GetTransactionDropdownData();

            if (AppointmentResponseDto == null)
            {
                throw new Exception("AppointmentResponseDto is null.");
            }

            TransactionRequestDto transactionRequestDto = new TransactionRequestDto
            {
                PaymentMethod = paymentMethod,
                Services = TransactionServices,
                PaymentDate = DateTime.Now,
                AppointmentId = AppointmentResponseDto.Id,
                Status = 1
            };

            var userId = int.Parse(HttpContext.Session.GetString("UserId"));

            await _transactionService.CreateTransactionAsync(transactionRequestDto, userId);

            Response.Redirect("/Login");
            return;
        }

        private async Task InitializeData()
        {
            var tempData = HttpContext.Session.GetString("appointment");

            try
            {
                AppointmentResponseDto = JsonSerializer.Deserialize<AppointmentResponseDto>(tempData);
                TransactionDropdownDto = _transactionService.GetTransactionDropdownData();

                if (AppointmentResponseDto != null)
                {
                    SelectedPets = await LoadSelectedPetList(AppointmentResponseDto.Pets.Select(p => p.Id).ToList());
                    SelectedVet = AppointmentResponseDto.Vet;
                    var quantity = AppointmentResponseDto.Pets.Count;
                    TransactionServices = CreateTransactionServices(AppointmentResponseDto.Services.Select(s => s.Id).ToList(), quantity);
                }
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

        private static List<TransactionServicesDto> CreateTransactionServices(List<int> serviceIdList, int quantity)
        {
            var transactionServices = new List<TransactionServicesDto>();
            foreach (var serviceId in serviceIdList)
            {
                transactionServices.Add(new TransactionServicesDto { ServiceId = serviceId, Quantity = quantity });
            }
            return transactionServices;
        }
    }
}