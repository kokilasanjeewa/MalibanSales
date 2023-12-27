export interface SaleSummaryReportDetail {
  saleHeaderId: number;
  orderDate: string;
  paid: boolean;
  paymentDate: string;
  paymentType:string
  employeeCode: string;
  totalAmount: number;
  customerReturnAmount: number;
  branchCode: string;
}
