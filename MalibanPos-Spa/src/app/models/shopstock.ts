import { Price } from './price';
import { Branch } from './branch';
import { BaseModel } from './basemodel';
import { Product } from './product';

export interface Shopstock extends BaseModel {
  shopStockId: number;
  unitsInStock: number;
  reorderLevel: ReorderLevel;
  alertQuantity: number;
  saleQuantity: number;
  productAvailable: boolean;
  transferDate: Date
  transferBranchLocationId: number;
  productExpireDate: Date
  productId: number;
  product: Product;
  branchLocationId: number;
  branchLocation: Branch;
  priceId: number;
  price: Price;
}
enum ReorderLevel { Zero, Available, OrderPending }
