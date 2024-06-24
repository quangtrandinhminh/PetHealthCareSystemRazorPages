using BusinessObject;
using DataAccessLayer.DAO;
using Repository.Base;
using Repository.Interfaces;

namespace Repository.Repositories;

public class AppointmentPetRepository : IAppointmentPetRepository
{
    public async Task CreateAsync(AppointmentPet appointmentPet)
    {
        await AppointmentPetDao.AddAsync(appointmentPet);
    }
}