import { BaseModel } from './basemodel';
import { Price } from './price';
import { Product } from './product';

export interface SaleDetail extends BaseModel {
  saleDetailId: number;
  saleDetailUid: string;
  productId: number;
  priceId: number;
  itemPrice: number;
  itemDiscount: number;
  total: number;
  subTotal: number;
  quantity: number;
  stockQuantity: number;
  returnQuantity: number;
  billDate: string;
  rowVersion: string;
  isReturn: boolean;
  paid: boolean;
  product: Product;
  price: Price;
}
