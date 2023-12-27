export interface EmployeeDto {
  mI_Employer_EmployeeId: number;
  mI_Employer_EmployeeUid: string;
  number: string;
  deleted: number | null;
  dateOfAppointment: string;
  schemeCode: number | null;
  ePFNo: string;
  dateResigned: string;
  fullName: string;
  creditLimit: number;
  creditBalance: number;
  scheme: string;
  previousCreditBalance: number;
  location: string;
  mapLocation: string;
}
