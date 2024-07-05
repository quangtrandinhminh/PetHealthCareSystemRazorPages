using BusinessObject.Entities;
using Repository.Base;

namespace Repository.Interfaces;

public interface IMedicalRecordRepository : IBaseRepository<MedicalRecord>
{
    Task<MedicalRecord> AddMedicalRecordAsync(MedicalRecord medicalRecord);
}