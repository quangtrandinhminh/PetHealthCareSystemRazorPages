using Azure;
using BusinessObject;
using BusinessObject.DTO;
using BusinessObject.DTO.Appointment;
using BusinessObject.DTO.Pet;
using BusinessObject.DTO.TimeTable;
using BusinessObject.DTO.Transaction;
using BusinessObject.DTO.User;
using BusinessObject.DTO.Vet;
using BusinessObject.Entities;
using BusinessObject.Mapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
using Utility.Helpers;

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

        var timetables = _timeTableRepo.GetAllWithCondition(t => t.Type == TimeTableType.Appointment);
        var response = _mapper.Map(timetables);

        return await response.ToListAsync();
    }

    public async Task<List<UserResponseDto>> GetFreeWithTimeFrameAndDateAsync(DateTimeQueryDto qo)
    {
        _logger.Information("Get free vet with time frame and date {@qo}", qo);
        if (!DateOnly.TryParse(qo.Date, out DateOnly date))
        {
            throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsCommon.DATE_WRONG_FORMAT);
        }

        if (qo.Date == null)
        {
            throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsCommon.DATA_NOT_ENOUGH);
        }

        _logger.Information("Get free vet with time frame and date");

        var vetList = await _userService.GetAllUsersByRoleAsync(UserRole.Vet);

        var appointmentList = _appointmentRepo.GetAllWithCondition(e => e.AppointmentDate == date 
                                                                        && e.TimeTableId == qo.TimetableId 
                                                                        && e.Status != AppointmentStatus.Cancelled);

        var freeVetList = vetList.Where(e => !appointmentList.Any(ee => ee.VetId == e.Id)).ToList();

        return freeVetList;
    }

    public async Task<AppointmentResponseDto> GetAppointmentByAppointmentId(int appointmentId)
    {
        _logger.Information($"Get appointment by appointment id {appointmentId}");
        var appointment = await _appointmentRepo.GetSingleAsync(a => a.Id == appointmentId,
            false, a => a.AppointmentPets, a => a.Services,
            a => a.TimeTable);
        if (appointment == null)
        {
            throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsAppointment.APPOINTMENT_NOT_FOUND);
        }

        var pets = new List<Pet>();
        foreach (var ap in appointment.AppointmentPets)
        {
            var pet = await _petRepository.GetSingleAsync(p => p.Id == ap.PetId);
            pets.Add(pet);
        }
        var vet = await _userRepository.GetSingleAsync(u => u.Id == appointment.VetId);
        var owner = await _userRepository.GetSingleAsync(u => u.Id == pets[0].OwnerID);

        var response = _mapper.Map(appointment);
        response.Pets = _mapper.Map(pets);
        foreach (var pet in response.Pets)
        {
            if (pet != null) pet.OwnerName = owner?.FullName;
        }

        if (vet != null)
        {
            response.Vet = _mapper.UserToUserResponseDto(vet);
        }

        if (owner != null)
        {
            response.Customer = _mapper.UserToUserResponseDto(owner);
        }

        return response;
    }

    public async Task<PaginatedList<AppointmentResponseDto>> GetAllAppointmentsAsync(int pageNumber, int pageSize)
    {
        _logger.Information("Get all appointments");

        var appointments = _appointmentRepo.GetAllWithCondition(a => a.DeletedTime == null, a => a.AppointmentPets);
        var response = _mapper.Map(appointments);
        var paginatedList = await PaginatedList<AppointmentResponseDto>.CreateAsync(response, pageNumber, pageSize);
        foreach (var item in paginatedList.Items)
        {
            var vet = await _userRepository.GetSingleAsync(u => u.Id == item.VetId);
            if (vet != null)
            {
                item.Vet = _mapper.UserToUserResponseDto(vet);
            }

            var customer = await _userRepository.GetSingleAsync(u => u.Id == item.CustomerId);
            if (customer != null)
            {
                item.Customer = _mapper.UserToUserResponseDto(customer);
            }

            var appointmentPet = (await appointments.FirstOrDefaultAsync(x => x.Id == item.Id))?.AppointmentPets!;
            var pets = new List<Pet>();
            foreach (var apoPet in appointmentPet)
            {
                var pet = await _petRepository.GetSingleAsync(p => p.Id == apoPet.PetId);
                
                pets.Add(pet);
            }
            item.Pets = _mapper.Map(pets);
            foreach (var pet in item.Pets)
            {
                if (pet != null) pet.OwnerName = customer?.FullName;
            }
        }
        return paginatedList;
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

        IQueryable<Appointment> appointments = new List<Appointment>().AsQueryable();
        if (date != DateOnly.MinValue)
        {
            appointments = _appointmentRepo.GetAllWithCondition(a => a.DeletedTime == null && a.VetId == vetId && a.AppointmentDate == date, 
                a => a.AppointmentPets);
        }
        else
        {
            appointments = _appointmentRepo.GetAllWithCondition(a => a.DeletedTime == null && a.VetId == vetId,
                a => a.AppointmentPets);
        }

        var response = _mapper.Map(appointments);
        var paginatedList = await PaginatedList<AppointmentResponseDto>.CreateAsync(response, pageNumber, pageSize);
        foreach (var item in paginatedList.Items)
        {
            var vet = await _userRepository.GetSingleAsync(u => u.Id == item.VetId);
            if (vet != null)
            {
                item.Vet = _mapper.UserToUserResponseDto(vet);
            }

            var customer = await _userRepository.GetSingleAsync(u => u.Id == item.CustomerId);
            if (customer != null)
            {
                item.Customer = _mapper.UserToUserResponseDto(customer);
            }

            var appointmentPet = (await appointments.FirstOrDefaultAsync(x => x.Id == item.Id))?.AppointmentPets!;
            var pets = new List<Pet>();
            foreach (var apoPet in appointmentPet)
            {
                var pet = await _petRepository.GetSingleAsync(p => p.Id == apoPet.PetId);

                pets.Add(pet);
            }
            item.Pets = _mapper.Map(pets);
            foreach (var pet in item.Pets)
            {
                if (pet != null) pet.OwnerName = customer?.FullName;
            }
        }
        return paginatedList;
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

        IQueryable<Appointment> appointments = new List<Appointment>().AsQueryable();
        if (date != DateOnly.MinValue)
        {
            appointments = _appointmentRepo.GetAllWithCondition(a => a.DeletedTime == null && a.CustomerId == ownerId && a.AppointmentDate == date,
                a => a.AppointmentPets);
        }
        else
        {
            appointments = _appointmentRepo.GetAllWithCondition(a => a.DeletedTime == null && a.CustomerId == ownerId,
                a => a.AppointmentPets);
        }

        var response = _mapper.Map(appointments);

        var paginatedList = await PaginatedList<AppointmentResponseDto>.CreateAsync(response, pageNumber, pageSize);
        foreach (var item in paginatedList.Items)
        {
            var vet = await _userRepository.GetSingleAsync(u => u.Id == item.VetId);
            if (vet != null)
            {
                item.Vet = _mapper.UserToUserResponseDto(vet);
            }

            var customer = await _userRepository.GetSingleAsync(u => u.Id == item.CustomerId);
            if (customer != null)
            {
                item.Customer = _mapper.UserToUserResponseDto(customer);
            }

            var appointmentPet = (await appointments.FirstOrDefaultAsync(x => x.Id == item.Id))?.AppointmentPets!;
            var pets = new List<Pet>();
            foreach (var apoPet in appointmentPet)
            {
                var pet = await _petRepository.GetSingleAsync(p => p.Id == apoPet.PetId);

                pets.Add(pet);
            }
            item.Pets = _mapper.Map(pets);
            foreach (var pet in item.Pets)
            {
                if (pet != null) pet.OwnerName = customer?.FullName;
            }
        }
        return paginatedList;
    }

    public async Task<PaginatedList<AppointmentResponseDto>> GetAppointmentWithFilter(AppointmentFilterDto filter, int pageNumber, int pageSize)
    {
        _logger.Information("Get all appointments with filter {@filter}", filter);

        var appointments = _appointmentRepo.GetAll();
        appointments = appointments.Where(e => e.DeletedTime == null);

        if (filter.CustomerId != null)
        {
            appointments = appointments.Where(e => e.AppointmentPets.ElementAt(0).Pet.OwnerID == filter.CustomerId);
        }

        if (filter.PetId != null)
        {
            appointments = appointments.Where(e => e.AppointmentPets.Any(ap => ap.PetId == filter.PetId));
        }

        if (filter.VetId != null)
        {
            appointments = appointments.Where(e => e.VetId == filter.VetId);
        }

        if (filter.TimeTableId != null)
        {
            appointments = appointments.Where(e => e.TimeTableId == filter.TimeTableId);
        }

        if (filter.AppointmentDate != null)
        {
            appointments = appointments.Where(e => e.AppointmentDate == filter.AppointmentDate);
        }
        else
        {
            if (filter.FromDate != null)
            {
                appointments = appointments.Where(e => e.AppointmentDate >= filter.FromDate);
            }

            if (filter.ToDate != null)
            {
                appointments = appointments.Where(e => e.AppointmentDate <= filter.ToDate);
            }
        }

        if (filter.Status != null)
        {
            appointments = appointments.Where(e => e.Status == filter.Status);
        }

        if (filter.BookingType != null)
        {
            appointments = appointments.Where(e => e.BookingType == filter.BookingType);
        }

        if (filter.Rating != null)
        {
            appointments = appointments.Where(e => e.Rating == filter.Rating);
        }

        if (filter.IsDecreasingByCreatedTime != null && filter.IsDecreasingByCreatedTime == true)
        {
            appointments = appointments.OrderByDescending(e => e.CreatedTime);
        }
        else
        {
            appointments = appointments.OrderBy(e => e.CreatedTime);
        }

        var response = _mapper.Map(appointments);

        foreach (var item in response)
        {
            var vet = await _userRepository.GetSingleAsync(u => u.Id == item.VetId);
            if (vet != null)
            {
                item.Vet = _mapper.UserToUserResponseDto(vet);
            }

            var customer = await _userRepository.GetSingleAsync(u => u.Id == item.CustomerId);
            if (customer != null)
            {
                item.Customer = _mapper.UserToUserResponseDto(customer);
            }
        }

        var paginatedList = await PaginatedList<AppointmentResponseDto>.CreateAsync(response, pageNumber, pageSize);
        return paginatedList;
    }

    public async Task<AppointmentResponseDto> BookAppointmentAsync(AppointmentBookRequestDto appointmentBookRequestDto, int createdById)
    {
        _logger.Information("Book online appointment {@appointmentBookRequestDto} by user id {@createdById}", appointmentBookRequestDto, createdById);

        // Check pet list is null or empty
        if (appointmentBookRequestDto.PetIdList.Count == 0)
        {
            throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsCommon.DATA_NOT_ENOUGH);
        }
        else
        {
            foreach (var i in appointmentBookRequestDto.PetIdList)
            {
                var pet = await _petRepository.GetByIdAsync(i);

                if (pet == null || (pet != null && pet.OwnerID != appointmentBookRequestDto.CustomerId))
                {
                    throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsPet.NOT_YOUR_PET);
                }
            }
        }

        // Check vet 
        var vet = await _userService.GetVetByIdAsync(appointmentBookRequestDto.VetId);

        if (vet == null)
        {
            throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsUser.VET_NOT_FOUND);
        }

        // Check date format
        if (!DateOnly.TryParse(appointmentBookRequestDto.AppointmentDate, out DateOnly date))
        {
            throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsCommon.DATE_WRONG_FORMAT);
        }

        // Check timetable
        var existTimetable = await _timeTableRepo.GetByIdAsync(appointmentBookRequestDto.TimeTableId);

        if (existTimetable == null)
        {
            throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsTimetable.TIMETABLE_NOT_FOUND);
        }

        // Check can book
        var canBook = await _appointmentRepo.GetSingleAsync(e => e.AppointmentDate == date && e.TimeTableId == appointmentBookRequestDto.TimeTableId && e.VetId == appointmentBookRequestDto.VetId);
        if (canBook != null)
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

        var appointment = _mapper.Map(appointmentBookRequestDto);
        appointment.Services = services;
        appointment.AppointmentDate = date;
        appointment.CreatedBy = appointment.LastUpdatedBy = createdById;
        appointment.BookingType = appointment.CustomerId == appointment.CreatedBy 
            ? AppointmentBookingType.Online : AppointmentBookingType.WalkIn;


        var addedAppointment = await _appointmentRepo.AddAppointmentAsync(appointment);

        // Add to AppointmentPet tables
        foreach (var i in appointmentBookRequestDto.PetIdList)
        {
            await _appointmentPetRepo.CreateAsync(new AppointmentPet
            {
                PetId = i,
                AppointmentId = addedAppointment.Id,
            });
        }

        return await GetAppointmentByAppointmentId(addedAppointment.Id);
    }
    public async Task<AppointmentResponseDto> UpdateStatusToDone(int appointmentId, int vetId)
    {
        _logger.Information($"Update status to done for appointment {appointmentId} by vet id {vetId}");

        var appointment = await _appointmentRepo.GetSingleAsync(a => a.Id == appointmentId);

        if (appointment == null)
        {
            throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsAppointment.APPOINTMENT_NOT_FOUND);
        }

        appointment.Status = AppointmentStatus.Completed;
        appointment.LastUpdatedBy = vetId;
        appointment.LastUpdatedTime = CoreHelper.SystemTimeNow;

        await _appointmentRepo.UpdateAsync(appointment);

        return await GetAppointmentByAppointmentId(appointmentId);
    }

    public async Task<AppointmentResponseDto> UpdateStatusToCancel(int appointmentId, int updatedById)
    {
        _logger.Information($"Update status to cancel for appointment {appointmentId} by user id {updatedById}");

        var appointment = await _appointmentRepo.GetSingleAsync(a => a.Id == appointmentId);

        if (appointment == null)
        {
            throw new AppException(ResponseCodeConstants.FAILED, ResponseMessageConstantsAppointment.APPOINTMENT_NOT_FOUND);
        }

        appointment.Status = AppointmentStatus.Cancelled;
        appointment.LastUpdatedBy = updatedById;
        appointment.LastUpdatedTime = CoreHelper.SystemTimeNow;

        await _appointmentRepo.UpdateAsync(appointment);

        return await GetAppointmentByAppointmentId(appointmentId);
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