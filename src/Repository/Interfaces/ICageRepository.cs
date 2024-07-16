using BusinessObject.Entities;
using Repository.Base;

namespace Repository.Interfaces;

public interface ICageRepository : IBaseRepository<Cage>
{
    Task<List<Cage>> GetAllCage();
    Task UpdateCageAsync(Cage cage);
    Task DeleteCageAsync(Cage cage);
    Task CreateCageAsync(Cage cage);
}