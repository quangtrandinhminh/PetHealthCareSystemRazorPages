using BusinessObject.Entities;
using Repository.Base;
using Repository.Interfaces;

namespace Repository.Repositories;

public class HospitalizationRepository : BaseRepository<Hospitalization>, IHospitalizationRepository
{
    public async Task CreateHospitalizationAsync(Hospitalization hospitalization)
    {
        await AddAsync(hospitalization);
    }

    public async Task DeleteHospitalizationAsync(Hospitalization hospitalization)
    {
        await DeleteAsync(hospitalization);
    }

    public async Task<List<Hospitalization>> GetAllHospitalization()
    {
        var list = await GetAllAsync();

        return list.Where(e => e.DeletedBy == null).ToList();
    }

    public async Task UpdateHospitalizationAsync(Hospitalization hospitalization)
    {
        await UpdateAsync(hospitalization);
    }
}