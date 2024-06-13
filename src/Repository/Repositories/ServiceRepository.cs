using BusinessObject.Entities;
using Repository.Base;
using Repository.Interfaces;

namespace Repository.Repositories;

public class ServiceRepository : BaseRepository<Service>, IServiceRepository
{
    public async Task CreateServiceAsync(Service service)
    {
        await AddAsync(service);
    }

    public async Task DeleteServiceAsync(Service service)
    {
        await DeleteAsync(service);
    }

    public async Task<List<Service>> GetAllService()
    {
        var list = await GetAllAsync();
        return list.Where(e => e.DeletedBy == null).ToList();
    }

    public async Task UpdateServiceAsync(Service service)
    {
        await UpdateAsync(service);
    }
}