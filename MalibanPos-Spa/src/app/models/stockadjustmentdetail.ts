import { BaseModel } from './basemodel';
import { Price } from './price';
import { Product } from './product';
import { StockAdjustmentHeader } from './stockadjustmentheader';

export interface StockAdjustmentDetail extends BaseModel {
  stockAdjustmentDetailId: number;
  stockAdjustmentDetailUid: string;
  stockAdjustmentHeaderId: number;
  barcode: string;
  productName: string;
  approveQuantity: number;
  cApprovalType:number;
  adjustmentQuantity: number;
  currentQuantity: number | null;
  previousQuantity: number | null;
  stockAdjustmentHeader: StockAdjustmentHeader;
  product: Product;
  price: Price;
  productId: number;
  priceId: number;
  costPrice: number;
  retailPrice: number;
  stockAdjustmentTotal: number;
}
