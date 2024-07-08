using BusinessObject.Entities;
using DataAccessLayer.DAO;
using Repository.Base;
using Repository.Interfaces;

namespace Repository.Repositories;

public class MedicalRecordRepository : BaseRepository<MedicalRecord>, IMedicalRecordRepository
{
    public async Task<MedicalRecord> AddMedicalRecordAsync(MedicalRecord medicalRecord)
        => await MedicalRecordDao.AddMedicalRecordAsync(medicalRecord);
}