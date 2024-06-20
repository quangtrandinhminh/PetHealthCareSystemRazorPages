using BusinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Base;

namespace Repository.Interfaces
{
    public interface IMedicalItemRepository : IBaseRepository<MedicalItem>
    {
        Task<List<MedicalItem>> GetAllMedicalItem();
        Task UpdateMedicalItemAsync(MedicalItem medicalItem);
        Task DeleteMedicalItemAsync(MedicalItem medicalItem);
        Task CreateMedicalItemAsync(MedicalItem medicalItem);
    }
}
