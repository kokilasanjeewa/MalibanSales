import { BaseModel } from './basemodel';
import { BranchLocation } from './branchlocation';

export interface StockMovementHeader extends BaseModel {
  stockMovementHeaderId: number;
  stockMovementHeaderUid: string;
  branchLocationId: number;
  branchLocation: BranchLocation;
  storeId: number | null;
}
