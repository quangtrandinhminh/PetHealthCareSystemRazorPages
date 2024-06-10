using BusinessObject.Entities;
using DataAccessLayer.Base;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.DAO;

public class PetDao : BaseDao<Pet>
{
    private static readonly AppDbContext _context = new();

    public async Task DeletePetAsync(int petId)
    {
        var pet = await _context.Pets
            .Include(p => p.AppointmentPets)
            .FirstOrDefaultAsync(p => p.Id == petId);

        if (pet != null)
        {
            _context.AppointmentPets.RemoveRange(pet.AppointmentPets);
            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();
        }
    }

}