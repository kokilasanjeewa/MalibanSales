import { BaseModel } from './basemodel';
import { BranchLocation } from './branchlocation';
import { StockIssueDetail } from './stockissuedetail';
import { Store } from './store';
import { StoreArea } from './storearea';

export interface StockIssueHeader extends BaseModel {
  stockIssueHeaderId: number;
  stockIssueHeaderUid: string;
  storeAreaId: number | null;
  storeArea: StoreArea;
  branchLocationId: number;
  branchLocation: BranchLocation;
  storeId: number;
  store: Store;
  stockIssueDetail: StockIssueDetail[];
}
