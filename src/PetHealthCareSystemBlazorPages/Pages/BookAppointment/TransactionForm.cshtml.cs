using Azure;
using BusinessObject.DTO.Appointment;
using BusinessObject.DTO.Pet;
using BusinessObject.DTO.Service;
using BusinessObject.DTO.TimeTable;
using BusinessObject.DTO.Transaction;
using BusinessObject.DTO.User;
using BusinessObject.DTO.VNPay;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service.IServices;
using System.Text.Json;
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
        private readonly IVnPayService _vnPayService;

        public TransactionFormModel(IPetService petService, IAppointmentService appointmentService, IService service, ITransactionService transactionService, IUserService userService, IVnPayService vnPayService)
        {
            _petService = petService;
            _appointmentService = appointmentService;
            _service = service;
            _transactionService = transactionService;
            _userService = userService;
            _vnPayService = vnPayService;

            // Initialize TimeTableResponseDto
            TimeTableResponseDto = new TimeTableResponseDto();
        }

        public AppointmentBookRequestDto AppointmentBookRequestDto { get; set; }
        public List<PetResponseDto> SelectedPets { get; set; }
        public UserResponseDto SelectedVet { get; set; }
        public TransactionDropdownDto TransactionDropdownDto { get; set; }
        public List<TransactionServicesDto> TransactionServices { get; set; }
        public TimeTableResponseDto TimeTableResponseDto { get; set; }
        public List<ServiceResponseDto> ServicesList { get; set; }
        public decimal Total { get; set; }
        public int PaymentMethodInput { get; set; }
        public int VnPayId { get; set; }

        public async Task OnGetAsync()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role == null || !role.Contains(UserRole.Customer.ToString()))
            {
                Response.Redirect("/Login");
                return;
            }

            await InitializeDataAsync();
        }

        private async Task InitializeDataAsync()
        {
            var tempData = HttpContext.Session.GetString("appointment");
            TransactionDropdownDto = _transactionService.GetTransactionDropdownData();

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

                    ServicesList = serviceList;

                    Total = PaymentCalculation(1, serviceList);

                    // Example of setting TimeTableResponseDto - ensure this is correct
                    TimeTableResponseDto = await _appointmentService.GetTimeTableByIdAsync(AppointmentBookRequestDto.TimeTableId);

                    if (TimeTableResponseDto == null)
                    {
                        TempData["Message"] = "TimeTableResponseDto is null.";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Failed to initialize data: " + ex.Message;
            }
        }


        public async Task<IActionResult> OnPostAsync(int paymentMethod, string appointmentResponseDtoJson)
        {
            PaymentMethodInput = paymentMethod;
            AppointmentBookRequestDto = JsonSerializer.Deserialize<AppointmentBookRequestDto>(appointmentResponseDtoJson);

            var tempData = HttpContext.Session.GetString("appointment");

            if (AppointmentBookRequestDto == null)
            {
                TempData["Message"] = "Appointment details are missing.";
                return Page();
            }

            if (paymentMethod == 2)
            {
                var response = await HandleVnPayPaymentAsync();
                if (response != null)
                {
                    return Redirect(response);
                }
                TempData["Message"] = "Failed to create VNPay payment URL.";
                return Page();
            }

            await ProcessBookingAndTransactionAsync();

            TempData["Message"] = "Payment Success!";
            return RedirectToPage("PaymentSuccess");
        }

        private async Task<string> HandleVnPayPaymentAsync()
        {
            var tempData = HttpContext.Session.GetString("appointment");
            AppointmentBookRequestDto = JsonSerializer.Deserialize<AppointmentBookRequestDto>(tempData);

            var serviceList = new List<ServiceResponseDto>();
            var customer = await _userService.GetByIdAsync(AppointmentBookRequestDto.CustomerId);

            foreach (var serviceId in AppointmentBookRequestDto.ServiceIdList)
            {
                var service = await _service.GetServiceBydId(serviceId);
                serviceList.Add(service);
            }

            Total = PaymentCalculation(1, serviceList);

            var vnPayModel = new VnPaymentRequestDto
            {
                Amount = (double)Total,
                Description = "Appointment Payment",
                FullName = customer.FullName,
                OrderId = new Random().Next(10000000, 99999999),
                ReturnUrl = "http://localhost:5096/BookAppointment/SuccessBooking",
                VnPayCommand = "pay"
            };

            VnPayId = vnPayModel.OrderId;

            return _vnPayService.CreatePaymentUrl(HttpContext, vnPayModel);
        }

        private async Task ProcessBookingAndTransactionAsync()
        {
            try
            {
                var tempData = HttpContext.Session.GetString("appointment");
                AppointmentBookRequestDto = JsonSerializer.Deserialize<AppointmentBookRequestDto>(tempData);

                if (AppointmentBookRequestDto != null)
                {
                    SelectedPets = await LoadSelectedPetListAsync(AppointmentBookRequestDto.PetIdList);
                    SelectedVet = await _userService.GetVetByIdAsync(AppointmentBookRequestDto.VetId);
                    var quantity = AppointmentBookRequestDto.PetIdList.Count;
                    TransactionServices = CreateTransactionServices(AppointmentBookRequestDto.ServiceIdList, quantity);
                }

                var userId = int.Parse(HttpContext.Session.GetString("UserId"));
                var bookAppointment = await _appointmentService.BookAppointmentAsync(AppointmentBookRequestDto, userId);

                var transactionRequest = new TransactionRequestDto
                {
                    AppointmentId = bookAppointment.Id,
                    PaymentMethod = PaymentMethodInput,
                    PaymentDate = DateTimeOffset.Now,
                    Status = 1,
                    Services = CreateTransactionServices(AppointmentBookRequestDto.ServiceIdList, 1)
                };

                await _transactionService.CreateTransactionAsync(transactionRequest, AppointmentBookRequestDto.CustomerId);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Failed to save appointment: " + ex.Message;
            }
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

        public IActionResult PaymentFail()
        {
            return Page();
        }

        public IActionResult PaymentSuccess()
        {
            return RedirectToPage("SuccessBooking");
        }

        public async Task<IActionResult> PaymentCallBackAsync()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            if (response == null || response.VnPayResponseCode != "00")
            {
                TempData["Message"] = $"Payment Fail: {response.VnPayResponseCode}";
                return RedirectToAction("PaymentFail");
            }

            try
            {
                var bookAppointment = await _appointmentService.BookAppointmentAsync(AppointmentBookRequestDto, AppointmentBookRequestDto.CustomerId);

                var transactionRequest = new TransactionRequestDto
                {
                    AppointmentId = bookAppointment.Id,
                    PaymentMethod = PaymentMethodInput,
                    PaymentDate = DateTimeOffset.Now,
                    PaymentId = response.OrderId,
                    Status = 2,
                    Services = CreateTransactionServices(AppointmentBookRequestDto.ServiceIdList, 1)
                };

                await _transactionService.CreateTransactionAsync(transactionRequest, AppointmentBookRequestDto.CustomerId);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Failed to save appointment: " + ex.Message;
            }

            TempData["Message"] = "Payment Success!";
            return RedirectToAction("PaymentSuccess");
        }
    }
}
