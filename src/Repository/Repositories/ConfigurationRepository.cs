using BusinessObject.Entities;
using Repository.Base;
using Repository.Interfaces;

namespace Repository.Repositories;

public class ConfigurationRepository : BaseRepository<Configuration>, IConfigurationRepository
{
    public async Task<Configuration?> GetValueByKey(string key)
    {
        var config = (await FindByConditionAsync(e => e.ConfigKey == key)).FirstOrDefault();
        return config;
    }
}