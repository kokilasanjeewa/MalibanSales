import { BaseModel } from './basemodel';
import { Bin } from './bin';
import { BranchLocation } from './branchlocation';
import { GoodReceptNoteDetail } from './goodreceptnotedetail';
import { Price } from './price';
import { Product } from './product';
import { Store } from './store';
import { StoreArea } from './storearea';

export interface StockDetail extends BaseModel {
  stockDetailId: number;
  stockDetailUid: string;
  stockQuantity: number;
  reorderLevel: ReorderLevel;
  alertQuantity: number | null;
  balanceQuantity: number;
  productAvailable: boolean;
  unitsOnOrder: number | null;
  storeAreaId: number;
  storeArea: StoreArea[];
  branchLocationId: number;
  branchLocation: BranchLocation;
  storeId: number | null;
  store: Store;
  productId: number;
  product: Product;
  binId: number | null;
  bin: Bin;
  goodReceptNoteDetailId: number;
  goodReceptNoteDetail: GoodReceptNoteDetail;
  priceId: number;
  price: Price;
}
enum ReorderLevel { Zero, Available, OrderPending }
