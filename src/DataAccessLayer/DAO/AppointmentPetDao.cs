using BusinessObject;
using DataAccessLayer.Base;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.DAO;

public class AppointmentPetDao
{
    public static async Task AddAsync(AppointmentPet entity)
    {
        await using var context = new AppDbContext();
        var dbSet = context.Set<AppointmentPet>();
        await dbSet.AddAsync(entity);
        await context.SaveChangesAsync();
        context.Entry(entity).State = EntityState.Detached;
    }
}