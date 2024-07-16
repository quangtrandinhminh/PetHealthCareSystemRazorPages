using BusinessObject.DTO.Configuration;
using BusinessObject.Entities;

namespace Service.IServices;

public interface IConfigurationService
{
    Task<List<ConfigurationResponseDto>> GetConfigurationsAsync();
    Task<ConfigurationResponseDto> UpdateConfiguration(ConfigurationUpdateRequestDto dto);
}