using Utility.Enum;

namespace BusinessObject.DTO.Appointment;

public class AppointmentFilterDto
{
    public int? CustomerId { get; set; }
    public int? PetId { get; set; }
    public int? VetId { get; set; }
    public int? TimeTableId { get; set; }
    public DateOnly? AppointmentDate { get; set; }
    public DateOnly? FromDate { get; set; }
    public DateOnly? ToDate { get; set; }
    public AppointmentStatus? Status { get; set; }
    public AppointmentBookingType? BookingType { get; set; }
    public short? Rating { get; set; }
    public bool? IsDecreasingByCreatedTime { get; set; }
}