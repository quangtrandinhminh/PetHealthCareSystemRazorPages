using BusinessObject.DTO.MedicalItem;
using BusinessObject.Entities;
using BusinessObject.Mapper;
using Microsoft.Extensions.DependencyInjection;
using Repository.Interfaces;
using Service.IServices;

namespace Service.Services
{
    public class MedicalItemService(IServiceProvider serviceProvider) : IMedicalItemService
    {
        private readonly MapperlyMapper _mapper = serviceProvider.GetRequiredService<MapperlyMapper>();
        private readonly IMedicalItemRepository _medicalItemRepo = serviceProvider.GetRequiredService<IMedicalItemRepository>();

        public async Task CreateMedicalItem(MedicalResponseDto medicalItem)
        {
            await _medicalItemRepo.CreateMedicalItemAsync(_mapper.Map(medicalItem));
        }

        public Task DeleteMedicalItem(int id, int deleteBy)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MedicalResponseDto>> GetAllMedicalItem()
        {
            var list = await _medicalItemRepo.GetAllMedicalItem();

            var listDto = _mapper.Map(list);

            return listDto.ToList();
        }

        public Task UpdateMedicalItem(MedicalResponseDto medicalItem)
        {
            throw new NotImplementedException();
        }
    }
}
