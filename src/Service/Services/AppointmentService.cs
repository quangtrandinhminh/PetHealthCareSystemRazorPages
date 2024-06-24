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
    private readonly IPetRepository _petRepository =
        serviceProvider.GetRequiredService<IPetRepository>();
    private readonly IAppointmentPetRepository _appointmentPetRepo =
        serviceProvider.GetRequiredService<IAppointmentPetRepository>();
    private readonly IUserService _userService = serviceProvider.GetRequiredService<IUserService>();
    private readonly MapperlyMapper _mapper = serviceProvider.GetRequiredService<MapperlyMapper>();
    private readonly ILogger _logger = Log.Logger;

    public async Task<List<TimeTableResponseDto>> GetAllTimeFramesForBookingAsync()
    {
        try
        {
            _logger.Information("Get all time frames for booking");

            var timetables = await _timeTableRepo.GetAllBookingTimeFramesAsync();
            var response = _mapper.Map(timetables);

            return response.ToList();
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            throw new AppException(ResponseCodeConstants.INTERNAL_SERVER_ERROR,
                ResponseMessageConstantsCommon.SERVER_ERROR);
        }
    }

    public async Task<List<UserResponseDto>> GetFreeWithTimeFrameAndDateAsync(DateOnly date, int timetableId)
    {
        try
        {
            _logger.Information("Get free vet with time frame and date");

            var vetList = (await _userService.GetVetsAsync()).ToList();

            var appointmentList = (await _appointmentRepo.GetAllAsync()).Where(e => e.AppointmentDate == date && e.TimeTableId == timetableId);

            var freeVetList = vetList.Where(e => !appointmentList.Any(ee => ee.VetId == e.Id)).ToList();

            return freeVetList;
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            throw new AppException(ResponseCodeConstants.INTERNAL_SERVER_ERROR,
                ResponseMessageConstantsCommon.SERVER_ERROR);
        }
    }

    public async Task BookOnlineAppointmentAsync(AppointmentBookRequestDto appointmentBookRequestDto)
    {
        try
        {
            _logger.Information("Book online appointment");

            Appointment appointment = new();

            var services =
                appointmentBookRequestDto.ServiceIdList.Select(e => new BusinessObject.Entities.Service
                {
                    Id = e,
                }).ToList();

            if (!DateOnly.TryParse(appointmentBookRequestDto.AppointmentDate, out DateOnly date))
            {
                throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsCommon.DATE_WRONG_FORMAT);
            }

            var canBook = !(await _appointmentRepo.GetAllAsync()).Any(e => e.AppointmentDate == date && e.TimeTableId == appointmentBookRequestDto.TimetableId && e.VetId == appointmentBookRequestDto.VetId);

            if (!canBook)
            {
                throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsAppointment.APPOINTMENT_EXISTED);
            }

            appointment.Services = services;
            appointment.VetId = appointmentBookRequestDto.VetId;
            appointment.Note = appointmentBookRequestDto.Note;
            appointment.TimeTableId = appointmentBookRequestDto.TimetableId;
            appointment.AppointmentDate = date;
            appointment.BookingType = AppointmentBookingType.Online;

            await _appointmentRepo.AddAppointmentAsync(appointment);

            foreach (var i in appointmentBookRequestDto.PetIdList)
            {
                await _appointmentPetRepo.CreateAsync(new AppointmentPet()
                {
                    PetId = i,
                    AppointmentId = appointment.Id,
                });
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            throw new AppException(ResponseCodeConstants.INTERNAL_SERVER_ERROR,
                ResponseMessageConstantsCommon.SERVER_ERROR);
        }
    }

    public async Task<PaginatedList<AppointmentResponseDto>> GetAllAppointmentsAsync(int pageNumber, int pageSize)
    {
        try
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
            var appointmentDtos = await Task.WhenAll(paginatedAppointments.Select(async e =>
            {
                var vet = vets.FirstOrDefault(ee => ee.Id == e.VetId);

                var pets = e.AppointmentPets.Select(ap => _petRepository.GetById(ap.PetId)).ToList();

                var timeTable = (await _timeTableRepo.FindByConditionAsync(tt => tt.Id == e.TimeTableId)).FirstOrDefault(); // Adjust this line if `TimeTableId` is not the correct property name

                return new AppointmentResponseDto()
                {
                    AppointmentDate = e.AppointmentDate,
                    Note = e.Note,
                    BookingType = e.BookingType,
                    Vet = vet,
                    Feedback = e.Feedback,
                    Pets = _mapper.Map(pets),
                    Rating = e.Rating,
                    TimeTable = new TimeTableResponseDto()
                    {
                        Id = timeTable.Id,
                        StartTime = timeTable.StartTime, // Assuming `StartTime` is a property in your `TimeTableResponseDto`
                        EndTime = timeTable.EndTime // Assuming `EndTime` is another property you need
                    },
                    Services = _mapper.Map(e.Services),
                };
            }));

            return new PaginatedList<AppointmentResponseDto>(appointmentDtos, totalAppointments, pageNumber, pageSize);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            throw new AppException(ResponseCodeConstants.INTERNAL_SERVER_ERROR,
                ResponseMessageConstantsCommon.SERVER_ERROR);
        }
    }

    public async Task<PaginatedList<AppointmentResponseDto>> GetVetAppointmentsAsync(int pageNumber, int pageSize, int vetId)
    {
        try
        {
            _logger.Information("Get all appointments");

            var appointments = (await _appointmentRepo.GetAppointmentsAsync()).Where(e => e.VetId == vetId);

            var totalAppointments = appointments.Count();

            // Apply pagination
            var paginatedAppointments = appointments
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var vets = await _userService.GetVetsAsync();

            // Fetch related data asynchronously for paginated appointments
            var appointmentDtos = await Task.WhenAll(paginatedAppointments.Select(async e =>
            {
                var vet = vets.FirstOrDefault(ee => ee.Id == e.VetId);

                var pets = e.AppointmentPets.Select(ap => _petRepository.GetById(ap.PetId)).ToList();

                var timeTable = (await _timeTableRepo.FindByConditionAsync(tt => tt.Id == e.TimeTableId)).FirstOrDefault(); // Adjust this line if `TimeTableId` is not the correct property name

                return new AppointmentResponseDto()
                {
                    AppointmentDate = e.AppointmentDate,
                    Note = e.Note,
                    BookingType = e.BookingType,
                    Vet = vet,
                    Feedback = e.Feedback,
                    Pets = _mapper.Map(pets),
                    Rating = e.Rating,
                    TimeTable = new TimeTableResponseDto()
                    {
                        Id = timeTable.Id,
                        StartTime = timeTable.StartTime, // Assuming `StartTime` is a property in your `TimeTableResponseDto`
                        EndTime = timeTable.EndTime // Assuming `EndTime` is another property you need
                    },
                    Services = _mapper.Map(e.Services),
                };
            }));

            return new PaginatedList<AppointmentResponseDto>(appointmentDtos, totalAppointments, pageNumber, pageSize);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            throw new AppException(ResponseCodeConstants.INTERNAL_SERVER_ERROR,
                ResponseMessageConstantsCommon.SERVER_ERROR);
        }
    }

    public async Task<PaginatedList<AppointmentResponseDto>> GetUserAppointmentsAsync(int pageNumber, int pageSize,
        int ownerId)
    {
        try
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
            var appointmentDtos = await Task.WhenAll(paginatedAppointments.Select(async e =>
            {
                var vet = vets.FirstOrDefault(ee => ee.Id == e.VetId);

                var pets = e.AppointmentPets.Select(ap => _petRepository.GetById(ap.PetId)).ToList();

                var timeTable = (await _timeTableRepo.FindByConditionAsync(tt => tt.Id == e.TimeTableId)).FirstOrDefault(); // Adjust this line if `TimeTableId` is not the correct property name

                return new AppointmentResponseDto()
                {
                    AppointmentDate = e.AppointmentDate,
                    Note = e.Note,
                    BookingType = e.BookingType,
                    Vet = vet,
                    Feedback = e.Feedback,
                    Pets = _mapper.Map(pets),
                    Rating = e.Rating,
                    TimeTable = new TimeTableResponseDto()
                    {
                        Id = timeTable.Id,
                        StartTime = timeTable.StartTime, // Assuming `StartTime` is a property in your `TimeTableResponseDto`
                        EndTime = timeTable.EndTime // Assuming `EndTime` is another property you need
                    },
                    Services = _mapper.Map(e.Services),
                };
            }));

            return new PaginatedList<AppointmentResponseDto>(appointmentDtos, totalAppointments, pageNumber, pageSize);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
            throw new AppException(ResponseCodeConstants.INTERNAL_SERVER_ERROR,
                ResponseMessageConstantsCommon.SERVER_ERROR);
        }
    }
}