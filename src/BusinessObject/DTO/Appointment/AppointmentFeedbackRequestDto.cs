using System.ComponentModel.DataAnnotations;

namespace BusinessObject.DTO.Appointment;

public class AppointmentFeedbackRequestDto
{
    [Range(0,5)]
    public short Rating { get; set; }
    public string Feedback { get; set; }
    public int AppointmentId { get; set; }
}