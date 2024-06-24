using BusinessObject.DTO.Pet;
using BusinessObject.DTO.Service;
using BusinessObject.DTO.TimeTable;
using BusinessObject.DTO.User;
using BusinessObject.DTO.Vet;
using BusinessObject.Entities.Identity;
using Utility.Enum;

namespace BusinessObject.DTO.Appointment;

public class AppointmentResponseDto
{
    public TimeTableResponseDto TimeTable { get; set; }
    public DateOnly AppointmentDate { get; set; }
    public string? Note { get; set; }
    public AppointmentBookingType BookingType { get; set; }
    public short? Rating { get; set; }
    public string? Feedback { get; set; }
    public UserResponseDto Vet { get; set; }
    public List<PetResponseDto?> Pets { get; set; }
    public List<ServiceResponseDto?> Services { get; set; }
}