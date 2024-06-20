using BusinessObject.Entities;
using Repository.Base;
using Repository.Interfaces;

namespace Repository.Repositories;

public class PetRepository : BaseRepository<Pet>, IPetRepository
{
    public async Task<List<Pet>> GetAllPetsByCustomerIdAsync(int id)
    {
        var list = await GetAllAsync();
        return list.Where(e => e.OwnerID == id && e.DeletedBy == null).ToList();
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