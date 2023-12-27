import { BranchLocation } from './branchlocation';
import { Price } from './price';
import { Product } from './product';
import { Store } from './store';

export interface Stock {
  stockId: number;
  stockUid: string;
  productId: number | null;
  priceId: number | null;
  unitsInStock: number;
  reorderLevel: ReorderLevel;
  alertQuantity: number | null;
  balanceQuantity: number;
  productAvailable: boolean;
  transferDate: string;
  transferBranchLocationId: number | null;
  productExpireDate: string;
  active: boolean;
  modifiedDate: string;
  createDate: string;
  createdBy: string;
  modifiedBy: string;
  product: Product;
  store: Store;
  branchLocation: BranchLocation;
  price: Price;
  branchLocationId: number | null;
  storeId: number | null;
}
enum ReorderLevel { Zero, Available, OrderPending }
