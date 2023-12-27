import { BaseModel } from './basemodel';
import { BranchLocation } from './branchlocation';
import { PurchaseOrderHeader } from './purchaseorderheader';
import { PurchaseReturnDetail } from './purchasereturndetail';
import { ReturnReason } from './returnreason';
import { Supplier } from './supplier';

export interface PurchaseReturnHeader extends BaseModel {
  purchaseReturnHeaderId: number;
  purchaseReturnHeaderUid: string;
  supplierId: number;
  branchLocationId: number;
  description: string;
  purchaseReturnDate: string;
  purchaseReturnAmount: number;
  purchaseReturnDetails: PurchaseReturnDetail[];
  supplier: Supplier;
  branchLocation: BranchLocation;
  returnTypeId: number;
  returnReason: ReturnReason;
  lines: PurchaseReturnDetail[];

}
