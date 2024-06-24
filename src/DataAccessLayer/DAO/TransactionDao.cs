using BusinessObject.Entities;
using DataAccessLayer.Base;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.DAO;

public class TransactionDao : BaseDao<Transaction>
{
    /*public static async Task<Transaction?> GetTransactionWithDetailsAsync(int id)
    {
        await using var context = new AppDbContext();
        return await context.Set<Transaction>()
            .Where(t => t = )
            .Include(t => t.TransactionDetails)
            .FirstOrDefaultAsync(x => x.Id == id);
    }*/

    public static async Task<Transaction?> CreateTransactionAsync(Transaction transaction)
    {
        await using var context = new AppDbContext();
        await context.Set<Transaction>().AddAsync(transaction);
        return transaction;
    }

    // save changes
    public static async Task SaveChangesAsync()
    {
        await using var context = new AppDbContext();
        await context.SaveChangesAsync();
    }
}