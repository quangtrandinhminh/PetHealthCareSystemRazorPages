using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Entities;

namespace BusinessObject;

public class AppointmentPet
{
    public int AppointmentId { get; set; }

    [ForeignKey(nameof(AppointmentId))]
    public virtual Appointment Appointment { get; set; }

    public int PetId { get; set; }

    [ForeignKey(nameof(PetId))]
    public virtual Pet Pet { get; set; }
}