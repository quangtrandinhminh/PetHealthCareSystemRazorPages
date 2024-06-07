using BusinessObject.Entities;
using Repository.Base;
using Repository.Interface;

namespace Repository.Repositories;

public class PetRepository : BaseRepository<Pet>, IPetRepository
{
    
}