import { Price } from './price';
import { Product } from './product';

export interface ReceiptDetails {
  price: Price;
  product: Product;
  quantity: number;
  saleDetailId:number;
  returnQuantity: number;
}
