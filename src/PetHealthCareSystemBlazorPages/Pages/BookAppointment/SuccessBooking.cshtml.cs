using BusinessObject.DTO.Appointment;
using BusinessObject.DTO.Service;
using BusinessObject.DTO.Transaction;
using BusinessObject.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.IServices;
using Service.Services;
using Utility.Enum;
using Azure;
using BusinessObject.DTO.Pet;
using BusinessObject.DTO.User;
using BusinessObject.DTO.VNPay;
using System.Text.Json;


namespace PetHealthCareSystemRazorPages.Pages.BookAppointment
{
    public class SuccessBookingModel : PageModel
    {
        private readonly IVnPayService _vpnPayService;
        private readonly IAppointmentService _appointmentService;
        private readonly ITransactionService _transactionService;
        private readonly IUserService _userService;
        private readonly IPetService _petService;
        private readonly IService _service;

        public SuccessBookingModel(IVnPayService vpnPayService, IAppointmentService appointmentService, ITransactionService transactionService, IUserService userService, IPetService petService, IService service)
        {
            _vpnPayService = vpnPayService;
            _appointmentService = appointmentService;
            _transactionService = transactionService;
            _userService = userService;
            _petService = petService;
            _service = service;
        }

        public AppointmentBookRequestDto AppointmentBookRequestDto { get; set; }
        public List<PetResponseDto> SelectedPets { get; set; }
        public UserResponseDto SelectedVet { get; set; }
        public List<TransactionServicesDto> TransactionServices { get; set; }
        public decimal Total { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role == null || !role.Contains(UserRole.Customer.ToString()))
            {
                return RedirectToPage("/Login");
            }

            var tempData = HttpContext.Session.GetString("appointment");

            try
            {
                AppointmentBookRequestDto = JsonSerializer.Deserialize<AppointmentBookRequestDto>(tempData);

                if (AppointmentBookRequestDto != null)
                {
                    SelectedPets = await LoadSelectedPetListAsync(AppointmentBookRequestDto.PetIdList);
                    SelectedVet = await _userService.GetVetByIdAsync(AppointmentBookRequestDto.VetId);
                    var quantity = AppointmentBookRequestDto.PetIdList.Count;
                    TransactionServices = CreateTransactionServices(AppointmentBookRequestDto.ServiceIdList, quantity);

                    var serviceList = new List<ServiceResponseDto>();

                    foreach (var serviceId in AppointmentBookRequestDto.ServiceIdList)
                    {
                        var service = await _service.GetServiceBydId(serviceId);
                        serviceList.Add(service);
                    }

                    Total = PaymentCalculation(1, serviceList);
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Failed to initialize data: " + ex.Message;
                return Page();
            }

            var response = ExecutePayment();
            if (!response.Item1)
            {
                TempData["Message"] = response.Item2;
                return RedirectToPage("/BookAppointment/PaymentFail");
            }

            try
            {
                var bookAppointment = await _appointmentService.BookAppointmentAsync(AppointmentBookRequestDto, AppointmentBookRequestDto.CustomerId);

                var transactionRequest = new TransactionRequestDto
                {
                    AppointmentId = bookAppointment.Id,
                    PaymentMethod = 2,
                    PaymentDate = DateTimeOffset.Now,
                    PaymentId = response.Item3.OrderId,
                    Status = 2,
                    Services = CreateTransactionServices(AppointmentBookRequestDto.ServiceIdList, 1)
                };

                await _transactionService.CreateTransactionAsync(transactionRequest, AppointmentBookRequestDto.CustomerId);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Failed to save appointment: " + ex.Message;
                return Page();
            }

            TempData["Message"] = "Payment Success!";
            return RedirectToPage("SuccessBooking");
        }

        private (bool, string, VnPaymentResponseDto) ExecutePayment()
        {
            var response = _vpnPayService.PaymentExecute(Request.Query);

            if (response == null || response.VnPayResponseCode != "00")
            {
                return (false, $"Payment Fail: {response?.VnPayResponseCode ?? "Unknown error"}", null);
            }

            return (true, "Payment Success", response);
        }

        private async Task<List<PetResponseDto>> LoadSelectedPetListAsync(List<int> petIdList)
        {
            var petList = new List<PetResponseDto>();
            foreach (var pet in petIdList)
            {
                var petResponseDto = await _petService.GetPetByIdAsync(pet);
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

        private static decimal PaymentCalculation(int quantity, List<ServiceResponseDto> serviceList)
        {
            decimal total = 0;
            foreach (var service in serviceList)
            {
                total += service.Price * quantity;
            }
            return total;
        }
    }
}
