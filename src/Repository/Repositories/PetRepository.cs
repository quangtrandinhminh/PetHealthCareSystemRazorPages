using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Base;
using Repository.Interfaces;

namespace Repository.Repositories;

public class PetRepository : BaseRepository<Pet>, IPetRepository
{
    public async Task<List<Pet>> GetAllPetsByCustomerIdAsync(int id)
    {
        var list = GetAll().Include(e => e.Owner);
        return await list.Where(e => e.OwnerID == id && e.DeletedBy == null).ToListAsync();
    }

    public async Task UpdatePetAsync(Pet pet)
    {
        await UpdateAsync(pet);
    }

    public async Task DeletePetAsync(Pet pet)
    {
        await UpdatePetAsync(pet);
    }

    public async Task CreatePetAsync(Pet pet)
    {
        await AddAsync(pet);
    }
}