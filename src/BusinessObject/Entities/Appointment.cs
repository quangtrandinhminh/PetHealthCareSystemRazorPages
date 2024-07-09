using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Entities.Base;
using BusinessObject.Entities.Identity;
using Utility.Enum;

namespace BusinessObject.Entities;

[Table("Appointment")]
public class Appointment : BaseEntity
{
    public Appointment()
    {
        Status = AppointmentStatus.Scheduled;
    }
    public int CustomerId { get; set; }
    public int TimeTableId { get; set; }
    public DateOnly AppointmentDate { get; set; }
    public string? Note { get; set; }
    public AppointmentStatus Status { get; set; }
    public AppointmentBookingType BookingType { get; set; }
    public short? Rating { get; set; }
    public string? Feedback { get; set; }
    public int VetId { get; set; }

    [ForeignKey(nameof(TimeTableId))]
    public virtual TimeTable TimeTable { get; set; }

    public virtual ICollection<AppointmentPet> AppointmentPets { get; set; }

    public virtual ICollection<Service> Services { get; set; }
}

