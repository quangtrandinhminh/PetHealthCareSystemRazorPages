using BusinessObject.Entities;
using Repository.Base;
using Repository.Interfaces;

namespace Repository.Repositories
{
    public class MedicalItemRepository : BaseRepository<MedicalItem>, IMedicalItemRepository
    {
        public async Task CreateMedicalItemAsync(MedicalItem medicalItem)
        {
            await AddAsync(medicalItem);
        }

        public async Task DeleteMedicalItemAsync(MedicalItem medicalItem)
        {
            await DeleteAsync(medicalItem);
        }

        public async Task<List<MedicalItem>> GetAllMedicalItem()
        {
            var list = await GetAllAsync();
            return list.Where(e => e.DeletedBy == null).ToList();
        }

        public async Task UpdateMedicalItemAsync(MedicalItem medicalItem)
        {
            await UpdateAsync(medicalItem);
        }
    }
}
