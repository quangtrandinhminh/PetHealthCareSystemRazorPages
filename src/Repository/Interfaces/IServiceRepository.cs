using BusinessObject.Entities;
using Repository.Base;

namespace Repository.Interfaces;

public interface IServiceRepository : IBaseRepository<Service>
{
    Task<List<Service>> GetAllService();
    Task UpdateServiceAsync(Service service);
    Task DeleteServiceAsync(Service service);
    Task CreateServiceAsync(Service service);
}