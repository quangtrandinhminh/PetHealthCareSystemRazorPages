using Azure;
using BusinessObject.DTO.Appointment;
using BusinessObject.DTO.Pet;
using BusinessObject.DTO.Service;
using BusinessObject.DTO.Transaction;
using BusinessObject.DTO.User;
using BusinessObject.DTO.VNPay;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        private readonly IVnPayService _vpnPayService;

        public TransactionFormModel(IPetService petService, IAppointmentService appointmentService, IService service, ITransactionService transactionService, IUserService userService, IVnPayService vpnPayService)
        {
            _petService = petService;
            _appointmentService = appointmentService;
            _service = service;
            _transactionService = transactionService;
            _userService = userService;
            _vpnPayService = vpnPayService;
        }

        public AppointmentBookRequestDto AppointmentBookRequestDto { get; set; }
        public List<PetResponseDto> SelectedPets { get; set; }
        public UserResponseDto SelectedVet { get; set; }
        public TransactionDropdownDto TransactionDropdownDto { get; set; }
        public List<TransactionServicesDto> TransactionServices { get; set; }
        public decimal Total {  get; set; }
        public int PaymentMethodInput { get; set; }
        public int VnPayId { get; set; }

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

        public async Task<IActionResult> OnPost(int paymentMethod, string appointmentResponseDtoJson)
        {
            PaymentMethodInput = paymentMethod;

            AppointmentBookRequestDto = JsonSerializer.Deserialize<AppointmentBookRequestDto>(appointmentResponseDtoJson);
            TransactionDropdownDto = _transactionService.GetTransactionDropdownData();

            var tempData = HttpContext.Session.GetString("appointment");

            if (AppointmentBookRequestDto == null)
            {
                throw new Exception("AppointmentResponseDto is null.");
            }

            if (paymentMethod.Equals(PaymentMethod.VnPay))
            {
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
                    CreatedDate = DateTime.Now,
                    Description = "Appointment Payment",
                    FullName = customer.FullName,
                    OrderId = new Random().Next(10000000, 99999999),
                };

                VnPayId = vnPayModel.OrderId;

                return Redirect(_vpnPayService.CreatePaymentUrl(HttpContext, vnPayModel));

            }

            try
            {
                AppointmentBookRequestDto = JsonSerializer.Deserialize<AppointmentBookRequestDto>(tempData);
                TransactionDropdownDto = _transactionService.GetTransactionDropdownData();

                if (AppointmentBookRequestDto != null)
                {
                    SelectedPets = await LoadSelectedPetList(AppointmentBookRequestDto.PetIdList);
                    SelectedVet = await _userService.GetVetByIdAsync(AppointmentBookRequestDto.VetId);
                    var quantity = AppointmentBookRequestDto.PetIdList.Count;
                    TransactionServices = CreateTransactionServices(AppointmentBookRequestDto.ServiceIdList, quantity);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString(), ex);
            }

            

            var userId = int.Parse(HttpContext.Session.GetString("UserId"));

            try
            {
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
                TempData["Message"] = "Fail while save appointment: " + ex.Message;
            }

            TempData["Message"] = "Payment Success!";
            return RedirectToAction("PaymentSuccess");
        }

        private async Task InitializeData()
        {
            var tempData = HttpContext.Session.GetString("appointment");

            try
            {
                AppointmentBookRequestDto = JsonSerializer.Deserialize<AppointmentBookRequestDto>(tempData);
                TransactionDropdownDto = _transactionService.GetTransactionDropdownData();

                if (AppointmentBookRequestDto != null)
                {
                    SelectedPets = await LoadSelectedPetList(AppointmentBookRequestDto.PetIdList);
                    SelectedVet = await _userService.GetVetByIdAsync(AppointmentBookRequestDto.VetId);
                    var quantity = AppointmentBookRequestDto.PetIdList.Count;
                    TransactionServices = CreateTransactionServices(AppointmentBookRequestDto.ServiceIdList, quantity);
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

        public async Task<IActionResult> PaymentCallBack()
        {
            var response = _vpnPayService.PaymentExecute(Request.Query);

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
                TempData["Message"] = "Fail while save appointment: " + ex.Message;
            }

            TempData["Message"] = "Payment Success!";
            return RedirectToAction("PaymentSuccess");
        }
    }
}