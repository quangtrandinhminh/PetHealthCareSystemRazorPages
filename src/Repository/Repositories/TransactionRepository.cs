using BusinessObject.Entities;
using DataAccessLayer.DAO;
using Repository.Base;
using Repository.Interfaces;

namespace Repository.Repositories;

public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
{
    /*public async Task<Transaction?> GetTransactionWithDetailsAsync(int id) =>
        await TransactionDao.GetTransactionWithDetailsAsync(id);*/

    public async Task<Transaction?> CreateTransactionAsync(Transaction transaction) =>
    await TransactionDao.CreateTransactionAsync(transaction);

    public async Task SaveChangesAsync() =>
        await TransactionDao.SaveChangesAsync();
}