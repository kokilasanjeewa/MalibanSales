using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Models
{
  public class Customer
  {
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string Surname { get; set; }
    public string Number { get; set; }
    public string TextCardNumber { get; set; }

  }
}
/*EmployeeCode	int	Unchecked
hrTitle	int	Unchecked
Surname	varchar(150)	Unchecked
Number	varchar(50)	Unchecked
CardNumber	int	Unchecked
Rostered	bit	Unchecked
AssignmentCode	int	Unchecked
GroupNumber	int	Unchecked
Deleted	tinyint	Unchecked
DepartmentCode	int	Unchecked
SectionCode	int	Unchecked
CategoryCode	int	Unchecked
Forenames	varchar(50)	Unchecked
JobTitle	varchar(50)	Unchecked
Salary	numeric(19, 2)	Unchecked
DateOfAppointment	datetime	Unchecked
DateOfBirth	datetime	Unchecked
NICNumber	varchar(15)	Unchecked
Picture	varchar(50)	Unchecked
SchemeCode	int	Unchecked
OperatorCode	int	Unchecked
TextCardNumber	varchar(10)	Unchecked
Overtime	tinyint	Unchecked
JulyJune	bit	Unchecked
DesignationCode	int	Unchecked
ReligionCode	int	Unchecked
Married	int	Unchecked
Male	tinyint	Unchecked
Casual	bit	Unchecked
PAddress2	varchar(500)	Unchecked
PAddress3	varchar(50)	Unchecked
AnnualTax	bit	Unchecked
MinimumOTPayment	numeric(19, 2)	Unchecked
EPFNo	varchar(20)	Unchecked
DateDeleted	datetime	Unchecked
DeletedReason	varchar(100)	Unchecked
UnionMember	bit	Unchecked
ConfirmDate	datetime	Unchecked
GradeCode	int	Unchecked
DateResigned	datetime	Unchecked
PageOrder	numeric(18, 0)	Unchecked
PAddress1	varchar(50)	Unchecked
PTelephone	varchar(16)	Unchecked
PDistrict	int	Unchecked
RAddress1	varchar(150)	Unchecked
RAddress2	varchar(150)	Unchecked
RAddress3	varchar(150)	Unchecked
RTelephone	varchar(16)	Unchecked
RDistrict	int	Unchecked
hrMobileNo	varchar(16)	Unchecked
CompanyCode	int	Unchecked
PCostCentreCode	int	Unchecked
SCostCentreCode	int	Unchecked
ContactName	varchar(50)	Unchecked
Relation	int	Unchecked
RContAddr1	varchar(50)	Unchecked
RContAddr2	varchar(50)	Unchecked
RContAddr3	varchar(50)	Unchecked
RContTel	varchar(16)	Unchecked
OContAddr1	varchar(50)	Unchecked
OContAddr2	varchar(50)	Unchecked
OContAddr3	varchar(50)	Unchecked
OContTel	varchar(16)	Unchecked
hrContMobile	varchar(16)	Unchecked
hrLiving	int	Unchecked
hrIncome	numeric(19, 2)	Unchecked
Father	varchar(50)	Unchecked
FAge	int	Unchecked
hrFOccupation	varchar(50)	Unchecked
Mother	varchar(50)	Unchecked
MAge	int	Unchecked
hrMOccupation	varchar(50)	Unchecked
Spouse	varchar(50)	Unchecked
SAge	int	Unchecked
hrSOccupation	varchar(50)	Unchecked
Dependants	int	Unchecked
hrChildren	int	Unchecked
hrPassportNo	varchar(20)	Unchecked
hrExpiryDate	datetime	Unchecked
hrNationalityCode	int	Unchecked
hrRaceCode	int	Unchecked
hrAge	int	Unchecked
hrDistance	decimal(18, 2)	Unchecked
hrTravelMode	int	Unchecked
hrODescription	varchar(50)	Unchecked
SectorCode	int	Unchecked
RequisitionId	int	Unchecked
Email	varchar(100)	Unchecked
ApplicantCode	int	Unchecked
FirstName	varchar(200)	Unchecked
MiddleName	varchar(200)	Unchecked
LastName	varchar(200)	Unchecked
MSPS	bit	Unchecked
EmploymentType	int	Unchecked
Probation	bit	Unchecked
Location	int	Unchecked
VersionCode	int	Unchecked
AttendAllowGroup	int	Unchecked
hrDOBofSpouse	datetime	Unchecked
hrDOBofMother	datetime	Unchecked
hrDOBofFather	datetime	Unchecked
BloodGroup	int	Unchecked
JoinMethod	int	Unchecked
ReportingDesignation	int	Unchecked
FatherMedEntitle	bit	Unchecked
MotherMedEntitle	bit	Unchecked
SpouseMedEntitle	bit	Unchecked
TransportPerDay	real	Unchecked
Level1Code	int	Unchecked
Level2Code	int	Unchecked
Level3Code	int	Unchecked
Level4Code	int	Unchecked
Level5Code	int	Unchecked
Level6Code	int	Unchecked
Level7Code	int	Unchecked
Level8Code	int	Unchecked
Level9Code	int	Unchecked
Level10Code	int	Unchecked
DriverLicencesNo	varchar(50)	Unchecked
MadrenName	varchar(200)	Unchecked
ElecDistrict	varchar(200)	Unchecked
ServicePhone	varchar(50)	Unchecked
StaffPhone	varchar(50)	Unchecked
IEMINo	varchar(50)	Unchecked
CouplePhone	varchar(50)	Unchecked
AnyOtherPhone	varchar(100)	Unchecked
JoinDesignation	int	Unchecked
fEmployer	varchar(200)	Unchecked
mEmployer	varchar(200)	Unchecked
sEmployer	varchar(200)	Unchecked
PoleStationDistance	decimal(18, 2)	Unchecked
PositionCode	int	Unchecked
EncryptSal	varchar(50)	Unchecked
OptEmail	varchar(100)	Unchecked
rptPerson	int	Unchecked
Extension	varchar(30)	Unchecked
Surname2	varchar(100)	Unchecked
OtherSystems	varchar(200)	Unchecked
Meal	bit	Unchecked
EmpPlanCode	int	Unchecked
ServicePhoneAccNo	varchar(50)	Unchecked
StaffPhoneAccNo	varchar(50)	Unchecked
CouplePhoneAccNo	varchar(50)	Unchecked
PCountry	int	Unchecked
RCountry	int	Unchecked
ECurrency	int	Unchecked
FLive	int	Unchecked
MLive	int	Unchecked
Status	tinyint	Unchecked
Effecteddate	datetime	Unchecked
Initials	nvarchar(MAX)	Unchecked
Expatriate	bit	Unchecked
Housing	varchar(50)	Unchecked
Transportation	varchar(50)	Unchecked
FullName	nvarchar(200)	Unchecked
NewSalary	money	Unchecked
NameAsIqama	nvarchar(MAX)	Unchecked
StatuName	nvarchar(200)	Unchecked
CareerPathID	int	Checked
MaidenName	varchar(500)	Unchecked
UPNname	varchar(500)	Unchecked
PhysicalLocationCode	int	Unchecked
fNIC	varchar(10)	Unchecked
mNIC	varchar(10)	Unchecked*/

