using BusinessObject.DTO.Appointment;
using BusinessObject.DTO.TimeTable;
using BusinessObject.DTO.User;
using BusinessObject.DTO.Vet;
using BusinessObject.Entities;
using Repository.Extensions;

namespace Service.IServices;

public interface IAppointmentService
{
    /*Task<List<AppointmentResponseDto>> GetAllAppointmentsAsync();
    Task CreateAppointmentAsync(AppointmentRequestDto appointment);
    Task UpdateAppointmentAsync(AppointmentUpdateRequestDto appointment);
    Task DeleteAppointmentAsync(int id);*/
    Task<List<TimeTableResponseDto>> GetAllTimeFramesForBookingAsync();
    Task<List<UserResponseDto>> GetFreeWithTimeFrameAndDateAsync(AppointmentDateTimeQueryDto qo);
    Task<AppointmentResponseDto> BookOnlineAppointmentAsync(AppointmentBookRequestDto appointmentBookRequestDto, int ownerId);
    Task<PaginatedList<AppointmentResponseDto>> GetAllAppointmentsAsync(int pageNumber, int pageSize);
    Task<PaginatedList<AppointmentResponseDto>> GetVetAppointmentsAsync(int vetId, string date, int pageNumber, int pageSize);
    Task<PaginatedList<AppointmentResponseDto>> GetUserAppointmentsAsync(int pageNumber, int pageSize, int id, string date);
}