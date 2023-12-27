import { BaseModel } from "./basemodel";

/* export interface GetTransactionByItemCodeDto extends BaseModel {
  transactionDetailId: number;
  dQuantity: number;
  branchLocationId: number;
  iDocID: number;
  cDocNo: string;
  dDocDate: string;
  productExpireDate: string;
  cDocType: cDocType;
  cExtRefNum: string;
  dIssueQuantity: number;
  dNetQuantity: number;
  dCostPrice: number;
  dSalePrice: number;
  dValue: number;
  dNetValue: number;
  dIN: number;
  dOUT: number;
  barcode: string;
  productName: string;
  dPreNetQuantity: number;
  saleEmployeeCode: string;
  saleReturnEmployeeCode: string;
} */
enum cDocType {
  GRN = 0,
  INV,
  AIN,
  PRN,
  SLR,
}
export interface GetTransactionByItemCodeDto extends BaseModel {
  productName: string;
  productCode: string;
  receivedDate: string;
  receivedUnitPrice: number;
  unitSellingprice: number;
  receivedQTY: number;
  productExpireDate: string;
  employeeNo: string;
  issuedDate: string;
  issuingUnitPrice: number;
  unitSelling: number;
  issuingQty: number;
  balance: number;
}
export interface GetBinByItemCodeDto extends BaseModel {
  transactionDetailId: number;
  tSerialNo:number;
  productName: string;
  tRefId: number;
  saleDetailId: number;
  productCode: string;
  tType: string;
  tiType: cDocType;
  receivedDate: string;
  receivedUnitPrice: number;
  unitSellingprice: number;
  receiveQty: number;
  productExpireDate: string;
  employeeNo: string;
  issuedDate: string;
  issuedUnitPrice: number;
  issuedSoldPrice: number;
  issuedQty: number;
  tNetQty: number;
  balance: number;
  paymentType:string;
}
