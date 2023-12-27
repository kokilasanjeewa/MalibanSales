export interface SaleReportDetailByCustomer {
  itemCode: string;
  productName: string;
  cashPrice: number;
  qTY: number;
  returnQTY: number;
  saleValue: number;
  cost: number;
  stockValue: number;
  productExpireDate: string;
  saleHeaderId: number;
  createDate: string;
  branchCode: string;
  paymentType:string;
}
