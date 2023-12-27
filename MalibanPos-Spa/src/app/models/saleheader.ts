import { MI_Employer_Employee } from './mI_employer_employee';
import { BaseModel } from './basemodel';
import { SaleDetail } from './saledetail';

export interface SaleHeader extends BaseModel {
  saleHeaderId: number;
  saleHeaderUid: string;
  orderDate: string;
  orderTimeStamp: string;
  transactStatus: boolean;
  errorLoc: string;
  errorMessage: string;
  lines: SaleDetail[];
  mI_Employer_EmployeeId: number;
  employeeCode: string;
  mI_Employer_Employee: MI_Employer_Employee;
  totalAmount: number;
}
