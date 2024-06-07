using BusinessObject.DTO.Pet;
using BusinessObject.DTO.Service;
using BusinessObject.DTO.User;
using BusinessObject.DTO.Vet;
using BusinessObject.Entities;
using BusinessObject.Entities.Identity;
using Riok.Mapperly.Abstractions;

namespace BusinessObject.Mapper;

[Mapper]
public partial class MapperlyMapper
{
    //  public partial entity Map(dto request); --for create
    // public partial dtoresponse Map(entity entity); --for get
    // public partial IList<dtoresponse> Map(IList<entity> entity); --for get all
    // public partial void Map(dto request, entity entity); --for update

    // user
    public partial UserEntity Map(RegisterDto request);
    public partial LoginResponseDto Map(UserEntity entity);
    public partial IList<LoginResponseDto> Map(IList<UserEntity> entity);
    public partial void Map(UserEntity entity, UserResponseDto response);
    public partial void Map(IList<UserEntity> entity, IList<UserResponseDto> response);
    public partial void Map(RegisterDto request, UserEntity entity);

    // vet
    public partial UserEntity Map(VetRequestDto request);
    public partial void Map(UserEntity entity, VetResponseDto response);
    public partial void Map(IList<UserEntity> entity, IList<VetResponseDto> response);
    public partial void Map(VetRequestDto request, UserEntity entity);

    // pet 
    public partial Pet Map(PetRequestDto request);
    public partial PetResponseDto Map(Pet entity);
    public partial IList<PetResponseDto> Map(IList<Pet> entity);
    public partial void Map(PetRequestDto request, Pet entity);

    // service
    public partial Service Map(ServiceRequestDto request);
    public partial ServiceResponseDto Map(Service entity);
    public partial IList<ServiceResponseDto> Map(IList<Service> entity);
    public partial void Map(ServiceRequestDto request, Service entity);

}