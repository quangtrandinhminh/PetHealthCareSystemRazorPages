using BusinessObject.DTO.Pet;
using BusinessObject.DTO.Service;
using BusinessObject.DTO.TimeTable;
using BusinessObject.DTO.User;
using BusinessObject.DTO.Vet;
using BusinessObject.Entities;
using BusinessObject.Entities.Identity;
using Riok.Mapperly.Abstractions;

namespace BusinessObject.Mapper;

[Mapper]
public partial class MapperlyMapper
{
    //  public partial entity UserToLoginResponseDto(dto request); --for create
    // public partial dtoresponse UserToLoginResponseDto(entity entity); --for get
    // public partial IList<dtoresponse> UserToLoginResponseDto(IList<entity> entity); --for get all
    // public partial void UserToLoginResponseDto(dto request, entity entity); --for update

    // user
    public partial UserEntity Map(RegisterDto request);
    public partial LoginResponseDto UserToLoginResponseDto(UserEntity entity);
    public partial UserResponseDto UserToUserResponseDto(UserEntity entity);
    public partial IList<UserResponseDto> Map(IList<UserEntity> entity);
    public partial void Map(RegisterDto request, UserEntity entity);

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

    // timetable
    //public partial TimeTable UserToLoginResponseDto(TimeTableRequestDto request);
    public partial TimeTableResponseDto Map(TimeTable entity);
    public partial IList<TimeTableResponseDto> Map(IList<TimeTable> entity);
}