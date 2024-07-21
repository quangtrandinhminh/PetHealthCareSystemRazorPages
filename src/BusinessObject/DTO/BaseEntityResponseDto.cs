namespace BusinessObject.DTO;

public class BaseEntityResponseDto
{
    public int? CreatedBy { get; set; }
    public string? CreatedByName { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
    public int? LastUpdatedBy { get; set; }
    public string? LastUpdatedByName { get; set; }
    public DateTimeOffset LastUpdatedTime { get; set; }
}