import { BaseModel } from './basemodel';
import { Bin } from './bin';
import { BranchLocation } from './branchlocation';
import { Stock } from './stock';
import { StockDetail } from './stockdetail';
import { StockIssueHeader } from './stockissueheader';
import { StockMovementHeader } from './stockmovementheader';
import { StoreArea } from './storearea';

export interface Store extends BaseModel {
  storeId: number;
  storeUid: string;
  storeName: string;
  storeDescription: string;
  storeArea: StoreArea[];
  branchLocationId: number | null;
  branchLocation: BranchLocation;
  stockMovementHeaders: StockMovementHeader[];
  stockDetails: StockDetail[];
  stockIssueHeaders: StockIssueHeader[];
  bins: Bin[];
  stocks: Stock[];
}
