import { BranchLocation } from './branchlocation';
import { Customer } from './customer';
import { Price } from './price';
import { Product } from './product';
import { ReceiptDetails } from './receiptdetails';

export interface ReceiptPrint {
  customer: CustomerDetail;
  receiptDetails: ReceiptDetails[];
  branchLocation: BranchLocation;
}
interface CustomerDetail {
  number: string;
  fullName: string;
  creditLimit: number;
  creditBalance: number;
  previousCreditBalance: number;
  epfNo: Number;
}
