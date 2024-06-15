using BusinessObject.DTO.MedicalItem;
using System.Globalization;
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
    public partial UserEntity Map(VetRequestDto request);
    public partial LoginResponseDto UserToLoginResponseDto(UserEntity entity);
    public partial UserResponseDto UserToUserResponseDto(UserEntity entity);
    public partial IList<UserResponseDto> Map(IList<UserEntity> entity);
    public partial void Map(RegisterDto request, UserEntity entity);

    // pet 
    public partial Pet Map(PetRequestDto request);
    // Custom mapping method for Pet to PetResponseDto with date formatting
    public PetResponseDto Map(Pet entity)
    {
        var response = new PetResponseDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Species = entity.Species,
            Breed = entity.Breed,
            Gender = entity.Gender,
            DateOfBirth = entity.DateOfBirth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
            IsNeutered = entity.IsNeutered
        };
        return response;
    }
    // Custom mapping method for IList<Pet> to IList<PetResponseDto> with date formatting
    public IList<PetResponseDto> Map(IList<Pet> entities)
    {
        return entities.Select(Map).ToList();
    }
    public partial Pet Map(PetUpdateRequestDto request);
    public partial void Map(PetRequestDto request, Pet entity);

    // service
    public partial Service Map(ServiceRequestDto request);
    public partial Service Map(ServiceResponseDto request);
    public partial ServiceResponseDto Map(Service entity);
    public partial IList<ServiceResponseDto> Map(IList<Service> entity);
    public partial void Map(ServiceRequestDto request, Service entity);

    // timetable
    //public partial TimeTable UserToLoginResponseDto(TimeTableRequestDto request);
    public partial TimeTableResponseDto Map(TimeTable entity);
    public partial IList<TimeTableResponseDto> Map(IList<TimeTable> entity);

    // medicalItem
    public partial MedicalItem Map(MedicalResponseDto request);
    public partial MedicalResponseDto Map(MedicalItem entity);
    public partial IList<MedicalResponseDto> Map(IList<MedicalItem> entity);
    public partial void Map(ServiceRequestDto request, MedicalItem entity);
}