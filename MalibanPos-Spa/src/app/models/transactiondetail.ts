import { BaseModel } from './basemodel';
import { BranchLocation } from './branchlocation';
import { Price } from './price';
import { Product } from './product';

export interface TransactionDetail extends BaseModel {
  transactionDetailId: number;
  transactionDetailUid: string;
  dQuantity: number;
  productAvailable: boolean;
  branchLocationId: number;
  branchLocation: BranchLocation;
  productId: number;
  product: Product;
  priceId: number;
  price: Price;
  productExpireDate: Date;
  iDocID: number;
  cDocNo: string;
  dDocDate: string;
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
}
export enum cDocType {
  GRN,
  INV,
  AIN,
  PRN,
  SLR
}
