using BusinessObject;
using Repository.Base;

namespace Repository.Interfaces;

public interface IAppointmentPetRepository
{
    Task CreateAsync(AppointmentPet appointmentPet);
}