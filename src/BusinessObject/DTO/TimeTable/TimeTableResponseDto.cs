using BusinessObject.DTO.Vet;

namespace BusinessObject.DTO.TimeTable;

public class TimeTableResponseDto
{
    public DateTimeOffset DateTimeStart { get; set; }
    public DateTimeOffset DateTimeEnd { get; set; }
    public IList<DayOfWeek> DayOfWeeks { get; set; }
    public VetResponseDto Vet { get; set; }
}