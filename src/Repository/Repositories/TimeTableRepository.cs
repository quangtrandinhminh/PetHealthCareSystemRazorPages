using BusinessObject.Entities;
using Repository.Base;
using Repository.Interfaces;
using Utility.Enum;

namespace Repository.Repositories;

public class TimeTableRepository : BaseRepository<TimeTable>, ITimeTableRepository
{
    public async Task<List<TimeTable>> GetAllBookingTimeFramesAsync()
    {
        var res = (await GetAllAsync()).ToList().Where(e => e.Note.Equals(TimeTableType.Book.ToString())).ToList();

        return res;
    }
}