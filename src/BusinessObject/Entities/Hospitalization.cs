﻿using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Entities.Base;
using BusinessObject.Entities.Identity;
using Utility.Enum;

namespace BusinessObject.Entities;

public class Hospitalization : BaseEntity
{
    public int MedicalRecordId { get; set; }
    public int VetId { get; set; }
    public int CageId { get; set; }
    public int? TimeTableId { get; set; }
    public DateOnly Date { get; set; }
    public HospitalizationStatus HospitalizationDateStatus { get; set; }
    public string? Reason { get; set; }
    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }
    public string? Note { get; set; }
    
    [ForeignKey(nameof(MedicalRecordId))]
    public virtual MedicalRecord MedicalRecord { get; set; }

    [ForeignKey(nameof(CageId))]
    public virtual Cage Cage { get; set; }

    [ForeignKey(nameof(TimeTableId))]
    public virtual TimeTable? TimeTable { get; set; }
}