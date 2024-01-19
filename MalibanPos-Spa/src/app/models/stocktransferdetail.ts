import { BaseModel } from "./basemodel";
import { cApprovalType } from "./capprovaltype";
import { Price } from "./price";
import { Product } from "./product";
import { StockTransferHeader } from "./stocktransferheader";
import { cDocType } from "./transactiondetail";

export interface StockTransferDetail extends BaseModel {
  stockTransferDetailId: number;
  stockTransferDetailUid: string;
  productId: number;
  product: Product;
  priceId: number;
  price: Price;
  iDocID: number;
  cDocNo: string;
  dDocDate: string;
  productExpireDate: string;
  cDocType: cDocType;
  cExtRefNum: string;
  dQuantity: number;
  dIssueQuantity: number;
  dTransferQuantity: number;
  dNetQuantity: number;
  approveQuantity: number;
  cApprovalType: cApprovalType;
  dCostPrice: number;
  dSalePrice: number;
  barcode: string;
  productName: string;
  stockTransferHeaderId: number;
  stockTransferHeader: StockTransferHeader;
}
