using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Maliban.Pos.Api.Migrations
{
    public partial class Initialdb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          /*  migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Description = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    CityId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    NameInEnglish = table.Column<string>(name: "Name In English", maxLength: 50, nullable: false),
                    NameInSinhala = table.Column<string>(name: "Name In Sinhala", maxLength: 50, nullable: false),
                    NameInTamil = table.Column<string>(name: "Name In Tamil", maxLength: 50, nullable: false),
                    PostalCode = table.Column<string>(name: "Postal Code", maxLength: 5, nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    CityUid = table.Column<string>(nullable: false),
                    DistrictCode = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.CityId);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CountryName = table.Column<string>(maxLength: 100, nullable: false),
                    ISO = table.Column<string>(maxLength: 2, nullable: true),
                    NiceName = table.Column<string>(maxLength: 100, nullable: true),
                    ISO3 = table.Column<string>(maxLength: 3, nullable: true),
                    PhoneCode = table.Column<string>(maxLength: 5, nullable: true),
                    CountryCode = table.Column<string>(maxLength: 6, nullable: false),
                    CountryUid = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DepartmentUid = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.DepartmentId);
                });

            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    DistrictId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DistrictNameEn = table.Column<string>(maxLength: 50, nullable: false),
                    DistrictNameSl = table.Column<string>(maxLength: 50, nullable: false),
                    DistrictNameTm = table.Column<string>(maxLength: 50, nullable: false),
                    ProvinceCode = table.Column<int>(nullable: false),
                    DistrictCode = table.Column<int>(nullable: false),
                    DistrictUid = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.DistrictId);
                });

            migrationBuilder.CreateTable(
                name: "MI_Employer_Employees",
                columns: table => new
                {
                    MI_Employer_EmployeeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MI_Employer_EmployeeUid = table.Column<string>(nullable: true),
                    EmployeeCode = table.Column<int>(nullable: true),
                    hrTitle = table.Column<int>(nullable: true),
                    Surname = table.Column<string>(maxLength: 150, nullable: true),
                    Number = table.Column<string>(maxLength: 50, nullable: true),
                    CardNumber = table.Column<int>(nullable: true),
                    Rostered = table.Column<bool>(nullable: false),
                    AssignmentCode = table.Column<int>(nullable: true),
                    GroupNumber = table.Column<int>(nullable: true),
                    Deleted = table.Column<int>(nullable: true),
                    DepartmentCode = table.Column<int>(nullable: true),
                    SectionCode = table.Column<int>(nullable: true),
                    CategoryCode = table.Column<int>(nullable: true),
                    Forenames = table.Column<string>(maxLength: 50, nullable: true),
                    JobTitle = table.Column<string>(maxLength: 50, nullable: true),
                    Salary = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    DateOfAppointment = table.Column<DateTime>(nullable: false),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    NICNumber = table.Column<string>(maxLength: 15, nullable: true),
                    Picture = table.Column<string>(maxLength: 50, nullable: true),
                    SchemeCode = table.Column<int>(nullable: false),
                    OperatorCode = table.Column<int>(nullable: true),
                    TextCardNumber = table.Column<int>(maxLength: 10, nullable: true),
                    Overtime = table.Column<byte>(nullable: false),
                    JulyJune = table.Column<bool>(nullable: false),
                    DesignationCode = table.Column<int>(nullable: true),
                    ReligionCode = table.Column<int>(nullable: true),
                    Married = table.Column<int>(nullable: true),
                    Male = table.Column<int>(nullable: true),
                    Casual = table.Column<bool>(nullable: false),
                    PAddress2 = table.Column<string>(maxLength: 500, nullable: true),
                    PAddress3 = table.Column<string>(maxLength: 50, nullable: true),
                    AnnualTax = table.Column<bool>(nullable: false),
                    MinimumOTPayment = table.Column<decimal>(type: "decimal(19, 2)", nullable: false),
                    EPFNo = table.Column<string>(maxLength: 20, nullable: false),
                    DateDeleted = table.Column<DateTime>(nullable: false),
                    DeletedReason = table.Column<string>(maxLength: 100, nullable: true),
                    UnionMember = table.Column<bool>(nullable: false),
                    ConfirmDate = table.Column<DateTime>(nullable: false),
                    GradeCode = table.Column<int>(nullable: true),
                    DateResigned = table.Column<DateTime>(nullable: false),
                    PageOrder = table.Column<decimal>(type: "decimal(18, 0)", nullable: false),
                    PAddress1 = table.Column<string>(maxLength: 50, nullable: true),
                    PTelephone = table.Column<string>(maxLength: 16, nullable: true),
                    PDistrict = table.Column<int>(nullable: true),
                    RAddress1 = table.Column<string>(maxLength: 150, nullable: true),
                    RAddress2 = table.Column<string>(maxLength: 150, nullable: true),
                    RAddress3 = table.Column<string>(maxLength: 150, nullable: true),
                    RTelephone = table.Column<string>(maxLength: 16, nullable: true),
                    RDistrict = table.Column<int>(nullable: true),
                    hrMobileNo = table.Column<string>(maxLength: 16, nullable: true),
                    CompanyCode = table.Column<int>(nullable: true),
                    PCostCentreCode = table.Column<int>(nullable: true),
                    SCostCentreCode = table.Column<int>(nullable: true),
                    ContactName = table.Column<string>(maxLength: 50, nullable: true),
                    Relation = table.Column<int>(nullable: true),
                    RContAddr1 = table.Column<string>(maxLength: 50, nullable: true),
                    RContAddr2 = table.Column<string>(maxLength: 50, nullable: true),
                    RContAddr3 = table.Column<string>(maxLength: 50, nullable: true),
                    RContTel = table.Column<string>(maxLength: 16, nullable: true),
                    OContAddr1 = table.Column<string>(maxLength: 50, nullable: true),
                    OContAddr2 = table.Column<string>(maxLength: 50, nullable: true),
                    OContAddr3 = table.Column<string>(maxLength: 50, nullable: true),
                    OContTel = table.Column<string>(maxLength: 16, nullable: true),
                    hrContMobile = table.Column<string>(maxLength: 16, nullable: true),
                    hrLiving = table.Column<int>(nullable: true),
                    hrIncome = table.Column<decimal>(type: "decimal(19, 2)", nullable: false),
                    Father = table.Column<string>(maxLength: 50, nullable: true),
                    FAge = table.Column<int>(nullable: true),
                    hrFOccupation = table.Column<string>(maxLength: 50, nullable: true),
                    Mother = table.Column<string>(maxLength: 50, nullable: true),
                    MAge = table.Column<int>(nullable: true),
                    hrMOccupation = table.Column<string>(maxLength: 50, nullable: true),
                    Spouse = table.Column<string>(maxLength: 50, nullable: true),
                    SAge = table.Column<int>(nullable: true),
                    hrSOccupation = table.Column<string>(maxLength: 50, nullable: true),
                    Dependants = table.Column<int>(nullable: true),
                    hrChildren = table.Column<int>(nullable: true),
                    hrPassportNo = table.Column<string>(maxLength: 20, nullable: true),
                    hrExpiryDate = table.Column<DateTime>(nullable: false),
                    hrNationalityCode = table.Column<int>(nullable: true),
                    hrRaceCode = table.Column<int>(nullable: true),
                    hrAge = table.Column<int>(nullable: true),
                    hrDistance = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    hrTravelMode = table.Column<int>(nullable: true),
                    hrODescription = table.Column<string>(maxLength: 50, nullable: true),
                    SectorCode = table.Column<int>(nullable: true),
                    RequisitionId = table.Column<int>(nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    ApplicantCode = table.Column<int>(nullable: true),
                    FirstName = table.Column<string>(maxLength: 200, nullable: true),
                    MiddleName = table.Column<string>(maxLength: 200, nullable: true),
                    LastName = table.Column<string>(maxLength: 200, nullable: true),
                    MSPS = table.Column<bool>(nullable: false),
                    EmploymentType = table.Column<int>(nullable: true),
                    Probation = table.Column<bool>(nullable: false),
                    Location = table.Column<int>(nullable: true),
                    VersionCode = table.Column<int>(nullable: true),
                    AttendAllowGroup = table.Column<int>(nullable: true),
                    hrDOBofSpouse = table.Column<DateTime>(nullable: false),
                    hrDOBofMother = table.Column<DateTime>(nullable: false),
                    hrDOBofFather = table.Column<DateTime>(nullable: false),
                    BloodGroup = table.Column<int>(nullable: true),
                    JoinMethod = table.Column<int>(nullable: true),
                    ReportingDesignation = table.Column<int>(nullable: true),
                    FatherMedEntitle = table.Column<byte>(nullable: false),
                    MotherMedEntitle = table.Column<byte>(nullable: false),
                    SpouseMedEntitle = table.Column<byte>(nullable: false),
                    TransportPerDay = table.Column<float>(nullable: false),
                    Level1Code = table.Column<int>(nullable: true),
                    Level2Code = table.Column<int>(nullable: true),
                    Level3Code = table.Column<int>(nullable: true),
                    Level4Code = table.Column<int>(nullable: true),
                    Level5Code = table.Column<int>(nullable: true),
                    Level6Code = table.Column<int>(nullable: true),
                    Level7Code = table.Column<int>(nullable: true),
                    Level8Code = table.Column<int>(nullable: true),
                    Level9Code = table.Column<int>(nullable: true),
                    Level10Code = table.Column<int>(nullable: true),
                    DriverLicencesNo = table.Column<string>(nullable: true),
                    MadrenName = table.Column<string>(nullable: true),
                    ElecDistrict = table.Column<string>(nullable: true),
                    ServicePhone = table.Column<string>(nullable: true),
                    StaffPhone = table.Column<string>(nullable: true),
                    IEMINo = table.Column<string>(nullable: true),
                    CouplePhone = table.Column<string>(nullable: true),
                    AnyOtherPhone = table.Column<string>(nullable: true),
                    JoinDesignation = table.Column<int>(nullable: true),
                    fEmployer = table.Column<string>(nullable: true),
                    mEmployer = table.Column<string>(nullable: true),
                    sEmployer = table.Column<string>(nullable: true),
                    PoleStationDistance = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    PositionCode = table.Column<int>(nullable: true),
                    EncryptSal = table.Column<string>(nullable: true),
                    OptEmail = table.Column<string>(nullable: true),
                    rptPerson = table.Column<int>(nullable: true),
                    Extension = table.Column<string>(nullable: true),
                    Surname2 = table.Column<string>(nullable: true),
                    OtherSystems = table.Column<string>(nullable: true),
                    Meal = table.Column<bool>(nullable: false),
                    EmpPlanCode = table.Column<int>(nullable: true),
                    ServicePhoneAccNo = table.Column<string>(nullable: true),
                    StaffPhoneAccNo = table.Column<string>(nullable: true),
                    CouplePhoneAccNo = table.Column<string>(nullable: true),
                    PCountry = table.Column<int>(nullable: true),
                    RCountry = table.Column<int>(nullable: true),
                    ECurrency = table.Column<int>(nullable: true),
                    FLive = table.Column<int>(nullable: true),
                    MLive = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: true),
                    Effecteddate = table.Column<DateTime>(nullable: false),
                    Initials = table.Column<string>(nullable: true),
                    Expatriate = table.Column<byte>(nullable: false),
                    Housing = table.Column<string>(nullable: true),
                    Transportation = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(maxLength: 200, nullable: false),
                    NewSalary = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    NameAsIqama = table.Column<string>(nullable: true),
                    StatuName = table.Column<string>(nullable: true),
                    CareerPathID = table.Column<int>(nullable: true),
                    MaidenName = table.Column<string>(nullable: true),
                    UPNname = table.Column<string>(nullable: true),
                    PhysicalLocationCode = table.Column<int>(nullable: true),
                    fNIC = table.Column<string>(nullable: true),
                    mNIC = table.Column<string>(nullable: true),
                    CreditLimit = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    CreditBalance = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Scheme = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MI_Employer_Employees", x => x.MI_Employer_EmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    PaymentMethodId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    MethodType = table.Column<string>(maxLength: 50, nullable: false),
                    PaymentDescription = table.Column<string>(maxLength: 100, nullable: true),
                    PaymentMethodUid = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.PaymentMethodId);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    PaymentUid = table.Column<string>(nullable: false),
                    PaymentType = table.Column<int>(nullable: false),
                    Allowed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentId);
                });

            migrationBuilder.CreateTable(
                name: "Provinces",
                columns: table => new
                {
                    ProvinceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ProvinceCode = table.Column<int>(nullable: false),
                    ProvinceNameEn = table.Column<string>(maxLength: 50, nullable: false),
                    ProvinceNameSl = table.Column<string>(maxLength: 50, nullable: false),
                    ProvinceNameTm = table.Column<string>(maxLength: 50, nullable: false),
                    ProvinceUid = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provinces", x => x.ProvinceId);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    ReportId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ReportName = table.Column<string>(nullable: false),
                    ReportCreateDate = table.Column<DateTime>(nullable: false),
                    ReportPermisionUser = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.ReportId);
                });

            migrationBuilder.CreateTable(
                name: "ReturnReasons",
                columns: table => new
                {
                    ReturnTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ReturnType = table.Column<string>(maxLength: 50, nullable: false),
                    ReturnDescription = table.Column<string>(maxLength: 100, nullable: true),
                    ReturnTypeUid = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnReasons", x => x.ReturnTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    SectionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    SectionUid = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.SectionId);
                });

            migrationBuilder.CreateTable(
                name: "Titles",
                columns: table => new
                {
                    TitleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    TitleName = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 100, nullable: true),
                    TitleUid = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Titles", x => x.TitleId);
                });

            migrationBuilder.CreateTable(
                name: "TypeofGoods",
                columns: table => new
                {
                    TypeofGoodId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    TypeofGoods = table.Column<string>(maxLength: 50, nullable: false),
                    GoodsDescription = table.Column<string>(maxLength: 100, nullable: true),
                    TypeofGoodUid = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeofGoods", x => x.TypeofGoodId);
                });

            migrationBuilder.CreateTable(
                name: "UnitOfMeasurements",
                columns: table => new
                {
                    UnitOfMeasurementId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UnitOfMeasurementUid = table.Column<string>(nullable: false),
                    UOM = table.Column<string>(maxLength: 10, nullable: false),
                    Description = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitOfMeasurements", x => x.UnitOfMeasurementId);
                });

            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    VendorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    VendorUid = table.Column<string>(nullable: false),
                    VendorName = table.Column<string>(maxLength: 50, nullable: false),
                    VendorCode = table.Column<string>(maxLength: 50, nullable: false),
                    VendorDescription = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.VendorId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BranchLocations",
                columns: table => new
                {
                    BranchLocationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    BranchLocationUid = table.Column<string>(nullable: true),
                    BranchCode = table.Column<string>(maxLength: 4, nullable: false),
                    BranchName = table.Column<string>(maxLength: 60, nullable: false),
                    Address = table.Column<string>(maxLength: 200, nullable: false),
                    ContactNumbers = table.Column<string>(maxLength: 15, nullable: false),
                    Longitude = table.Column<string>(maxLength: 50, nullable: true),
                    Latitude = table.Column<string>(maxLength: 50, nullable: true),
                    CityId = table.Column<int>(nullable: true),
                    DistrictId = table.Column<int>(nullable: true),
                    CountryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchLocations", x => x.BranchLocationId);
                    table.ForeignKey(
                        name: "FK_BranchLocations_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "CityId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BranchLocations_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BranchLocations_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "DistrictId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Uid = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: false),
                    Password = table.Column<string>(maxLength: 20, nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    RegDate = table.Column<DateTime>(nullable: false, defaultValueSql: "CONVERT(date, GETDATE())"),
                    SystemNumber = table.Column<string>(maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    LastAccessDate = table.Column<DateTime>(nullable: false),
                    LastConnectedDate = table.Column<DateTime>(nullable: false),
                    LastLoginDate = table.Column<DateTime>(nullable: false),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    NationId = table.Column<string>(maxLength: 100, nullable: true),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    MiddleName = table.Column<string>(maxLength: 50, nullable: true),
                    SurName = table.Column<string>(maxLength: 50, nullable: true),
                    Address = table.Column<string>(maxLength: 200, nullable: true),
                    BirthPlace = table.Column<string>(maxLength: 100, nullable: true),
                    ProfileImageURL = table.Column<string>(maxLength: 400, nullable: true),
                    ContactNumber = table.Column<string>(maxLength: 50, nullable: true),
                    BranchLocationId = table.Column<int>(nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 50, nullable: true),
                    CityId = table.Column<int>(nullable: true),
                    DistrictId = table.Column<int>(nullable: true),
                    CountryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.UniqueConstraint("AK_AspNetUsers_Uid", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_BranchLocations_BranchLocationId",
                        column: x => x.BranchLocationId,
                        principalTable: "BranchLocations",
                        principalColumn: "BranchLocationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "CityId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "DistrictId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CalculationMasters",
                columns: table => new
                {
                    CalculationMasterId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CalculationMasterUid = table.Column<string>(nullable: false),
                    CalculationMasterDescription = table.Column<string>(maxLength: 100, nullable: true),
                    BranchLocationId = table.Column<int>(nullable: false),
                    SalePricePrecentageCompany = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    SalePricePrecentageEmployee = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    ManagerCreditLimitPrecentage = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    StaffCreditLimitPrecentage = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    WorkerCreditLimitPrecentage = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalculationMasters", x => x.CalculationMasterId);
                    table.ForeignKey(
                        name: "FK_CalculationMasters_BranchLocations_BranchLocationId",
                        column: x => x.BranchLocationId,
                        principalTable: "BranchLocations",
                        principalColumn: "BranchLocationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerOrderHeaders",
                columns: table => new
                {
                    CustomerOrderHeaderId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CustomerOrderHeaderUid = table.Column<string>(nullable: false),
                    CustomerOrderNote = table.Column<string>(maxLength: 100, nullable: true),
                    CustomerOrderDate = table.Column<DateTime>(nullable: false),
                    ExpectedDate = table.Column<DateTime>(nullable: false),
                    BranchLocationId = table.Column<int>(nullable: false),
                    IsComfirm = table.Column<bool>(nullable: false),
                    CustomerOrderAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    MI_Employer_EmployeeId = table.Column<int>(nullable: false),
                    EmployeeCode = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerOrderHeaders", x => x.CustomerOrderHeaderId);
                    table.ForeignKey(
                        name: "FK_CustomerOrderHeaders_BranchLocations_BranchLocationId",
                        column: x => x.BranchLocationId,
                        principalTable: "BranchLocations",
                        principalColumn: "BranchLocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerOrderHeaders_MI_Employer_Employees_MI_Employer_EmployeeId",
                        column: x => x.MI_Employer_EmployeeId,
                        principalTable: "MI_Employer_Employees",
                        principalColumn: "MI_Employer_EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerReturnHeaders",
                columns: table => new
                {
                    CustomerReturnHeaderId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CustomerReturnHeaderUid = table.Column<string>(nullable: false),
                    CustomerReturnNote = table.Column<string>(maxLength: 100, nullable: true),
                    CustomerReturnDate = table.Column<DateTime>(nullable: false),
                    BranchLocationId = table.Column<int>(nullable: false),
                    IsProcess = table.Column<bool>(nullable: false),
                    CustomerReturnAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    MI_Employer_EmployeeId = table.Column<int>(nullable: false),
                    EmployeeCode = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerReturnHeaders", x => x.CustomerReturnHeaderId);
                    table.ForeignKey(
                        name: "FK_CustomerReturnHeaders_BranchLocations_BranchLocationId",
                        column: x => x.BranchLocationId,
                        principalTable: "BranchLocations",
                        principalColumn: "BranchLocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerReturnHeaders_MI_Employer_Employees_MI_Employer_EmployeeId",
                        column: x => x.MI_Employer_EmployeeId,
                        principalTable: "MI_Employer_Employees",
                        principalColumn: "MI_Employer_EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseRequestHeaders",
                columns: table => new
                {
                    PurchaseRequestHeaderId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    PurchaseRequestHeaderUid = table.Column<string>(nullable: false),
                    BranchLocationId = table.Column<int>(nullable: false),
                    DepartmentId = table.Column<int>(nullable: false),
                    SectionId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 100, nullable: true),
                    ExpectedDate = table.Column<DateTime>(nullable: false),
                    IsProcess = table.Column<bool>(nullable: false),
                    IsComfirm = table.Column<bool>(nullable: false),
                    PurchaseRequestStatus = table.Column<int>(nullable: false),
                    ApprovedBy = table.Column<string>(maxLength: 100, nullable: true),
                    AuthorizedBy = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseRequestHeaders", x => x.PurchaseRequestHeaderId);
                    table.ForeignKey(
                        name: "FK_PurchaseRequestHeaders_BranchLocations_BranchLocationId",
                        column: x => x.BranchLocationId,
                        principalTable: "BranchLocations",
                        principalColumn: "BranchLocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseRequestHeaders_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseRequestHeaders_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "SectionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockAdjustmentHeaders",
                columns: table => new
                {
                    StockAdjustmentHeaderId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    StockAdjustmentHeaderUid = table.Column<string>(nullable: false),
                    StockAdjustmentsName = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 100, nullable: true),
                    IsProcess = table.Column<bool>(nullable: false),
                    AdjustmentType = table.Column<int>(nullable: false),
                    BranchLocationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockAdjustmentHeaders", x => x.StockAdjustmentHeaderId);
                    table.ForeignKey(
                        name: "FK_StockAdjustmentHeaders_BranchLocations_BranchLocationId",
                        column: x => x.BranchLocationId,
                        principalTable: "BranchLocations",
                        principalColumn: "BranchLocationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    StoreId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    StoreUid = table.Column<string>(nullable: true),
                    StoreName = table.Column<string>(maxLength: 50, nullable: false),
                    StoreDescription = table.Column<string>(maxLength: 100, nullable: false),
                    BranchLocationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.StoreId);
                    table.ForeignKey(
                        name: "FK_Stores_BranchLocations_BranchLocationId",
                        column: x => x.BranchLocationId,
                        principalTable: "BranchLocations",
                        principalColumn: "BranchLocationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    SupplierId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    SupplierUid = table.Column<string>(nullable: false),
                    SupplierName = table.Column<string>(maxLength: 100, nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    Address1 = table.Column<string>(maxLength: 150, nullable: false),
                    Address2 = table.Column<string>(maxLength: 150, nullable: true),
                    Address3 = table.Column<string>(maxLength: 150, nullable: true),
                    Phone = table.Column<string>(maxLength: 15, nullable: true),
                    Mobile = table.Column<string>(maxLength: 15, nullable: true),
                    Fax = table.Column<string>(maxLength: 15, nullable: true),
                    Email = table.Column<string>(maxLength: 1000, nullable: true),
                    URL = table.Column<string>(maxLength: 400, nullable: true),
                    DiscountType = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(maxLength: 200, nullable: true),
                    DiscountAvailable = table.Column<bool>(nullable: false),
                    LogoUrl = table.Column<string>(maxLength: 400, nullable: true),
                    CityId = table.Column<int>(nullable: true),
                    DistrictId = table.Column<int>(nullable: true),
                    CountryId = table.Column<int>(nullable: true),
                    TitleId = table.Column<int>(nullable: true),
                    ProvinceId = table.Column<int>(nullable: true),
                    TypeofGoodId = table.Column<int>(nullable: true),
                    PaymentMethodId = table.Column<int>(nullable: true),
                    BranchLocationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.SupplierId);
                    table.ForeignKey(
                        name: "FK_Suppliers_BranchLocations_BranchLocationId",
                        column: x => x.BranchLocationId,
                        principalTable: "BranchLocations",
                        principalColumn: "BranchLocationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Suppliers_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "CityId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Suppliers_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Suppliers_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "DistrictId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Suppliers_PaymentMethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "PaymentMethods",
                        principalColumn: "PaymentMethodId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Suppliers_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "ProvinceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Suppliers_Titles_TitleId",
                        column: x => x.TitleId,
                        principalTable: "Titles",
                        principalColumn: "TitleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Suppliers_TypeofGoods_TypeofGoodId",
                        column: x => x.TypeofGoodId,
                        principalTable: "TypeofGoods",
                        principalColumn: "TypeofGoodId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SaleHeaders",
                columns: table => new
                {
                    SaleHeaderId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    SaleHeaderUid = table.Column<string>(nullable: false),
                    OrderDate = table.Column<DateTime>(nullable: false),
                    OrderTimeStamp = table.Column<DateTime>(nullable: false),
                    TransactStatus = table.Column<bool>(nullable: false),
                    ErrorLoc = table.Column<string>(maxLength: 200, nullable: true),
                    ErrorMessage = table.Column<string>(maxLength: 100, nullable: true),
                    Paid = table.Column<bool>(nullable: false),
                    PaymentDate = table.Column<DateTime>(nullable: false),
                    PaymentId = table.Column<int>(nullable: false),
                    BranchLocationId = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    MI_Employer_EmployeeId = table.Column<int>(nullable: false),
                    EmployeeCode = table.Column<string>(maxLength: 5, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleHeaders", x => x.SaleHeaderId);
                    table.ForeignKey(
                        name: "FK_SaleHeaders_BranchLocations_BranchLocationId",
                        column: x => x.BranchLocationId,
                        principalTable: "BranchLocations",
                        principalColumn: "BranchLocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaleHeaders_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SaleHeaders_MI_Employer_Employees_MI_Employer_EmployeeId",
                        column: x => x.MI_Employer_EmployeeId,
                        principalTable: "MI_Employer_Employees",
                        principalColumn: "MI_Employer_EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaleHeaders_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "PaymentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockMovementHeaders",
                columns: table => new
                {
                    StockMovementHeaderId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    StockMovementHeaderUid = table.Column<string>(nullable: false),
                    BranchLocationId = table.Column<int>(nullable: false),
                    StoreId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockMovementHeaders", x => x.StockMovementHeaderId);
                    table.ForeignKey(
                        name: "FK_StockMovementHeaders_BranchLocations_BranchLocationId",
                        column: x => x.BranchLocationId,
                        principalTable: "BranchLocations",
                        principalColumn: "BranchLocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockMovementHeaders_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderHeaders",
                columns: table => new
                {
                    PurchaseOrderHeaderId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    PurchaseOrderUid = table.Column<string>(nullable: false),
                    Description = table.Column<string>(maxLength: 100, nullable: true),
                    PoDate = table.Column<DateTime>(nullable: false),
                    DeliveryDate = table.Column<DateTime>(nullable: false),
                    IsProcess = table.Column<bool>(nullable: false),
                    QuatationNo = table.Column<string>(maxLength: 30, nullable: true),
                    IsComfirm = table.Column<bool>(nullable: false),
                    PurchaseAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    BranchLocationId = table.Column<int>(nullable: false),
                    SupplierId = table.Column<int>(nullable: false),
                    PaymentMethodId = table.Column<int>(nullable: false),
                    PaymentId = table.Column<int>(nullable: true),
                    ApprovedBy = table.Column<string>(maxLength: 100, nullable: true),
                    AuthorizedBy = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderHeaders", x => x.PurchaseOrderHeaderId);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderHeaders_BranchLocations_BranchLocationId",
                        column: x => x.BranchLocationId,
                        principalTable: "BranchLocations",
                        principalColumn: "BranchLocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderHeaders_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "PaymentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderHeaders_PaymentMethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "PaymentMethods",
                        principalColumn: "PaymentMethodId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderHeaders_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequestForQuotationHeaders",
                columns: table => new
                {
                    RequestForQuotationHeaderId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    RequestForQuotationHeaderUid = table.Column<string>(nullable: false),
                    BranchLocationId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 200, nullable: true),
                    ExpectedDate = table.Column<DateTime>(nullable: false),
                    IsProcess = table.Column<bool>(nullable: false),
                    SupplierId = table.Column<int>(nullable: false),
                    IsComfirm = table.Column<bool>(nullable: false),
                    SupplierName = table.Column<string>(nullable: false),
                    ApprovedBy = table.Column<string>(maxLength: 100, nullable: true),
                    AuthorizedBy = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestForQuotationHeaders", x => x.RequestForQuotationHeaderId);
                    table.ForeignKey(
                        name: "FK_RequestForQuotationHeaders_BranchLocations_BranchLocationId",
                        column: x => x.BranchLocationId,
                        principalTable: "BranchLocations",
                        principalColumn: "BranchLocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestForQuotationHeaders_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoodReceptNoteHeaders",
                columns: table => new
                {
                    GoodReceptNoteHeaderId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    GoodReceptNoteHeaderUid = table.Column<string>(nullable: false),
                    SupplierInvoiceId = table.Column<string>(maxLength: 30, nullable: false),
                    SupplierInvoiceDate = table.Column<DateTime>(nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    FreeItemsTotalCost = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    TotalDiscountAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    TotalBillAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    SupplierId = table.Column<int>(nullable: true),
                    StoreId = table.Column<long>(nullable: true),
                    BranchLocationId = table.Column<int>(nullable: false),
                    ApplicationUserId = table.Column<int>(nullable: true),
                    PurchaseOrderHeaderId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodReceptNoteHeaders", x => x.GoodReceptNoteHeaderId);
                    table.ForeignKey(
                        name: "FK_GoodReceptNoteHeaders_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GoodReceptNoteHeaders_BranchLocations_BranchLocationId",
                        column: x => x.BranchLocationId,
                        principalTable: "BranchLocations",
                        principalColumn: "BranchLocationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GoodReceptNoteHeaders_PurchaseOrderHeaders_PurchaseOrderHeaderId",
                        column: x => x.PurchaseOrderHeaderId,
                        principalTable: "PurchaseOrderHeaders",
                        principalColumn: "PurchaseOrderHeaderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GoodReceptNoteHeaders_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GoodReceptNoteHeaders_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseReturnHeaders",
                columns: table => new
                {
                    PurchaseReturnHeaderId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    PurchaseReturnHeaderUid = table.Column<string>(nullable: false),
                    SupplierId = table.Column<int>(nullable: false),
                    BranchLocationId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 100, nullable: true),
                    PurchaseReturnDate = table.Column<DateTime>(nullable: false),
                    PurchaseReturnAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    ReturnTypeId = table.Column<int>(nullable: false),
                    PurchaseOrderHeaderId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseReturnHeaders", x => x.PurchaseReturnHeaderId);
                    table.ForeignKey(
                        name: "FK_PurchaseReturnHeaders_BranchLocations_BranchLocationId",
                        column: x => x.BranchLocationId,
                        principalTable: "BranchLocations",
                        principalColumn: "BranchLocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseReturnHeaders_PurchaseOrderHeaders_PurchaseOrderHeaderId",
                        column: x => x.PurchaseOrderHeaderId,
                        principalTable: "PurchaseOrderHeaders",
                        principalColumn: "PurchaseOrderHeaderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseReturnHeaders_ReturnReasons_ReturnTypeId",
                        column: x => x.ReturnTypeId,
                        principalTable: "ReturnReasons",
                        principalColumn: "ReturnTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseReturnHeaders_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockDetails",
                columns: table => new
                {
                    StockDetailId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    StockDetailUid = table.Column<string>(nullable: true),
                    StockQuantity = table.Column<int>(nullable: false),
                    ReorderLevel = table.Column<int>(nullable: false),
                    AlertQuantity = table.Column<int>(nullable: true),
                    BalanceQuantity = table.Column<int>(nullable: false),
                    ProductAvailable = table.Column<bool>(nullable: false),
                    UnitsOnOrder = table.Column<int>(nullable: true),
                    StoreAreaId = table.Column<int>(nullable: false),
                    BranchLocationId = table.Column<int>(nullable: false),
                    StoreId = table.Column<long>(nullable: false),
                    ProductId = table.Column<long>(nullable: false),
                    BinId = table.Column<int>(nullable: false),
                    GoodReceptNoteDetailId = table.Column<long>(nullable: false),
                    PriceId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockDetails", x => x.StockDetailId);
                    table.ForeignKey(
                        name: "FK_StockDetails_BranchLocations_BranchLocationId",
                        column: x => x.BranchLocationId,
                        principalTable: "BranchLocations",
                        principalColumn: "BranchLocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockDetails_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StoreAreas",
                columns: table => new
                {
                    StoreAreaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    StoreAreaUid = table.Column<string>(nullable: false),
                    StoreAreaName = table.Column<string>(maxLength: 50, nullable: false),
                    StoreAreaDescription = table.Column<string>(maxLength: 100, nullable: false),
                    StoreId1 = table.Column<long>(nullable: true),
                    StoreId = table.Column<int>(nullable: false),
                    BranchLocationId = table.Column<int>(nullable: false),
                    StockDetailId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreAreas", x => x.StoreAreaId);
                    table.ForeignKey(
                        name: "FK_StoreAreas_BranchLocations_BranchLocationId",
                        column: x => x.BranchLocationId,
                        principalTable: "BranchLocations",
                        principalColumn: "BranchLocationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoreAreas_StockDetails_StockDetailId",
                        column: x => x.StockDetailId,
                        principalTable: "StockDetails",
                        principalColumn: "StockDetailId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StoreAreas_Stores_StoreId1",
                        column: x => x.StoreId1,
                        principalTable: "Stores",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Racks",
                columns: table => new
                {
                    RackId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    RackName = table.Column<string>(maxLength: 30, nullable: false),
                    RackUid = table.Column<string>(nullable: false),
                    RackDescription = table.Column<string>(maxLength: 100, nullable: false),
                    StoreAreaId = table.Column<int>(nullable: true),
                    NumberofShelves = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Racks", x => x.RackId);
                    table.ForeignKey(
                        name: "FK_Racks_StoreAreas_StoreAreaId",
                        column: x => x.StoreAreaId,
                        principalTable: "StoreAreas",
                        principalColumn: "StoreAreaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockIssueHeaders",
                columns: table => new
                {
                    StockIssueHeaderId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    StockIssueHeaderUid = table.Column<string>(nullable: false),
                    StoreAreaId = table.Column<int>(nullable: false),
                    BranchLocationId = table.Column<int>(nullable: false),
                    StoreId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockIssueHeaders", x => x.StockIssueHeaderId);
                    table.ForeignKey(
                        name: "FK_StockIssueHeaders_BranchLocations_BranchLocationId",
                        column: x => x.BranchLocationId,
                        principalTable: "BranchLocations",
                        principalColumn: "BranchLocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockIssueHeaders_StoreAreas_StoreAreaId",
                        column: x => x.StoreAreaId,
                        principalTable: "StoreAreas",
                        principalColumn: "StoreAreaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockIssueHeaders_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bins",
                columns: table => new
                {
                    BinId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ShelfNumber = table.Column<int>(nullable: false),
                    BinNumber = table.Column<int>(nullable: false),
                    BinUid = table.Column<string>(nullable: false),
                    BinDescription = table.Column<string>(maxLength: 100, nullable: false),
                    BinWidth = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    BintHeight = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    BinLength = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    MaxTolerableWeight = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    BranchLocationId = table.Column<int>(nullable: false),
                    StoreId = table.Column<long>(nullable: false),
                    StoreAreaId = table.Column<int>(nullable: false),
                    RackId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bins", x => x.BinId);
                    table.ForeignKey(
                        name: "FK_Bins_BranchLocations_BranchLocationId",
                        column: x => x.BranchLocationId,
                        principalTable: "BranchLocations",
                        principalColumn: "BranchLocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bins_Racks_RackId",
                        column: x => x.RackId,
                        principalTable: "Racks",
                        principalColumn: "RackId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bins_StoreAreas_StoreAreaId",
                        column: x => x.StoreAreaId,
                        principalTable: "StoreAreas",
                        principalColumn: "StoreAreaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bins_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockIssueDetails",
                columns: table => new
                {
                    StockIssueDetailId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    StockIssueDetailUid = table.Column<string>(nullable: false),
                    IssueQuantity = table.Column<int>(nullable: false),
                    ProductId = table.Column<long>(nullable: false),
                    BinId = table.Column<int>(nullable: false),
                    BinId1 = table.Column<long>(nullable: true),
                    GoodReceptNoteDetailId = table.Column<long>(nullable: false),
                    StockIssueHeaderId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockIssueDetails", x => x.StockIssueDetailId);
                    table.ForeignKey(
                        name: "FK_StockIssueDetails_Bins_BinId1",
                        column: x => x.BinId1,
                        principalTable: "Bins",
                        principalColumn: "BinId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockIssueDetails_StockIssueHeaders_StockIssueHeaderId",
                        column: x => x.StockIssueHeaderId,
                        principalTable: "StockIssueHeaders",
                        principalColumn: "StockIssueHeaderId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerOrderDetails",
                columns: table => new
                {
                    CustomerOrderDetailId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CustomerOrderDetailUid = table.Column<string>(nullable: false),
                    CustomerOrderHeaderId = table.Column<long>(nullable: false),
                    ProductId = table.Column<long>(nullable: false),
                    PriceId = table.Column<long>(nullable: false),
                    BranchLocationId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    CurrentQuantity = table.Column<int>(nullable: true),
                    ReturnQuantity = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerOrderDetails", x => x.CustomerOrderDetailId);
                    table.ForeignKey(
                        name: "FK_CustomerOrderDetails_BranchLocations_BranchLocationId",
                        column: x => x.BranchLocationId,
                        principalTable: "BranchLocations",
                        principalColumn: "BranchLocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerOrderDetails_CustomerOrderHeaders_CustomerOrderHeaderId",
                        column: x => x.CustomerOrderHeaderId,
                        principalTable: "CustomerOrderHeaders",
                        principalColumn: "CustomerOrderHeaderId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerReturnDetails",
                columns: table => new
                {
                    CustomerReturnDetailId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CustomerReturnDetailUid = table.Column<string>(nullable: false),
                    CustomerReturnHeaderId = table.Column<long>(nullable: false),
                    ProductId = table.Column<long>(nullable: false),
                    PriceId = table.Column<long>(nullable: false),
                    BranchLocationId = table.Column<int>(nullable: false),
                    ReturnQuantity = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    CurrentQuantity = table.Column<int>(nullable: true),
                    BalanceQuantity = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerReturnDetails", x => x.CustomerReturnDetailId);
                    table.ForeignKey(
                        name: "FK_CustomerReturnDetails_BranchLocations_BranchLocationId",
                        column: x => x.BranchLocationId,
                        principalTable: "BranchLocations",
                        principalColumn: "BranchLocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerReturnDetails_CustomerReturnHeaders_CustomerReturnHeaderId",
                        column: x => x.CustomerReturnHeaderId,
                        principalTable: "CustomerReturnHeaders",
                        principalColumn: "CustomerReturnHeaderId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    PriceId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    PriceUid = table.Column<string>(nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    MaximumRetailPrice = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    SalePrice = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    DiscountPrice = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    DiscountPrecentage = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    PromotionPrice = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    PromotionPrecentage = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    TaxPrecentage = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    TaXMethod = table.Column<int>(nullable: false),
                    StatusType = table.Column<int>(nullable: false),
                    ProductId = table.Column<long>(nullable: false),
                    BranchLocationId = table.Column<int>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.PriceId);
                    table.UniqueConstraint("AK_Prices_PriceUid", x => x.PriceUid);
                    table.ForeignKey(
                        name: "FK_Prices_BranchLocations_BranchLocationId",
                        column: x => x.BranchLocationId,
                        principalTable: "BranchLocations",
                        principalColumn: "BranchLocationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShopStocks",
                columns: table => new
                {
                    ShopStockId = table.Column<long>(nullable: false),
                    ShopStockUid = table.Column<string>(nullable: false),
                    UnitsInStock = table.Column<int>(nullable: false),
                    ReorderLevel = table.Column<int>(nullable: false),
                    AlertQuantity = table.Column<int>(nullable: true),
                    SaleQuantity = table.Column<int>(nullable: false),
                    ProductAvailable = table.Column<bool>(nullable: false),
                    TransferDate = table.Column<DateTime>(nullable: false),
                    TransferBranchLocationId = table.Column<int>(nullable: true),
                    ProductExpireDate = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    ModifyDate = table.Column<DateTime>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    ProductId = table.Column<long>(nullable: false),
                    BranchLocationId = table.Column<int>(nullable: false),
                    PriceId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopStocks", x => x.ShopStockId);
                    table.ForeignKey(
                        name: "FK_ShopStocks_BranchLocations_BranchLocationId",
                        column: x => x.BranchLocationId,
                        principalTable: "BranchLocations",
                        principalColumn: "BranchLocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopStocks_Prices_ShopStockId",
                        column: x => x.ShopStockId,
                        principalTable: "Prices",
                        principalColumn: "PriceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    StockId = table.Column<long>(nullable: false),
                    StockUid = table.Column<string>(nullable: false),
                    ProductId = table.Column<long>(nullable: false),
                    PriceId = table.Column<long>(nullable: false),
                    UnitsInStock = table.Column<int>(nullable: false),
                    ReorderLevel = table.Column<int>(nullable: false),
                    AlertQuantity = table.Column<int>(nullable: true),
                    BalanceQuantity = table.Column<int>(nullable: false),
                    ProductAvailable = table.Column<bool>(nullable: false),
                    TransferDate = table.Column<DateTime>(nullable: false),
                    TransferBranchLocationId = table.Column<int>(nullable: true),
                    ProductExpireDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    BranchLocationId = table.Column<int>(nullable: false),
                    StoreId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.StockId);
                    table.ForeignKey(
                        name: "FK_Stocks_BranchLocations_BranchLocationId",
                        column: x => x.BranchLocationId,
                        principalTable: "BranchLocations",
                        principalColumn: "BranchLocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stocks_Prices_PriceId",
                        column: x => x.PriceId,
                        principalTable: "Prices",
                        principalColumn: "PriceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Stocks_Stores_StockId",
                        column: x => x.StockId,
                        principalTable: "Stores",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SupplierPrices",
                columns: table => new
                {
                    SupplierPriceId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    SupplierPriceUid = table.Column<string>(nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    MaximumRetailPrice = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    DiscountPrice = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    DiscountPrecentage = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    PromotionPrice = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    PromotionPrecentage = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    TaxPrecentage = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    TaXMethod = table.Column<int>(nullable: false),
                    StatusType = table.Column<int>(nullable: false),
                    ProductId = table.Column<long>(nullable: false),
                    BranchLocationId = table.Column<int>(nullable: false),
                    SupplierId = table.Column<int>(nullable: false),
                    PriceDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierPrices", x => x.SupplierPriceId);
                    table.ForeignKey(
                        name: "FK_SupplierPrices_BranchLocations_BranchLocationId",
                        column: x => x.BranchLocationId,
                        principalTable: "BranchLocations",
                        principalColumn: "BranchLocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupplierPrices_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupplierProducts",
                columns: table => new
                {
                    ProductId = table.Column<long>(nullable: false),
                    SupplierId = table.Column<int>(nullable: false),
                    BranchLocationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierProducts", x => new { x.ProductId, x.SupplierId });
                    table.ForeignKey(
                        name: "FK_SupplierProducts_BranchLocations_BranchLocationId",
                        column: x => x.BranchLocationId,
                        principalTable: "BranchLocations",
                        principalColumn: "BranchLocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SupplierProducts_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ProductUid = table.Column<string>(nullable: false),
                    ExpiryDate = table.Column<DateTime>(nullable: false),
                    Barcode = table.Column<string>(maxLength: 100, nullable: false),
                    ProductName = table.Column<string>(maxLength: 100, nullable: false),
                    ProductDescription = table.Column<string>(maxLength: 200, nullable: true),
                    Size = table.Column<string>(maxLength: 5, nullable: true),
                    Color = table.Column<string>(maxLength: 30, nullable: true),
                    Width = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    Height = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    Length = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    UnitWeight = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    Note = table.Column<string>(maxLength: 100, nullable: true),
                    VendorId = table.Column<int>(nullable: true),
                    CategoryId = table.Column<int>(nullable: true),
                    SubCategoryId = table.Column<int>(nullable: true),
                    UnitOfMeasurementId = table.Column<int>(nullable: true),
                    BrandId = table.Column<int>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    StatusType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.UniqueConstraint("AK_Products_ProductUid", x => x.ProductUid);
                    table.ForeignKey(
                        name: "FK_Products_UnitOfMeasurements_UnitOfMeasurementId",
                        column: x => x.UnitOfMeasurementId,
                        principalTable: "UnitOfMeasurements",
                        principalColumn: "UnitOfMeasurementId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "VendorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GoodReceptNoteDetails",
                columns: table => new
                {
                    GoodReceptNoteDetailId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    GoodReceptNoteDetailUid = table.Column<string>(nullable: true),
                    CostPrice = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    RetailPrice = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    PricePrecentage = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    ItemQuantity = table.Column<int>(nullable: false),
                    BalanceItemQuantity = table.Column<int>(nullable: false),
                    FreeItemQuantity = table.Column<int>(nullable: true),
                    InvoiceItemDate = table.Column<DateTime>(nullable: false),
                    GroupDiscountAmount = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    ProductExpiryDate = table.Column<DateTime>(nullable: false),
                    BatchNumber = table.Column<int>(nullable: false),
                    GoodReceptNoteLine = table.Column<int>(nullable: false),
                    ProductId = table.Column<long>(nullable: false),
                    SupplierPriceId = table.Column<long>(nullable: false),
                    GoodReceptNoteHeaderId = table.Column<long>(nullable: false),
                    PurchaseOrderHeaderId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodReceptNoteDetails", x => x.GoodReceptNoteDetailId);
                    table.ForeignKey(
                        name: "FK_GoodReceptNoteDetails_GoodReceptNoteHeaders_GoodReceptNoteHeaderId",
                        column: x => x.GoodReceptNoteHeaderId,
                        principalTable: "GoodReceptNoteHeaders",
                        principalColumn: "GoodReceptNoteHeaderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoodReceptNoteDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoodReceptNoteDetails_PurchaseOrderHeaders_PurchaseOrderHeaderId",
                        column: x => x.PurchaseOrderHeaderId,
                        principalTable: "PurchaseOrderHeaders",
                        principalColumn: "PurchaseOrderHeaderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoodReceptNoteDetails_SupplierPrices_SupplierPriceId",
                        column: x => x.SupplierPriceId,
                        principalTable: "SupplierPrices",
                        principalColumn: "SupplierPriceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    PhotoId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Url = table.Column<string>(maxLength: 450, nullable: true),
                    Description = table.Column<string>(maxLength: 200, nullable: true),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    IsMain = table.Column<bool>(nullable: false),
                    AttachmentPath = table.Column<string>(maxLength: 200, nullable: false),
                    Length = table.Column<long>(nullable: false),
                    ContentType = table.Column<string>(maxLength: 50, nullable: false),
                    FileName = table.Column<string>(maxLength: 100, nullable: false),
                    ProductId = table.Column<long>(nullable: true),
                    SupplierId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.PhotoId);
                    table.ForeignKey(
                        name: "FK_Photos_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Photos_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderDetails",
                columns: table => new
                {
                    PurchaseOrderDetailId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PurchaseOrderDetailUid = table.Column<string>(nullable: false),
                    PurchaseOrderHeaderId = table.Column<long>(nullable: false),
                    ProductId = table.Column<long>(nullable: false),
                    SupplierPriceId = table.Column<long>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ModifyDate = table.Column<DateTime>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    CurrentQuantity = table.Column<int>(nullable: false),
                    ReturnQuantity = table.Column<int>(nullable: true),
                    BonusQuantity = table.Column<int>(nullable: true),
                    GRNQuantity = table.Column<int>(nullable: true),
                    FreeQuantity = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderDetails", x => x.PurchaseOrderDetailId);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderDetails_PurchaseOrderHeaders_PurchaseOrderHeaderId",
                        column: x => x.PurchaseOrderHeaderId,
                        principalTable: "PurchaseOrderHeaders",
                        principalColumn: "PurchaseOrderHeaderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderDetails_SupplierPrices_SupplierPriceId",
                        column: x => x.SupplierPriceId,
                        principalTable: "SupplierPrices",
                        principalColumn: "SupplierPriceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseReturnDetails",
                columns: table => new
                {
                    PurchaseReturnDetailId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    PurchaseReturnDetailUid = table.Column<string>(nullable: false),
                    PurchaseReturnHeaderId = table.Column<long>(nullable: false),
                    UnitOfMeasurementId = table.Column<int>(nullable: false),
                    ProductId = table.Column<long>(nullable: false),
                    SupplierPriceId = table.Column<long>(nullable: false),
                    Barcode = table.Column<string>(maxLength: 100, nullable: false),
                    ReturnQuantity = table.Column<int>(nullable: false),
                    ReturnPrice = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    FreeQuantity = table.Column<int>(nullable: false),
                    PurchaseOrderHeaderId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseReturnDetails", x => x.PurchaseReturnDetailId);
                    table.ForeignKey(
                        name: "FK_PurchaseReturnDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseReturnDetails_PurchaseOrderHeaders_PurchaseOrderHeaderId",
                        column: x => x.PurchaseOrderHeaderId,
                        principalTable: "PurchaseOrderHeaders",
                        principalColumn: "PurchaseOrderHeaderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseReturnDetails_PurchaseReturnHeaders_PurchaseReturnHeaderId",
                        column: x => x.PurchaseReturnHeaderId,
                        principalTable: "PurchaseReturnHeaders",
                        principalColumn: "PurchaseReturnHeaderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseReturnDetails_SupplierPrices_SupplierPriceId",
                        column: x => x.SupplierPriceId,
                        principalTable: "SupplierPrices",
                        principalColumn: "SupplierPriceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseReturnDetails_UnitOfMeasurements_UnitOfMeasurementId",
                        column: x => x.UnitOfMeasurementId,
                        principalTable: "UnitOfMeasurements",
                        principalColumn: "UnitOfMeasurementId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SaleDetails",
                columns: table => new
                {
                    SaleDetailId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    SaleDetailUid = table.Column<string>(nullable: false),
                    ProductId = table.Column<long>(nullable: false),
                    PriceId = table.Column<long>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    ReturnQuantity = table.Column<int>(nullable: false),
                    BillDate = table.Column<DateTime>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    IsReturn = table.Column<bool>(nullable: false),
                    Paid = table.Column<bool>(nullable: false),
                    SaleHeaderId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleDetails", x => x.SaleDetailId);
                    table.ForeignKey(
                        name: "FK_SaleDetails_Prices_PriceId",
                        column: x => x.PriceId,
                        principalTable: "Prices",
                        principalColumn: "PriceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaleDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaleDetails_SaleHeaders_SaleHeaderId",
                        column: x => x.SaleHeaderId,
                        principalTable: "SaleHeaders",
                        principalColumn: "SaleHeaderId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockAdjustmentDetails",
                columns: table => new
                {
                    StockAdjustmentDetailId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    StockAdjustmentDetailUid = table.Column<string>(nullable: false),
                    StockAdjustmentHeaderId = table.Column<long>(nullable: false),
                    Barcode = table.Column<string>(maxLength: 100, nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    CurrentQuantity = table.Column<int>(nullable: true),
                    ProductId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockAdjustmentDetails", x => x.StockAdjustmentDetailId);
                    table.ForeignKey(
                        name: "FK_StockAdjustmentDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockAdjustmentDetails_StockAdjustmentHeaders_StockAdjustmentHeaderId",
                        column: x => x.StockAdjustmentHeaderId,
                        principalTable: "StockAdjustmentHeaders",
                        principalColumn: "StockAdjustmentHeaderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockMovementDetails",
                columns: table => new
                {
                    StockMovementDetailId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    StockMovementDetailUid = table.Column<string>(nullable: false),
                    ProductId = table.Column<long>(nullable: false),
                    MovementQuantity = table.Column<int>(nullable: false),
                    PreviousQuantity = table.Column<int>(nullable: false),
                    CurrentQuantity = table.Column<int>(nullable: false),
                    StockMovementHeaderId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockMovementDetails", x => x.StockMovementDetailId);
                    table.ForeignKey(
                        name: "FK_StockMovementDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockMovementDetails_StockMovementHeaders_StockMovementHeaderId",
                        column: x => x.StockMovementHeaderId,
                        principalTable: "StockMovementHeaders",
                        principalColumn: "StockMovementHeaderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    BrandId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    BrandName = table.Column<string>(maxLength: 50, nullable: false),
                    BrandDescription = table.Column<string>(maxLength: 100, nullable: true),
                    BrandUid = table.Column<string>(nullable: false),
                    PhotoId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.BrandId);
                    table.UniqueConstraint("AK_Brands_BrandUid", x => x.BrandUid);
                    table.ForeignKey(
                        name: "FK_Brands_Photos_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "Photos",
                        principalColumn: "PhotoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CategoryUid = table.Column<string>(nullable: false),
                    CategoryName = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 100, nullable: true),
                    PhotoId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                    table.ForeignKey(
                        name: "FK_Categories_Photos_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "Photos",
                        principalColumn: "PhotoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseRequestDetails",
                columns: table => new
                {
                    PurchaseRequestDetailId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    PurchaseRequestDetailUid = table.Column<string>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    RejectQuantity = table.Column<int>(nullable: false),
                    StockQuantity = table.Column<int>(nullable: false),
                    BrandId = table.Column<int>(nullable: false),
                    PurchaseRequestHeaderId = table.Column<long>(nullable: false),
                    UnitOfMeasurementId = table.Column<int>(nullable: false),
                    ProductId = table.Column<long>(nullable: false),
                    PurchaseOrderHeaderId = table.Column<long>(nullable: false),
                    IsComfirm = table.Column<bool>(nullable: false),
                    ApprovedQuantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseRequestDetails", x => x.PurchaseRequestDetailId);
                    table.ForeignKey(
                        name: "FK_PurchaseRequestDetails_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseRequestDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseRequestDetails_PurchaseOrderHeaders_PurchaseOrderHeaderId",
                        column: x => x.PurchaseOrderHeaderId,
                        principalTable: "PurchaseOrderHeaders",
                        principalColumn: "PurchaseOrderHeaderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseRequestDetails_PurchaseRequestHeaders_PurchaseRequestHeaderId",
                        column: x => x.PurchaseRequestHeaderId,
                        principalTable: "PurchaseRequestHeaders",
                        principalColumn: "PurchaseRequestHeaderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseRequestDetails_UnitOfMeasurements_UnitOfMeasurementId",
                        column: x => x.UnitOfMeasurementId,
                        principalTable: "UnitOfMeasurements",
                        principalColumn: "UnitOfMeasurementId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequestForQuotationDetails",
                columns: table => new
                {
                    RequestForQuotationDetailId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    RequestForQuotationDetailUid = table.Column<string>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    ProductName = table.Column<string>(maxLength: 50, nullable: false),
                    ProductDescription = table.Column<string>(maxLength: 100, nullable: false),
                    Size = table.Column<string>(maxLength: 5, nullable: true),
                    Color = table.Column<string>(maxLength: 30, nullable: true),
                    UnitWeight = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    Note = table.Column<string>(maxLength: 200, nullable: true),
                    BrandId = table.Column<int>(nullable: true),
                    UnitOfMeasurementId = table.Column<int>(nullable: true),
                    ProductId = table.Column<long>(nullable: false),
                    RequestForQuotationHeaderId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestForQuotationDetails", x => x.RequestForQuotationDetailId);
                    table.ForeignKey(
                        name: "FK_RequestForQuotationDetails_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestForQuotationDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestForQuotationDetails_RequestForQuotationHeaders_RequestForQuotationHeaderId",
                        column: x => x.RequestForQuotationHeaderId,
                        principalTable: "RequestForQuotationHeaders",
                        principalColumn: "RequestForQuotationHeaderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestForQuotationDetails_UnitOfMeasurements_UnitOfMeasurementId",
                        column: x => x.UnitOfMeasurementId,
                        principalTable: "UnitOfMeasurements",
                        principalColumn: "UnitOfMeasurementId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    ProductId = table.Column<long>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    ProductCategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => new { x.ProductId, x.CategoryId });
                    table.UniqueConstraint("AK_ProductCategories_ProductCategoryId", x => x.ProductCategoryId);
                    table.ForeignKey(
                        name: "FK_ProductCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCategories_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubCategories",
                columns: table => new
                {
                    SubCategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    SubCategoryUid = table.Column<string>(nullable: false),
                    SubCategoryName = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 100, nullable: true),
                    PictureUrl = table.Column<string>(maxLength: 450, nullable: true),
                    CategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategories", x => x.SubCategoryId);
                    table.ForeignKey(
                        name: "FK_SubCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductSubCategories",
                columns: table => new
                {
                    ProductSubCategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<int>(nullable: false),
                    ProductId1 = table.Column<long>(nullable: true),
                    SubCategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSubCategories", x => x.ProductSubCategoryId);
                    table.ForeignKey(
                        name: "FK_ProductSubCategories_Products_ProductId1",
                        column: x => x.ProductId1,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductSubCategories_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategories",
                        principalColumn: "SubCategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BranchLocationId",
                table: "AspNetUsers",
                column: "BranchLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CityId",
                table: "AspNetUsers",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CountryId",
                table: "AspNetUsers",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DistrictId",
                table: "AspNetUsers",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Bins_BranchLocationId",
                table: "Bins",
                column: "BranchLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Bins_RackId",
                table: "Bins",
                column: "RackId");

            migrationBuilder.CreateIndex(
                name: "IX_Bins_StoreAreaId",
                table: "Bins",
                column: "StoreAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Bins_StoreId",
                table: "Bins",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchLocations_CityId",
                table: "BranchLocations",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchLocations_CountryId",
                table: "BranchLocations",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchLocations_DistrictId",
                table: "BranchLocations",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Brands_PhotoId",
                table: "Brands",
                column: "PhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_CalculationMasters_BranchLocationId",
                table: "CalculationMasters",
                column: "BranchLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_PhotoId",
                table: "Categories",
                column: "PhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrderDetails_BranchLocationId",
                table: "CustomerOrderDetails",
                column: "BranchLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrderDetails_CustomerOrderHeaderId",
                table: "CustomerOrderDetails",
                column: "CustomerOrderHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrderDetails_PriceId",
                table: "CustomerOrderDetails",
                column: "PriceId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrderDetails_ProductId",
                table: "CustomerOrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrderHeaders_BranchLocationId",
                table: "CustomerOrderHeaders",
                column: "BranchLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrderHeaders_MI_Employer_EmployeeId",
                table: "CustomerOrderHeaders",
                column: "MI_Employer_EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerReturnDetails_BranchLocationId",
                table: "CustomerReturnDetails",
                column: "BranchLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerReturnDetails_CustomerReturnHeaderId",
                table: "CustomerReturnDetails",
                column: "CustomerReturnHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerReturnDetails_PriceId",
                table: "CustomerReturnDetails",
                column: "PriceId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerReturnDetails_ProductId",
                table: "CustomerReturnDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerReturnHeaders_BranchLocationId",
                table: "CustomerReturnHeaders",
                column: "BranchLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerReturnHeaders_MI_Employer_EmployeeId",
                table: "CustomerReturnHeaders",
                column: "MI_Employer_EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodReceptNoteDetails_GoodReceptNoteHeaderId",
                table: "GoodReceptNoteDetails",
                column: "GoodReceptNoteHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodReceptNoteDetails_ProductId",
                table: "GoodReceptNoteDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodReceptNoteDetails_PurchaseOrderHeaderId",
                table: "GoodReceptNoteDetails",
                column: "PurchaseOrderHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodReceptNoteDetails_SupplierPriceId",
                table: "GoodReceptNoteDetails",
                column: "SupplierPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodReceptNoteHeaders_ApplicationUserId",
                table: "GoodReceptNoteHeaders",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodReceptNoteHeaders_BranchLocationId",
                table: "GoodReceptNoteHeaders",
                column: "BranchLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodReceptNoteHeaders_PurchaseOrderHeaderId",
                table: "GoodReceptNoteHeaders",
                column: "PurchaseOrderHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodReceptNoteHeaders_StoreId",
                table: "GoodReceptNoteHeaders",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodReceptNoteHeaders_SupplierId",
                table: "GoodReceptNoteHeaders",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_ProductId",
                table: "Photos",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_SupplierId",
                table: "Photos",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_BranchLocationId",
                table: "Prices",
                column: "BranchLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_ProductId",
                table: "Prices",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_CategoryId",
                table: "ProductCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId",
                table: "Products",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SubCategoryId",
                table: "Products",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_UnitOfMeasurementId",
                table: "Products",
                column: "UnitOfMeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_VendorId",
                table: "Products",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSubCategories_ProductId1",
                table: "ProductSubCategories",
                column: "ProductId1");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSubCategories_SubCategoryId",
                table: "ProductSubCategories",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDetails_ProductId",
                table: "PurchaseOrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDetails_PurchaseOrderHeaderId",
                table: "PurchaseOrderDetails",
                column: "PurchaseOrderHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDetails_SupplierPriceId",
                table: "PurchaseOrderDetails",
                column: "SupplierPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderHeaders_BranchLocationId",
                table: "PurchaseOrderHeaders",
                column: "BranchLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderHeaders_PaymentId",
                table: "PurchaseOrderHeaders",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderHeaders_PaymentMethodId",
                table: "PurchaseOrderHeaders",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderHeaders_SupplierId",
                table: "PurchaseOrderHeaders",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequestDetails_BrandId",
                table: "PurchaseRequestDetails",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequestDetails_ProductId",
                table: "PurchaseRequestDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequestDetails_PurchaseOrderHeaderId",
                table: "PurchaseRequestDetails",
                column: "PurchaseOrderHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequestDetails_PurchaseRequestHeaderId",
                table: "PurchaseRequestDetails",
                column: "PurchaseRequestHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequestDetails_UnitOfMeasurementId",
                table: "PurchaseRequestDetails",
                column: "UnitOfMeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequestHeaders_BranchLocationId",
                table: "PurchaseRequestHeaders",
                column: "BranchLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequestHeaders_DepartmentId",
                table: "PurchaseRequestHeaders",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequestHeaders_SectionId",
                table: "PurchaseRequestHeaders",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReturnDetails_ProductId",
                table: "PurchaseReturnDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReturnDetails_PurchaseOrderHeaderId",
                table: "PurchaseReturnDetails",
                column: "PurchaseOrderHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReturnDetails_PurchaseReturnHeaderId",
                table: "PurchaseReturnDetails",
                column: "PurchaseReturnHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReturnDetails_SupplierPriceId",
                table: "PurchaseReturnDetails",
                column: "SupplierPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReturnDetails_UnitOfMeasurementId",
                table: "PurchaseReturnDetails",
                column: "UnitOfMeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReturnHeaders_BranchLocationId",
                table: "PurchaseReturnHeaders",
                column: "BranchLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReturnHeaders_PurchaseOrderHeaderId",
                table: "PurchaseReturnHeaders",
                column: "PurchaseOrderHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReturnHeaders_ReturnTypeId",
                table: "PurchaseReturnHeaders",
                column: "ReturnTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReturnHeaders_SupplierId",
                table: "PurchaseReturnHeaders",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Racks_StoreAreaId",
                table: "Racks",
                column: "StoreAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestForQuotationDetails_BrandId",
                table: "RequestForQuotationDetails",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestForQuotationDetails_ProductId",
                table: "RequestForQuotationDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestForQuotationDetails_RequestForQuotationHeaderId",
                table: "RequestForQuotationDetails",
                column: "RequestForQuotationHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestForQuotationDetails_UnitOfMeasurementId",
                table: "RequestForQuotationDetails",
                column: "UnitOfMeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestForQuotationHeaders_BranchLocationId",
                table: "RequestForQuotationHeaders",
                column: "BranchLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestForQuotationHeaders_SupplierId",
                table: "RequestForQuotationHeaders",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleDetails_PriceId",
                table: "SaleDetails",
                column: "PriceId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleDetails_ProductId",
                table: "SaleDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleDetails_SaleHeaderId",
                table: "SaleDetails",
                column: "SaleHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleHeaders_BranchLocationId",
                table: "SaleHeaders",
                column: "BranchLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleHeaders_Id",
                table: "SaleHeaders",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SaleHeaders_MI_Employer_EmployeeId",
                table: "SaleHeaders",
                column: "MI_Employer_EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleHeaders_PaymentId",
                table: "SaleHeaders",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopStocks_BranchLocationId",
                table: "ShopStocks",
                column: "BranchLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopStocks_ProductId",
                table: "ShopStocks",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockAdjustmentDetails_ProductId",
                table: "StockAdjustmentDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockAdjustmentDetails_StockAdjustmentHeaderId",
                table: "StockAdjustmentDetails",
                column: "StockAdjustmentHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_StockAdjustmentHeaders_BranchLocationId",
                table: "StockAdjustmentHeaders",
                column: "BranchLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_StockDetails_BranchLocationId",
                table: "StockDetails",
                column: "BranchLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_StockDetails_GoodReceptNoteDetailId",
                table: "StockDetails",
                column: "GoodReceptNoteDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_StockDetails_PriceId",
                table: "StockDetails",
                column: "PriceId");

            migrationBuilder.CreateIndex(
                name: "IX_StockDetails_ProductId",
                table: "StockDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockDetails_StoreId",
                table: "StockDetails",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StockIssueDetails_BinId1",
                table: "StockIssueDetails",
                column: "BinId1");

            migrationBuilder.CreateIndex(
                name: "IX_StockIssueDetails_GoodReceptNoteDetailId",
                table: "StockIssueDetails",
                column: "GoodReceptNoteDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_StockIssueDetails_ProductId",
                table: "StockIssueDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockIssueDetails_StockIssueHeaderId",
                table: "StockIssueDetails",
                column: "StockIssueHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_StockIssueHeaders_BranchLocationId",
                table: "StockIssueHeaders",
                column: "BranchLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_StockIssueHeaders_StoreAreaId",
                table: "StockIssueHeaders",
                column: "StoreAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_StockIssueHeaders_StoreId",
                table: "StockIssueHeaders",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovementDetails_ProductId",
                table: "StockMovementDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovementDetails_StockMovementHeaderId",
                table: "StockMovementDetails",
                column: "StockMovementHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovementHeaders_BranchLocationId",
                table: "StockMovementHeaders",
                column: "BranchLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovementHeaders_StoreId",
                table: "StockMovementHeaders",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_BranchLocationId",
                table: "Stocks",
                column: "BranchLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_PriceId",
                table: "Stocks",
                column: "PriceId");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_ProductId",
                table: "Stocks",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreAreas_BranchLocationId",
                table: "StoreAreas",
                column: "BranchLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreAreas_StockDetailId",
                table: "StoreAreas",
                column: "StockDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreAreas_StoreId1",
                table: "StoreAreas",
                column: "StoreId1");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_BranchLocationId",
                table: "Stores",
                column: "BranchLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_CategoryId",
                table: "SubCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierPrices_BranchLocationId",
                table: "SupplierPrices",
                column: "BranchLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierPrices_ProductId",
                table: "SupplierPrices",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierPrices_SupplierId",
                table: "SupplierPrices",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierProducts_BranchLocationId",
                table: "SupplierProducts",
                column: "BranchLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierProducts_SupplierId",
                table: "SupplierProducts",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_BranchLocationId",
                table: "Suppliers",
                column: "BranchLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_CityId",
                table: "Suppliers",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_CountryId",
                table: "Suppliers",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_DistrictId",
                table: "Suppliers",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_PaymentMethodId",
                table: "Suppliers",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_ProvinceId",
                table: "Suppliers",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_TitleId",
                table: "Suppliers",
                column: "TitleId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_TypeofGoodId",
                table: "Suppliers",
                column: "TypeofGoodId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockDetails_Prices_PriceId",
                table: "StockDetails",
                column: "PriceId",
                principalTable: "Prices",
                principalColumn: "PriceId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockDetails_Products_ProductId",
                table: "StockDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockDetails_GoodReceptNoteDetails_GoodReceptNoteDetailId",
                table: "StockDetails",
                column: "GoodReceptNoteDetailId",
                principalTable: "GoodReceptNoteDetails",
                principalColumn: "GoodReceptNoteDetailId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockDetails_Bins_PriceId",
                table: "StockDetails",
                column: "PriceId",
                principalTable: "Bins",
                principalColumn: "BinId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockIssueDetails_Products_ProductId",
                table: "StockIssueDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockIssueDetails_GoodReceptNoteDetails_GoodReceptNoteDetailId",
                table: "StockIssueDetails",
                column: "GoodReceptNoteDetailId",
                principalTable: "GoodReceptNoteDetails",
                principalColumn: "GoodReceptNoteDetailId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerOrderDetails_Prices_PriceId",
                table: "CustomerOrderDetails",
                column: "PriceId",
                principalTable: "Prices",
                principalColumn: "PriceId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerOrderDetails_Products_ProductId",
                table: "CustomerOrderDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerReturnDetails_Prices_PriceId",
                table: "CustomerReturnDetails",
                column: "PriceId",
                principalTable: "Prices",
                principalColumn: "PriceId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerReturnDetails_Products_ProductId",
                table: "CustomerReturnDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prices_Products_ProductId",
                table: "Prices",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopStocks_Products_ProductId",
                table: "ShopStocks",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Products_ProductId",
                table: "Stocks",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierPrices_Products_ProductId",
                table: "SupplierPrices",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierProducts_Products_ProductId",
                table: "SupplierProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "BrandId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_SubCategories_SubCategoryId",
                table: "Products",
                column: "SubCategoryId",
                principalTable: "SubCategories",
                principalColumn: "SubCategoryId",
                onDelete: ReferentialAction.Restrict); */
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
          /*   migrationBuilder.DropForeignKey(
                name: "FK_GoodReceptNoteHeaders_AspNetUsers_ApplicationUserId",
                table: "GoodReceptNoteHeaders");

            migrationBuilder.DropForeignKey(
                name: "FK_Bins_BranchLocations_BranchLocationId",
                table: "Bins");

            migrationBuilder.DropForeignKey(
                name: "FK_GoodReceptNoteHeaders_BranchLocations_BranchLocationId",
                table: "GoodReceptNoteHeaders");

            migrationBuilder.DropForeignKey(
                name: "FK_Prices_BranchLocations_BranchLocationId",
                table: "Prices");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderHeaders_BranchLocations_BranchLocationId",
                table: "PurchaseOrderHeaders");

            migrationBuilder.DropForeignKey(
                name: "FK_StockDetails_BranchLocations_BranchLocationId",
                table: "StockDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreAreas_BranchLocations_BranchLocationId",
                table: "StoreAreas");

            migrationBuilder.DropForeignKey(
                name: "FK_Stores_BranchLocations_BranchLocationId",
                table: "Stores");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplierPrices_BranchLocations_BranchLocationId",
                table: "SupplierPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_BranchLocations_BranchLocationId",
                table: "Suppliers");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_Cities_CityId",
                table: "Suppliers");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_Countries_CountryId",
                table: "Suppliers");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_Districts_DistrictId",
                table: "Suppliers");

            migrationBuilder.DropForeignKey(
                name: "FK_Bins_Racks_RackId",
                table: "Bins");

            migrationBuilder.DropForeignKey(
                name: "FK_Bins_StoreAreas_StoreAreaId",
                table: "Bins");

            migrationBuilder.DropForeignKey(
                name: "FK_Brands_Photos_PhotoId",
                table: "Brands");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Photos_PhotoId",
                table: "Categories");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CalculationMasters");

            migrationBuilder.DropTable(
                name: "CustomerOrderDetails");

            migrationBuilder.DropTable(
                name: "CustomerReturnDetails");

            migrationBuilder.DropTable(
                name: "ProductCategories");

            migrationBuilder.DropTable(
                name: "ProductSubCategories");

            migrationBuilder.DropTable(
                name: "PurchaseOrderDetails");

            migrationBuilder.DropTable(
                name: "PurchaseRequestDetails");

            migrationBuilder.DropTable(
                name: "PurchaseReturnDetails");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "RequestForQuotationDetails");

            migrationBuilder.DropTable(
                name: "SaleDetails");

            migrationBuilder.DropTable(
                name: "ShopStocks");

            migrationBuilder.DropTable(
                name: "StockAdjustmentDetails");

            migrationBuilder.DropTable(
                name: "StockIssueDetails");

            migrationBuilder.DropTable(
                name: "StockMovementDetails");

            migrationBuilder.DropTable(
                name: "Stocks");

            migrationBuilder.DropTable(
                name: "SupplierProducts");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "CustomerOrderHeaders");

            migrationBuilder.DropTable(
                name: "CustomerReturnHeaders");

            migrationBuilder.DropTable(
                name: "PurchaseRequestHeaders");

            migrationBuilder.DropTable(
                name: "PurchaseReturnHeaders");

            migrationBuilder.DropTable(
                name: "RequestForQuotationHeaders");

            migrationBuilder.DropTable(
                name: "SaleHeaders");

            migrationBuilder.DropTable(
                name: "StockAdjustmentHeaders");

            migrationBuilder.DropTable(
                name: "StockIssueHeaders");

            migrationBuilder.DropTable(
                name: "StockMovementHeaders");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropTable(
                name: "ReturnReasons");

            migrationBuilder.DropTable(
                name: "MI_Employer_Employees");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "BranchLocations");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Districts");

            migrationBuilder.DropTable(
                name: "Racks");

            migrationBuilder.DropTable(
                name: "StoreAreas");

            migrationBuilder.DropTable(
                name: "StockDetails");

            migrationBuilder.DropTable(
                name: "GoodReceptNoteDetails");

            migrationBuilder.DropTable(
                name: "Bins");

            migrationBuilder.DropTable(
                name: "Prices");

            migrationBuilder.DropTable(
                name: "GoodReceptNoteHeaders");

            migrationBuilder.DropTable(
                name: "SupplierPrices");

            migrationBuilder.DropTable(
                name: "PurchaseOrderHeaders");

            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "SubCategories");

            migrationBuilder.DropTable(
                name: "UnitOfMeasurements");

            migrationBuilder.DropTable(
                name: "Vendors");

            migrationBuilder.DropTable(
                name: "PaymentMethods");

            migrationBuilder.DropTable(
                name: "Provinces");

            migrationBuilder.DropTable(
                name: "Titles");

            migrationBuilder.DropTable(
                name: "TypeofGoods");

            migrationBuilder.DropTable(
                name: "Categories"); */
        }
    }
}
