using BusinessObject.Entities.Base;

namespace BusinessObject.Entities;

public class Configuration : BaseEntity
{
    public string ConfigKey { get; set; }
    public string Value { get; set; }   
    public string? Description { get; set; }
}