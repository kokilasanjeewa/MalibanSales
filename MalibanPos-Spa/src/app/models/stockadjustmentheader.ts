import { AdjustmentType } from './adjustmenttype';
import { ApplicationUser } from './applicationuser';
import { BaseModel } from './basemodel';
import { BranchLocation } from './branchlocation';
import { StockAdjustmentDetail } from './stockadjustmentdetail';

export interface StockAdjustmentHeader extends BaseModel {
  stockAdjustmentHeaderId: number;
  stockAdjustmentHeaderUid: string;
  stockAdjustmentsName: string;
  stockTakenDate: string;
  stockAdjustmentDate: string;
  description: string;
  isProcess: boolean;
  isApprove: boolean;
  stockAdjustmentCostAmount: number;
  stockAdjustmentSaleAmount: number;
  adjustmentType: AdjustmentType;
  lines: StockAdjustmentDetail[];
  branchLocation: BranchLocation;
  branchLocationId: number;
  adjustmentTypeId: number;
  id: number;
  applicationUser: ApplicationUser;
}
