using BusinessObject.Entities;
using Repository.Base;

namespace Repository.Interfaces;

public interface ITransactionRepository : IBaseRepository<Transaction>
{
    /*Task<Transaction?> GetTransactionWithDetailsAsync(int id);*/

    Task SaveChangesAsync();
}