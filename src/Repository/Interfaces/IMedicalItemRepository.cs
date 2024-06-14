using BusinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IMedicalItemRepository
    {
        Task<List<MedicalItem>> GetAllMedicalItem();
        Task UpdateMedicalItemAsync(MedicalItem medicalItem);
        Task DeleteMedicalItemAsync(MedicalItem medicalItem);
        Task CreateMedicalItemAsync(MedicalItem medicalItem);
    }
}
