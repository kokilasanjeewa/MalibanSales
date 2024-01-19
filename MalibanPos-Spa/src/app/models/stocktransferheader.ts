import { ApplicationUser } from "./applicationuser";
import { BaseModel } from "./basemodel";
import { BranchLocation } from "./branchlocation";
import { StockTransferDetail } from "./stocktransferdetail";
import { Store } from "./store";
import { Supplier } from "./supplier";

export interface StockTransferHeader extends BaseModel {
  stockTransferHeaderId: number;
  stockTransferHeaderUid: string;
  stockTransferHeaderFactoryId: number;
  stockTransferHeaderReferenceNo: string;
  lines: StockTransferDetail[];
  supplierId: number;
  totalCost: number;
  totalDiscountAmount: number | null;
  totalBillAmount: number | null;
  supplier: Supplier;
  transferSupplier: Supplier;
  transferSupplierId: number;
  store: Store;
  branchLocationId: number;
  branchLocation: BranchLocation;
  transferBranchLocationId: number;
  transferBranchLocation: BranchLocation;
  id: number;
  applicationUser: ApplicationUser;
}
