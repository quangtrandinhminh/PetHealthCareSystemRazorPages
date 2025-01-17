﻿using BusinessObject.DTO.Transaction;
using System.ComponentModel.DataAnnotations;
using Utility.Constants;

namespace BusinessObject.DTO.MedicalRecord;

public class MedicalRecordRequestDto
{
    public int Id { get; set; }
    [Required(ErrorMessage = ResponseMessageConstantsAppointment.APPOINTMENT_ID_REQUIRED)]
    public int AppointmentId { get; set; }

    [Required(ErrorMessage = ResponseMessageConstantsPet.PETID_REQUIRED)]
    public int PetId { get; set; }
    public string? RecordDetails { get; set; }
    [Required(ErrorMessage = "Diagnosis không được để trống")]
    public string Diagnosis { get; set; }
    [Required(ErrorMessage = "Treatment không được để trống")]
    public string Treatment { get; set; }
    public string? Note { get; set; }
    public DateTimeOffset? NextAppointment { get; set; }

    [Range(0, double.MaxValue)]
    public decimal PetWeight { get; set; }
    public List<TransactionMedicalItemsDto>? MedicalItems { get; set; }

    // Hospitalization
    public DateTimeOffset? AdmissionDate { get; set; }
    public DateTimeOffset? DischargeDate { get; set; }
}