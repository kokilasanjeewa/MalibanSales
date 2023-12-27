export interface SaleReportDetail {
  itemCode: string;
  productName: string;
  cashPrice: number;
  qTY: number;
  saleValue: number;
  cost: number;
  stockValue: number;
  saleHeaderId: number;
  createDate: Date;
  paymentType:string;
}
