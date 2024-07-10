using BusinessObject.DTO.MedicalItem;
using System.Globalization;
using BusinessObject.DTO.Appointment;
using BusinessObject.DTO.Cage;
using BusinessObject.DTO.MedicalRecord;
using BusinessObject.DTO.Pet;
using BusinessObject.DTO.Service;
using BusinessObject.DTO.TimeTable;
using BusinessObject.DTO.Transaction;
using BusinessObject.DTO.User;
using BusinessObject.DTO.Vet;
using BusinessObject.Entities;
using BusinessObject.Entities.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Riok.Mapperly.Abstractions;
using BusinessObject.DTO.Hospitalization;

namespace BusinessObject.Mapper;

[Mapper]
public partial class MapperlyMapper
{
    //  public partial entity UserToLoginResponseDto(dto request); --for create
    // public partial dtoresponse UserToLoginResponseDto(entity entity); --for get
    // public partial IList<dtoresponse> UserToLoginResponseDto(IList<entity> entity); --for get all
    // public partial void UserToLoginResponseDto(dto request, entity entity); --for update

    // user
    public partial IList<RoleResponseDto> Map(IList<RoleEntity> entity);
    public partial UserEntity Map(RegisterDto request);
    public partial UserEntity Map(VetRequestDto request);
    public partial LoginResponseDto UserToLoginResponseDto(UserEntity entity);
    public partial UserResponseDto UserToUserResponseDto(UserEntity entity);
    public partial IList<UserResponseDto> Map(IList<UserEntity> entity);
    public partial void Map(RegisterDto request, UserEntity entity);

    // pet 
    public partial Pet Map(PetRequestDto request);
    public PetResponseDto Map(Pet entity)
    {
        var response = new PetResponseDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Species = entity.Species,
            Breed = entity.Breed,
            Gender = entity.Gender,
            DateOfBirth = DateOnly.FromDateTime(entity.DateOfBirth.Date),
            IsNeutered = entity.IsNeutered,
            OwnerName = entity.Owner?.FullName ?? "N/A",
        };

        return response;
    }
    // Custom mapping method for IList<Pet> to IList<PetResponseDto> with date formatting
    public partial List<PetResponseDto?> Map(List<Pet> entities);
    public partial Pet Map(PetUpdateRequestDto request);
    public partial void Map(PetRequestDto request, Pet entity);

    // service
    public partial Service Map(ServiceRequestDto request);
    public partial Service Map(ServiceResponseDto request);
    public partial ServiceResponseDto Map(Service entity);
    public partial IList<ServiceResponseDto> Map(IList<Service> entity);
    public List<ServiceResponseDto?> Map(ICollection<Service?> entities)
    {
        return entities.Select(Map).ToList();
    }
    public partial void Map(ServiceRequestDto request, Service entity);

    // timetable
    //public partial TimeTable UserToLoginResponseDto(TimeTableRequestDto request);
    public partial TimeTableResponseDto Map(TimeTable entity);
    public partial IQueryable<TimeTableResponseDto> Map(IQueryable<TimeTable> entity);

    // medicalItem
    public partial MedicalItem Map(MedicalResponseDto request);
    public partial MedicalResponseDto Map(MedicalItem entity);
    public partial IList<MedicalResponseDto> Map(IList<MedicalItem> entity);
    public partial void Map(ServiceRequestDto request, MedicalItem entity);

    // transaction
    public partial Transaction Map(TransactionRequestDto request);
    public partial Transaction Map(TransactionResponseDto request);
    public partial TransactionResponseDto Map(Transaction entity);
    public partial TransactionResponseWithDetailsDto TransactionToTransactionResponseWithDetails(Transaction entity);
    public partial IQueryable<TransactionResponseDto> Map(IQueryable<Transaction> entity);
    public partial void Map(TransactionRequestDto request, Transaction entity);

    // transaction detail
    public partial IList<TransactionDetailResponseDto> Map(IList<TransactionDetail> entity);

    // medical record
    public partial MedicalRecord Map(MedicalRecordRequestDto request);
    public partial MedicalRecordResponseDto Map(MedicalRecord entity);

    public partial MedicalRecordResponseDtoWithDetails MedicalRecordToMedicalRecordResponseDtoWithDetails(
        MedicalRecord entity);
    public partial void Map(MedicalRecordResponseDto request, MedicalRecord entity);
    public partial IQueryable<MedicalRecordResponseDto> Map(IQueryable<MedicalRecord> entity);

    // appointment
    public partial AppointmentResponseDto Map(Appointment entity);
    public partial IQueryable<AppointmentResponseDto> Map(IQueryable<Appointment> entities);
    public partial Appointment Map(AppointmentBookRequestDto request);

    // hospitalization
    public partial HospitalizationResponseDto Map(Hospitalization entity);
    public partial IQueryable<HospitalizationResponseDto> Map(IQueryable<Hospitalization> entity);

    public partial HospitalizationResponseDtoWithDetails HospitalizationToHospitalizationResponseDtoWithDetails(
               Hospitalization entity);

    public partial Hospitalization Map(HospitalizationRequestDto request);


    // cage
    public partial CageResponseDto Map(Cage entity);
    public partial IQueryable<CageResponseDto> Map(IQueryable<Cage> entity);
}