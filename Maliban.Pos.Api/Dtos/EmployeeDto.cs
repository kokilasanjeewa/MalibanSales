using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maliban.Pos.Api.Dtos
{
  public class EmployeeDto
  {
    public int MI_Employer_EmployeeId { get; set; }
    //[Required]
    public string MI_Employer_EmployeeUid { get; set; }
/*    //[Required]
    public int? EmployeeCode { get; set; }
    //[Required]
    public int? hrTitle { get; set; }
    //[Required]
    public string Surname { get; set; }*/
    //[Required]
    public string Number { get; set; }
  /*  //[Required]
    public int? CardNumber { get; set; }
    //[Required]
    public Boolean Rostered { get; set; }*/
    //[Required]
    //[Required]
    public int? Deleted { get; set; }
 /*   //[Required]
    public int? DepartmentCode { get; set; }
    //[Required]
    public int? SectionCode { get; set; }
    //[Required]
    public int? CategoryCode { get; set; }
    //[Required]
    public string Forenames { get; set; }
    //[Required]

    public string JobTitle { get; set; }
    //[Required]
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Salary { get; set; }*/
    //[Required]
    public DateTime DateOfAppointment { get; set; }
 /*   //[Required]
    public DateTime DateOfBirth { get; set; }
    //[Required]
    public string NICNumber { get; set; }
    //[Required]
    public string Picture { get; set; }*/
    public int? SchemeCode { get; set; }
    [Required]
    public string EPFNo { get; set; }
  /*  //[Required]
    public DateTime DateDeleted { get; set; }
    //[Required]

    public string DeletedReason { get; set; }
    //[Required]
    public DateTime ConfirmDate { get; set; }*/
    //[Required]
    public DateTime DateResigned { get; set; }
  /*  //[Required]
    public int? CompanyCode { get; set; }
    public string ContactName { get; set; }
    //[Required]
    public string Email { get; set; }

    public string FirstName { get; set; }
    //[Required]
    public string MiddleName { get; set; }
    //[Required]

    public string LastName { get; set; }
    //[Required]
    public int? EmploymentType { get; set; }
    //[Required]
    public Boolean Probation { get; set; }
    public string MadrenName { get; set; }
    public string Surname2 { get; set; }
    ////[Required]
    public Boolean Meal { get; set; }
    // //[Required]
    public int? Status { get; set; }
    ////[Required]
    // [Display(Name = "Effected Date")]
    //  [DataType(DataType.Date)]
    public DateTime Effecteddate { get; set; }
    ////[Required]
    public string Initials { get; set; }
*/
    [Required]
    public string FullName { get; set; }
    // //[Required]
    //  [Range(1, 100)]
    //  [DataType(DataType.Currency)]
    //  [Column(TypeName = "decimal(18, 2)")]
  /*  [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal NewSalary { get; set; }
    // [StringLength(200)]
    public string MaidenName { get; set; }*/
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal CreditLimit { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal CreditBalance { get; set; } = 0;
    public string Scheme { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal PreviousCreditBalance { get; set; } = 0;

    public string Location { get; set; }
    public string MapLocation { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal CashLimit { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal CashBalance { get; set; } = 0;
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal PreviousCashBalance { get; set; } = 0;
    }
}
