using BusinessObject.Entities;
using DataAccessLayer.Base;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.DAO;

public class AppointmentDao : BaseDao<Appointment>
{
    private static readonly AppDbContext _context = new();

    public async Task DeleteAppointmentAsync(int appointmentId)
    {
        var appointment = await _context.Appointments
            .Include(a => a.AppointmentPets)
            .FirstOrDefaultAsync(a => a.Id == appointmentId);

        if (appointment != null)
        {
            _context.AppointmentPets.RemoveRange(appointment.AppointmentPets);
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
        }
    }
}