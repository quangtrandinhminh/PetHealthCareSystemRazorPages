using BusinessObject.Entities;
using Repository.Base;

namespace Repository.Interfaces;

public interface ITimeTableRepository : IBaseRepository<TimeTable>
{
    public Task<List<TimeTable>> GetAllBookingTimeFramesAsync();
}