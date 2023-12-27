import { ApplicationUser } from './applicationuser';
import { BaseModel } from './basemodel';
import { BranchLocation } from './branchlocation';
import { GoodReceptNoteDetail } from './goodreceptnotedetail';
import { PurchaseOrderHeader } from './purchaseorderheader';
import { Store } from './store';
import { Supplier } from './supplier';

export interface GoodReceptNoteHeader extends BaseModel {
  goodReceptNoteHeaderId: number;
  goodReceptNoteHeaderUid: string;
  goodReceptNoteHeaderFactoryId: number;
  goodReceptNoteReferenceNo: string;
  lines: GoodReceptNoteDetail[];
  supplierInvoiceId: string;
  supplierInvoiceDate: string;
  totalCost: number;
  freeItemsTotalCost: number;
  totalDiscountAmount: number;
  totalBillAmount: number;
  supplier: Supplier;
  store: Store;
  branchLocationId: number;
  branchLocation: BranchLocation;
  applicationUser: ApplicationUser;
  purchaseOrderHeader: PurchaseOrderHeader;
}
