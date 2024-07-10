using Utility.Enum;

namespace BusinessObject.DTO.Appointment;

public class AppointmentBookRequestDto
{
    public List<int> ServiceIdList { get; set; }
    public int VetId { get; set; }
    public int CustomerId { get; set; }
    public string? Note { get; set; }
    public int TimeTableId { get; set; }
    public string AppointmentDate { get; set; }
    public List<int> PetIdList { get; set; }
}