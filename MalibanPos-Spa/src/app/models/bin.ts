import { BaseModel } from './basemodel';
import { BranchLocation } from './branchlocation';
import { Rack } from './rack';
import { StockDetail } from './stockdetail';
import { Store } from './store';
import { StoreArea } from './storearea';

export interface Bin extends BaseModel {
  binId: number;
  shelfNumber: number | null;
  binNumber: number | null;
  binUid: string;
  binDescription: string;
  binWidth: number | null;
  bintHeight: number | null;
  binLength: number | null;
  maxTolerableWeight: number | null;
  rack: Rack;
  storeArea: StoreArea;
  store: Store;
  branchLocation: BranchLocation;
  branchLocationId: number | null;
  storeId: number | null;
  storeAreaId: number | null;
  rackId: number | null;
  binCode: string;
  stockDetails: StockDetail[];
}
