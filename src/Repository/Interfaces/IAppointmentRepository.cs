using BusinessObject.Entities;
using Repository.Base;

namespace Repository.Interfaces;

public interface IAppointmentRepository : IBaseRepository<Appointment>
{
    Task<Appointment> AddAppointmentAsync(Appointment appointment);
    Task<List<Appointment>> GetAppointmentsAsync();
}