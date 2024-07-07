using Azure;
using BusinessObject;
using BusinessObject.DTO;
using BusinessObject.DTO.Appointment;
using BusinessObject.DTO.TimeTable;
using BusinessObject.DTO.Transaction;
using BusinessObject.DTO.User;
using BusinessObject.DTO.Vet;
using BusinessObject.Entities;
using BusinessObject.Mapper;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using Repository.Extensions;
using Repository.Interfaces;
using Serilog;
using Service.IServices;
using Utility.Constants;
using Utility.Enum;
using Utility.Exceptions;

namespace Service.Services;

public class AppointmentService(IServiceProvider serviceProvider) : IAppointmentService
{
    private readonly ITimeTableRepository _timeTableRepo =
        serviceProvider.GetRequiredService<ITimeTableRepository>();
    private readonly IAppointmentRepository _appointmentRepo =
        serviceProvider.GetRequiredService<IAppointmentRepository>();
    private readonly IServiceRepository _serviceRepo =
        serviceProvider.GetRequiredService<IServiceRepository>();
    private readonly IUserRepository _userRepository =
        serviceProvider.GetRequiredService<IUserRepository>();
    private readonly IMedicalRecordRepository _medicalRecordRepository =
        serviceProvider.GetRequiredService<IMedicalRecordRepository>();
    private readonly IPetRepository _petRepository =
        serviceProvider.GetRequiredService<IPetRepository>();
    private readonly IAppointmentPetRepository _appointmentPetRepo =
        serviceProvider.GetRequiredService<IAppointmentPetRepository>();
    private readonly IUserService _userService = serviceProvider.GetRequiredService<IUserService>();
    private readonly MapperlyMapper _mapper = serviceProvider.GetRequiredService<MapperlyMapper>();
    private readonly ILogger _logger = Log.Logger;

    public async Task<List<TimeTableResponseDto>> GetAllTimeFramesForBookingAsync()
    {
        _logger.Information("Get all time frames for booking");

        var timetables = await _timeTableRepo.GetAllBookingTimeFramesAsync();
        var response = _mapper.Map(timetables);

        return response.ToList();
    }

    public async Task<List<UserResponseDto>> GetFreeWithTimeFrameAndDateAsync(AppointmentDateTimeQueryDto qo)
    {
        _logger.Information("Get free vet with time frame and date {@qo}", qo);
        if (!DateOnly.TryParse(qo.Date, out DateOnly date))
        {
            throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsCommon.DATE_WRONG_FORMAT);
        }

