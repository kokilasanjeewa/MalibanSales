using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maliban.Pos.Api.Models
{
  //[Range(1, 100)]
  //[DataType(DataType.Currency)]
  //[Column(TypeName = "decimal(18, 2)")]
  //[RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
  //[Display(Name = "Release Date")]
  //[DataType(DataType.Date)]
  //https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-data-type-mappings
  public class MI_Employer_Employee
  {
    [Key]
    public int MI_Employer_EmployeeId { get; set; }
    //[Required]
    [NotMapped]
    public string MI_Employer_EmployeeUid { get; set; } = Guid.NewGuid().ToString();
    //[Required]
    public int? EmployeeCode { get; set; }
    //[Required]
    public int? hrTitle { get; set; }
    //[Required]
    [StringLength(150)]
    public string Surname { get; set; }
    //[Required]
    [StringLength(50)]
    public string Number { get; set; }
    //[Required]
    public int? CardNumber { get; set; }
    //[Required]
    public Boolean Rostered { get; set; }
    //[Required]
    public int? AssignmentCode { get; set; }
    //[Required]
    public int? GroupNumber { get; set; }
    //[Required]
    public int? Deleted { get; set; }
    //[Required]
    public int? DepartmentCode { get; set; }
    //[Required]
    public int? SectionCode { get; set; }
    //[Required]
    public int? CategoryCode { get; set; }
    //[Required]
    [StringLength(50)]
    public string Forenames { get; set; }
    //[Required]
    [StringLength(50)]

    public string JobTitle { get; set; }
    //[Required]
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Salary { get; set; }
    //[Required]
    public DateTime? DateOfAppointment { get; set; }
    //[Required]
    public DateTime? DateOfBirth { get; set; }
    //[Required]
    [StringLength(15)]
    public string NICNumber { get; set; }
    //[Required]
    [StringLength(50)]
    public string Picture { get; set; }
    [Required]
    public int? SchemeCode { get; set; }
    //[Required]
    public int? OperatorCode { get; set; }
    //[Required]
    [StringLength(10)]
    public int? TextCardNumber { get; set; }
    //[Required]
    public byte Overtime { get; set; }
    //[Required]
    public Boolean JulyJune { get; set; }
    //[Required]
    public int? DesignationCode { get; set; }
    //[Required]
    public int? ReligionCode { get; set; }
    //[Required]
    public int? Married { get; set; }
    //[Required]
    public int? Male { get; set; }
    //[Required]
    public Boolean Casual { get; set; }
    //[Required]
    [StringLength(500)]

    public string PAddress2 { get; set; }
    //[Required]
    [StringLength(50)]

    public string PAddress3 { get; set; }
    //[Required]
    public Boolean AnnualTax { get; set; }
    //[Required]
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(19, 2)")]
    public decimal MinimumOTPayment { get; set; }
    [Required]
    [StringLength(20)]
    public string EPFNo { get; set; }
    //[Required]
    public DateTime? DateDeleted { get; set; }
    //[Required]
    [StringLength(100)]

    public string DeletedReason { get; set; }
    //[Required]
    public Boolean UnionMember { get; set; }
    //[Required]
    public DateTime? ConfirmDate { get; set; }
    //[Required]
    public int? GradeCode { get; set; }
    //[Required]
    public DateTime? DateResigned { get; set; }
    //[Required]
    [Column(TypeName = "decimal(18, 0)")]
    public decimal PageOrder { get; set; }
    //[Required]
    [StringLength(50)]
    public string PAddress1 { get; set; }
    //[Required]
    [StringLength(16)]

    public string PTelephone { get; set; }
    //[Required]
    public int? PDistrict { get; set; }
    //[Required]
    [StringLength(150)]
    public string RAddress1 { get; set; }
    //[Required]
    [StringLength(150)]

    public string RAddress2 { get; set; }
    //[Required]
    [StringLength(150)]

    public string RAddress3 { get; set; }
    //[Required]
    [StringLength(16)]
    public string RTelephone { get; set; }
    //[Required]
    public int? RDistrict { get; set; }
    //[Required]
    [StringLength(16)]

    public string hrMobileNo { get; set; }
    //[Required]
    public int? CompanyCode { get; set; }
    //[Required]
    public int? PCostCentreCode { get; set; }
    //[Required]
    public int? SCostCentreCode { get; set; }
    //[Required]
    [StringLength(50)]
    public string ContactName { get; set; }
    //[Required]
    public int? Relation { get; set; }
    //[Required]
    [StringLength(50)]
    public string RContAddr1 { get; set; }
    //[Required]
    [StringLength(50)]
    public string RContAddr2 { get; set; }
    //[Required]
    [StringLength(50)]
    public string RContAddr3 { get; set; }
    //[Required]
    [StringLength(16)]
    public string RContTel { get; set; }
    //[Required]
    [StringLength(50)]
    public string OContAddr1 { get; set; }
    //[Required]
    [StringLength(50)]
    public string OContAddr2 { get; set; }
    //[Required]
    [StringLength(50)]
    public string OContAddr3 { get; set; }
    //[Required]
    [StringLength(16)]
    public string OContTel { get; set; }
    //[Required]
    [StringLength(16)]
    public string hrContMobile { get; set; }
    //[Required]
    public int? hrLiving { get; set; }
    //[Required]
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(19, 2)")]
    public decimal hrIncome { get; set; }
    //[Required]
    [StringLength(50)]
    public string Father { get; set; }
    //[Required]
    public int? FAge { get; set; }
    //[Required]
    [StringLength(50)]
    public string hrFOccupation { get; set; }
    //[Required]
    [StringLength(50)]
    public string Mother { get; set; }
    //[Required]
    public int? MAge { get; set; }
    //[Required]
    [StringLength(50)]
    public string hrMOccupation { get; set; }
    //[Required]
    [StringLength(50)]

    public string Spouse { get; set; }
    //[Required]
    public int? SAge { get; set; }
    //[Required]
    [StringLength(50)]
    public string hrSOccupation { get; set; }
    //[Required]
    public int? Dependants { get; set; }
    //[Required]
    public int? hrChildren { get; set; }
    //[Required]
    [StringLength(20)]
    public string hrPassportNo { get; set; }
    //[Required]
    public DateTime? hrExpiryDate { get; set; }
    //[Required]
    public int? hrNationalityCode { get; set; }
    //[Required]
    public int? hrRaceCode { get; set; }
    //[Required]
    public int? hrAge { get; set; }
    //[Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal hrDistance { get; set; }
    //[Required]
    public int? hrTravelMode { get; set; }
    //[Required]
    [StringLength(50)]
    public string hrODescription { get; set; }
    //[Required]
    public int? SectorCode { get; set; }
    //[Required]
    public int? RequisitionId { get; set; }
    //[Required]
    [StringLength(100)]
    public string Email { get; set; }
    //[Required]
    public int? ApplicantCode { get; set; }
    //[Required]
    [StringLength(200)]

    public string FirstName { get; set; }
    //[Required]
    [StringLength(200)]
    public string MiddleName { get; set; }
    //[Required]
    [StringLength(200)]

    public string LastName { get; set; }
    //[Required]
    public Boolean MSPS { get; set; }
    //[Required]
    public int? EmploymentType { get; set; }
    //[Required]
    public Boolean Probation { get; set; }
    [Required]
    [StringLength(2)]
    public string Location { get; set; }
    //[Required]
    public int? VersionCode { get; set; }
    //[Required]
    public int? AttendAllowGroup { get; set; }
    //[Required]
    public DateTime? hrDOBofSpouse { get; set; }
    //[Required]
    public DateTime? hrDOBofMother { get; set; }
    //[Required]
    public DateTime? hrDOBofFather { get; set; }
    //[Required]
    public int? BloodGroup { get; set; }
    //[Required]
    public int? JoinMethod { get; set; }
    //[Required]
    public int? ReportingDesignation { get; set; }
    //[Required]
    public byte FatherMedEntitle { get; set; }
    //[Required]
    public byte MotherMedEntitle { get; set; }
    //[Required]
    public byte SpouseMedEntitle { get; set; }
    //[Required]
    public Single TransportPerDay { get; set; }
    //[Required]
    public int? Level1Code { get; set; }
    //[Required]
    public int? Level2Code { get; set; }
    //[Required]
    public int? Level3Code { get; set; }
    //[Required]
    public int? Level4Code { get; set; }
    //[Required]
    public int? Level5Code { get; set; }
    //[Required]
    public int? Level6Code { get; set; }
    //[Required]
    public int? Level7Code { get; set; }
    //[Required]
    public int? Level8Code { get; set; }
    //[Required]
    public int? Level9Code { get; set; }
    // //[Required]
    public int? Level10Code { get; set; }
    ////[Required]
    // [StringLength(50)]
    public string DriverLicencesNo { get; set; }
    // //[Required]
    //  [StringLength(200)]
    public string MadrenName { get; set; }
    //  //[Required]
    //  [StringLength(200)]
    public string ElecDistrict { get; set; }
    // //[Required]
    //  [StringLength(50)]
    public string ServicePhone { get; set; }
    // //[Required]
    //  [StringLength(50)]
    public string StaffPhone { get; set; }
    // //[Required]
    // [StringLength(50)]
    public string IEMINo { get; set; }
    ////[Required]
    //  [StringLength(50)]
    public string CouplePhone { get; set; }
    // //[Required]
    //  [StringLength(100)]

    public string AnyOtherPhone { get; set; }
    // //[Required]
    public int? JoinDesignation { get; set; }
    //  //[Required]
    //  [StringLength(200)]
    public string fEmployer { get; set; }
    //  //[Required]
    // [StringLength(200)]
    public string mEmployer { get; set; }
    // //[Required]
    // [StringLength(200)]
    public string sEmployer { get; set; }
    // //[Required]
    //  [Column(TypeName = "decimal(18, 2)")]
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal PoleStationDistance { get; set; }
    // //[Required]
    public int? PositionCode { get; set; }
    //  //[Required]
    // [StringLength(50)]
    public string EncryptSal { get; set; }
    // //[Required]
    //  [StringLength(100)]
    public string OptEmail { get; set; }
    // //[Required]
    public int? rptPerson { get; set; }
    //  //[Required]
    //  [StringLength(30)]
    public string Extension { get; set; }
    //  //[Required]
    //  [StringLength(100)]
    public string Surname2 { get; set; }
    ////[Required]
    // [StringLength(200)]

    public string OtherSystems { get; set; }
    ////[Required]
    public Boolean Meal { get; set; }
    // //[Required]
    public int? EmpPlanCode { get; set; }
    // //[Required]
    //  [StringLength(50)]
    public string ServicePhoneAccNo { get; set; }
    // //[Required]
    //  [StringLength(50)]

    public string StaffPhoneAccNo { get; set; }
    // //[Required]
    //[StringLength(50)]
    public string CouplePhoneAccNo { get; set; }
    ////[Required]
    public int? PCountry { get; set; }
    // //[Required]
    public int? RCountry { get; set; }
    ////[Required]
    public int? ECurrency { get; set; }
    ////[Required]
    public int? FLive { get; set; }
    // //[Required]
    public int? MLive { get; set; }
    // //[Required]
    public int? Status { get; set; }
    ////[Required]
    // [Display(Name = "Effected Date")]
    //  [DataType(DataType.Date)]
    public DateTime? Effecteddate { get; set; }
    ////[Required]
    public string Initials { get; set; }
    ////[Required]
    public byte Expatriate { get; set; }
    // //[Required]
    // [StringLength(50)]
    public string Housing { get; set; }
    // //[Required]
    //   [StringLength(50)]
    public string Transportation { get; set; }
    [Required]
    [StringLength(200)]
    public string FullName { get; set; }
    // //[Required]
    //  [Range(1, 100)]
    //  [DataType(DataType.Currency)]
    //  [Column(TypeName = "decimal(18, 2)")]
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal NewSalary { get; set; }
    // //[Required]
    public string NameAsIqama { get; set; }
    // //[Required]
    // [StringLength(200)]
    public string StatuName { get; set; }
    public int? CareerPathID { get; set; }
    // //[Required]
    // [StringLength(200)]
    public string MaidenName { get; set; }
    ////[Required]
    // [StringLength(500)]
    public string UPNname { get; set; }
    // //[Required]
    public int? PhysicalLocationCode { get; set; }
    // //[Required]
    //  [StringLength(11)]
    public string fNIC { get; set; }
    // //[Required]
    //  [StringLength(11)]
    public string mNIC { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal CreditLimit { get; set; } = 0;
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal CashLimit { get; set; } = 0;
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal CreditBalance { get; set; } = 0;
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal CashBalance { get; set; } = 0;
        /*     [Column(TypeName = "decimal(18, 2)")]
            [DisplayFormat(DataFormatString = "{0:C}")]
            public decimal CustomerReturnAmount { get; set; } = 0; */
    public string Scheme { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal PreviousCreditBalance { get; set; } = 0;

    [Column(TypeName = "decimal(18, 2)")]
    [DisplayFormat(DataFormatString = "{0:C}")]
    public decimal PreviousCashBalance { get; set; } = 0;

    }
}