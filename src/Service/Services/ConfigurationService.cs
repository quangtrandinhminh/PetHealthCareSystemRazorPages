using BusinessObject.DTO.Configuration;
using BusinessObject.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Repository.Interfaces;
using Service.IServices;
using Utility.Constants;
using Utility.Exceptions;

namespace Service.Services;

public class ConfigurationService : IConfigurationService
{
    private readonly IConfigurationRepository _configurationRepo;

    public ConfigurationService(IConfigurationRepository configurationRepository)
    {
        _configurationRepo = configurationRepository;
    }

    public async Task<List<ConfigurationResponseDto>> GetConfigurationsAsync()
    {
        var keyList = ConfigurationKey.KeyDictionary.Keys.ToList();
        List<ConfigurationResponseDto> configurations = new();

        foreach (var key in keyList)
        {
            var findConfig = await _configurationRepo.GetValueByKey(key);
            if (findConfig != null)
            {
                var config = new ConfigurationResponseDto()
                {
                    DisplayString = ConfigurationKey.KeyDictionary[key],
                    Key = key,
                    Value = findConfig.Value,
                };
                configurations.Add(config);
            }
            else
            {
                var config = new ConfigurationResponseDto()
                {
                    DisplayString = ConfigurationKey.KeyDictionary[key],
                    Key = key,
                    Value = null,
                };
                configurations.Add(config);
            }
        }

        return configurations;
    }

    public async Task<ConfigurationResponseDto> UpdateConfiguration(ConfigurationUpdateRequestDto dto)
    {
        if (!ConfigurationKey.KeyDictionary.ContainsKey(dto.Key))
        {
            throw new AppException(ResponseCodeConstants.NOT_FOUND, ResponseMessageConstantsCommon.NOT_FOUND,
                StatusCodes.Status404NotFound);
        }

        var findConfig = await _configurationRepo.GetValueByKey(dto.Key);

        if (findConfig == null)
        {
            Configuration config = new Configuration()
            {
                ConfigKey = dto.Key,
                Value = dto.Value,
            };
            var createConfig = await _configurationRepo.AddAsync(config);

            return new ConfigurationResponseDto()
            {
                DisplayString = ConfigurationKey.KeyDictionary[createConfig.ConfigKey],
                Key = createConfig.ConfigKey,
                Value = createConfig.Value,
            };
        }

        findConfig.Value = dto.Value;
        await _configurationRepo.UpdateAsync(findConfig);

        return new ConfigurationResponseDto()
        {
            DisplayString = ConfigurationKey.KeyDictionary[findConfig.ConfigKey],
            Key = findConfig.ConfigKey,
            Value = findConfig.Value,
        };
    }
}