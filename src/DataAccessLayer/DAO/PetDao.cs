using BusinessObject.Entities;
using DataAccessLayer.Base;
using Repository;

namespace DataAccessLayer.DAO;

public class PetDao : BaseDAO<Pet>
{
    public static readonly AppDbContext _context = new();
}