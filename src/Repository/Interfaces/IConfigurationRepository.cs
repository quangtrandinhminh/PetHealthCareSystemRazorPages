using BusinessObject.Entities;
using Repository.Base;

namespace Repository.Interfaces;

public interface IConfigurationRepository : IBaseRepository<Configuration>
{
    Task<Configuration?> GetValueByKey(string key);
}