import { BaseModel } from './basemodel';
import { Bin } from './bin';
import { BranchLocation } from './branchlocation';
import { Rack } from './rack';
import { StockIssueHeader } from './stockissueheader';
import { Store } from './store';

export interface StoreArea extends BaseModel {
  storeAreaId: number;
  storeAreaUid: string;
  storeAreaName: string;
  storeAreaDescription: string;
  store: Store;
  storeId: number;
  branchLocationId: number;
  racks: Rack[];
  branchLocation: BranchLocation;
  stockIssueHeaders: StockIssueHeader[];
  bins: Bin[];
}
