using BusinessObject.Entities;
using Repository.Base;

namespace Repository.Interfaces;

public interface IPetRepository : IBaseRepository<Pet>
{
    Task<List<Pet>> GetAllPetsByCustomerIdAsync(int id);
    Task UpdatePetAsync(Pet pet);
    Task DeletePetAsync(Pet pet);
    Task CreatePetAsync(Pet pet);
}