using BusinessObject.Entities;
using DataAccessLayer.Base;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.DAO;

public class MedicalRecordDao : BaseDao<MedicalRecord>
{
    public static async Task<MedicalRecord> AddMedicalRecordAsync(MedicalRecord medicalRecord)
    {
        try
        {
            await using var db = new AppDbContext();

            foreach (var item in medicalRecord.MedicalItems.ToList())
            {
                var existingService = db.MedicalItems.SingleOrDefault(e => e.Id == item.Id);

                if (existingService != null)
                {
                    // Attach the existing tag
                    db.Entry(existingService).State = EntityState.Unchanged;
                    medicalRecord.MedicalItems.Remove(item);
                    medicalRecord.MedicalItems.Add(existingService);
                }
            }

            var addedMedicalRecord = await db.MedicalRecords.AddAsync(medicalRecord);
            await db.SaveChangesAsync();

            return addedMedicalRecord.Entity;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}