        if (qo.Date == null || qo.TimetableId == null)
        {
            throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsCommon.DATA_NOT_ENOUGH);
        }

        _logger.Information("Get free vet with time frame and date");

        var vetList = (await _userService.GetVetsAsync()).ToList();

        var appointmentList = (await _appointmentRepo.GetAllAsync()).Where(e => e.AppointmentDate == date && e.TimeTableId == qo.TimetableId);

        var freeVetList = vetList.Where(e => !appointmentList.Any(ee => ee.VetId == e.Id)).ToList();

        return freeVetList;
    }

    public async Task<AppointmentResponseDto> BookOnlineAppointmentAsync(AppointmentBookRequestDto appointmentBookRequestDto, int ownerId)
    {
        _logger.Information("Book online appointment {@appointmentBookRequestDto} by owner {@ownerId}", appointmentBookRequestDto, ownerId);

        // Check pet list is null or empty
        if (appointmentBookRequestDto.PetIdList == null || appointmentBookRequestDto.PetIdList.Count == 0)
        {
            throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsCommon.DATA_NOT_ENOUGH);
        }

        // Check vet 
        var vet = await _userService.GetVetByIdAsync(appointmentBookRequestDto.VetId);

        if (vet == null)
        {
            throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsVet.VET_NOT_FOUND);
        }

        // Check date format
        if (!DateOnly.TryParse(appointmentBookRequestDto.AppointmentDate, out DateOnly date))
        {
            throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsCommon.DATE_WRONG_FORMAT);
        }

        // Check timetable
        var existTimetable = await _timeTableRepo.GetByIdAsync(appointmentBookRequestDto.TimetableId);

        if (existTimetable == null)
        {
            throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsTimetable.NOT_FOUND);
        }

        // Check can book
        var canBook = !(await _appointmentRepo.GetAllAsync()).Any(e => e.AppointmentDate == date && e.TimeTableId == appointmentBookRequestDto.TimetableId && e.VetId == appointmentBookRequestDto.VetId);

        if (!canBook)
        {
            throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsAppointment.APPOINTMENT_EXISTED);
        }

        // Check existed service
        List<BusinessObject.Entities.Service> services = new();

        foreach (var i in appointmentBookRequestDto.ServiceIdList)
        {
            var existService = await _serviceRepo.GetByIdAsync(i);

            if (existService == null)
            {
                throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsService.SERVICE_NOT_FOUND);
            }

            services.Add(new BusinessObject.Entities.Service
            {
                Id = i,
            });
        }

        Appointment appointment = new()
        {
            Services = services,
            VetId = appointmentBookRequestDto.VetId,
            Note = appointmentBookRequestDto.Note,
            TimeTableId = appointmentBookRequestDto.TimetableId,
            AppointmentDate = date,
            BookingType = AppointmentBookingType.Online,
            Status = AppointmentStatus.Scheduled
        };

        await _appointmentRepo.AddAppointmentAsync(appointment);

        // Add to AppointmentPet tables
        foreach (var i in appointmentBookRequestDto.PetIdList)
        {
            var pet = await _petRepository.GetByIdAsync(i);

            if (pet == null || (pet != null && pet.OwnerID != ownerId))
            {
                await _appointmentRepo.DeleteAsync(appointment);
                throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsPet.NOT_YOUR_PET);
            }

            await _appointmentPetRepo.CreateAsync(new AppointmentPet
            {
                PetId = i,
                AppointmentId = appointment.Id,
            });
        }

        var appointments = await _appointmentRepo.GetAppointmentsAsync();
        var returnAppointment = appointments.Where(e => e.Id == appointment.Id).FirstOrDefault();
        var vets = await _userService.GetVetsAsync();

        return await ToAppointmentResponseDto(appointments, vets, returnAppointment);
    }

    public async Task<PaginatedList<AppointmentResponseDto>> GetAllAppointmentsAsync(int pageNumber, int pageSize)
    {
        _logger.Information("Get all appointments");

        var appointments = await _appointmentRepo.GetAppointmentsAsync();

        var totalAppointments = appointments.Count();

        // Apply pagination
        var paginatedAppointments = appointments
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var vets = await _userService.GetVetsAsync();

        // Fetch related data asynchronously for paginated appointments
        var appointmentDtos = await Task.WhenAll(paginatedAppointments.Select(async e => await ToAppointmentResponseDto(appointments, vets, e)));

        return new PaginatedList<AppointmentResponseDto>(appointmentDtos, totalAppointments, pageNumber, pageSize);
    }

    public async Task<PaginatedList<AppointmentResponseDto>> GetVetAppointmentsAsync(int vetId, string dateString, int pageNumber, int pageSize)
    {
        _logger.Information($"Get all appointments for vet {vetId} on {dateString}");

        DateOnly date = DateOnly.MinValue;

        // Check date format
        if (!string.IsNullOrWhiteSpace(dateString) && !DateOnly.TryParse(dateString, out date))
        {
            throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsCommon.DATE_WRONG_FORMAT);
        }

        IEnumerable<Appointment> appointments;

        if (date != DateOnly.MinValue)
        {
            appointments = (await _appointmentRepo.GetAppointmentsAsync()).Where(e => e.VetId == vetId && e.AppointmentDate == date);
        }
        else
        {
            appointments = (await _appointmentRepo.GetAppointmentsAsync()).Where(e => e.VetId == vetId);
        }

        var totalAppointments = appointments.Count();

        // Apply pagination
        var paginatedAppointments = appointments
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var vets = await _userService.GetVetsAsync();

        // Fetch related data asynchronously for paginated appointments
        var appointmentDtos = await Task.WhenAll(paginatedAppointments.Select(async e => await ToAppointmentResponseDto(appointments, vets, e)));

        return new PaginatedList<AppointmentResponseDto>(appointmentDtos, totalAppointments, pageNumber, pageSize);
    }

    public async Task<PaginatedList<AppointmentResponseDto>> GetUserAppointmentsAsync(int pageNumber, int pageSize,
        int ownerId, string dateString)
    {
        _logger.Information($"Get all appointments for user {ownerId} on {dateString}");

        DateOnly date = DateOnly.MinValue;

        // Check date format
        if (!string.IsNullOrWhiteSpace(dateString) && !DateOnly.TryParse(dateString, out date))
        {
            throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsCommon.DATE_WRONG_FORMAT);
        }

        IEnumerable<Appointment> appointments;

        if (date != DateOnly.MinValue)
        {
            appointments = (await _appointmentRepo.GetAppointmentsAsync()).Where(e => e.AppointmentPets.ElementAt(0).Pet.OwnerID == ownerId && e.AppointmentDate == date);
        }
        else
        {
            appointments = (await _appointmentRepo.GetAppointmentsAsync()).Where(e => e.AppointmentPets.ElementAt(0).Pet.OwnerID == ownerId);
        }

        var totalAppointments = appointments.Count();

        // Apply pagination
        var paginatedAppointments = appointments
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var vets = await _userService.GetVetsAsync();

        // Fetch related data asynchronously for paginated appointments
        var appointmentDtos = await Task.WhenAll(paginatedAppointments.Select(async e => await ToAppointmentResponseDto(appointments, vets, e)));

        return new PaginatedList<AppointmentResponseDto>(appointmentDtos, totalAppointments, pageNumber, pageSize);
    }

    private async Task<AppointmentResponseDto> ToAppointmentResponseDto(IEnumerable<Appointment> appointments,
        IList<UserResponseDto> vets, Appointment e)
    {
        var vet = vets.FirstOrDefault(ee => ee.Id == e.VetId);

        var pets = e.AppointmentPets.Select(ap => ap.Pet).ToList();

        var ownerName = await _userRepository.GetFullnameAsyncs(pets[0].OwnerID);

        foreach (var pet in pets)
        {
            pet.Owner = new() { FullName = ownerName };
        }

        var petsDto = _mapper.Map(pets);

        foreach (var petResponseDto in petsDto)
        {
            var mr =
                await _medicalRecordRepository.GetSingleAsync(e =>
                    e.AppointmentId == e.Id && e.PetId == petResponseDto.Id);

            bool hasMedicalRecord = mr != null;

            petResponseDto.HasMedicalRecord = hasMedicalRecord;
        }

        var timeTable = (await _timeTableRepo.FindByConditionAsync(tt => tt.Id == e.TimeTableId)).FirstOrDefault(); // Adjust this line if `TimeTableId` is not the correct property name

        return new AppointmentResponseDto()
        {
            Id = e.Id,
            AppointmentDate = e.AppointmentDate,
            Note = e.Note,
            BookingType = e.BookingType.ToString(),
            Vet = vet,
            Feedback = e.Feedback,
            Pets = petsDto,
            Rating = e.Rating,
            TimeTable = new TimeTableResponseDto()
            {
                Id = timeTable.Id,
                StartTime = timeTable.StartTime, // Assuming `StartTime` is a property in your `TimeTableResponseDto`
                EndTime = timeTable.EndTime // Assuming `EndTime` is another property you need
            },
            Services = _mapper.Map(e.Services),
            Status = e.Status.ToString(),
        };
    }
}