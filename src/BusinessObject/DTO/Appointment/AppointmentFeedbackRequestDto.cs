using System.ComponentModel.DataAnnotations;

namespace BusinessObject.DTO.Appointment;

public class AppointmentFeedbackRequestDto
{
    [Range(1,5)]
    public short Rating { get; set; }
    [Required, MinLength(2)]
    public string Feedback { get; set; }
    public int AppointmentId { get; set; }
}