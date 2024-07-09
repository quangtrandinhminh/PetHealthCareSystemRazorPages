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
    public int Id { get; set; }
    public TimeTableResponseDto TimeTable { get; set; }
    public DateOnly AppointmentDate { get; set; }
    public string? Note { get; set; }
    public string BookingType { get; set; }
    public short? Rating { get; set; }
    public string? Feedback { get; set; }
    public int VetId { get; set; }
    public int CustomerId { get; set; }
    public UserResponseDto Customer { get; set; }
    public UserResponseDto Vet { get; set; }
    public List<PetResponseDto?> Pets { get; set; }
    public List<ServiceResponseDto?> Services { get; set; }
    public string Status { get; set; }
}