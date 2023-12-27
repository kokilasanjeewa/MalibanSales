using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
  public class UserForDetailedDto : BaseDto
  {
    public int Id { get; set; }
    public string UserName { get; set; }
    public string NormalizedUserName { get; set; }
    public string Email { get; set; }
    public string NormalizedEmail { get; set; }
    public bool EmailConfirmed { get; set; }
    public string PasswordHash { get; set; }
    public string SecurityStamp { get; set; }
    public string ConcurrencyStamp { get; set; }
    public string PhoneNumber { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public DateTime LockoutEnd { get; set; }
    public bool LockoutEnabled { get; set; }
    public int IntaccessFailedCount { get; set; }
    public string Uid { get; set; }
    public string Password { get; set; }
    public string UserRole { get; set; }
    public string CreatedBy { get; set; }
    [Display(Name = "Created")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime RegDate { get; set; }
    public string UniqueId { get; set; }
    public string SystemNumber { get; set; }
    public string Mode { get; set; }
    public string Platform { get; set; }
    public string Uuid { get; set; }
    public string Version { get; set; }
    public string Feild1 { get; set; }
    public string Feild2 { get; set; }
    public string Feild3 { get; set; }
    public string SettingsOptions { get; set; }
    [Display(Name = "Created")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime LastAccessDate { get; set; }
    [Display(Name = "Created")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime LastDataSyncDate { get; set; }
    [Display(Name = "Created")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime LastConnectedDate { get; set; }
    [Display(Name = "Created")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime LastLoginDate { get; set; }
    public string LoginFailedReason { get; set; }
    public bool Deleted { get; set; }
    [Display(Name = "Created")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime LastStudentSyncTime { get; set; }
    [Display(Name = "Created")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime LastClassStudentSyncTime { get; set; }
    [Display(Name = "Created")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime LastAttendanceSyncTime { get; set; }
    [Display(Name = "Created")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime LastPaymentSyncTime { get; set; }
    public int IntFeild1 { get; set; }
    public int IntFeild2 { get; set; }
    public decimal DecimalFeild1 { get; set; }
    public decimal DecimalFeild2 { get; set; }
    public int DataToBeSynced { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int RootObjectid { get; set; }
    public string PhotoUrl { get; set; }
    public int Age { get; set; }
  }
}