import { BaseModel } from './basemodel';
import { BranchLocation } from './branchlocation';
import { Department } from './department';
import { PurchaseRequestDetail } from './purchaserequestdetail';
import { Section } from './section';

export interface PurchaseRequestHeader extends BaseModel {
  purchaseRequestHeaderId: number;
  purchaseRequestHeaderUid: string;
  branchLocationId: number | null;
  departmentId: number | null;
  sectionId: number | null;
  description: string;
  expectedDate: string;
  isProcess: boolean;
  isComfirm: boolean;
  purchaseRequestDetails: PurchaseRequestDetail[];
  branchLocation: BranchLocation;
  purchaseRequestStatus: PurchaseRequestStatus;
  department: Department;
  section: Section;
  approvedBy: string;
  authorizedBy: string;
}
enum PurchaseRequestStatus { Unapproved, Approved, Authorized }
