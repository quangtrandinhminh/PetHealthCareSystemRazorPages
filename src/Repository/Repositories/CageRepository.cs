using BusinessObject.Entities;
using Repository.Base;
using Repository.Interfaces;

namespace Repository.Repositories;

public class CageRepository : BaseRepository<Cage>, ICageRepository
{
    public async Task CreateCageAsync(Cage cage)
    {
        await AddAsync(cage);
    }

    public async Task DeleteCageAsync(Cage cage)
    {
        await DeleteAsync(cage);
    }

    public async Task<List<Cage>> GetAllCage()
    {
        var list = await GetAllAsync();
        return list.Where(e => e.DeletedBy == null).ToList();
    }

    public async Task UpdateCageAsync(Cage cage)
    {
        await UpdateAsync(cage);
    }
}