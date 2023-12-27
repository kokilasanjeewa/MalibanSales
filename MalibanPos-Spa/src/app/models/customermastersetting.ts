import { BaseModel } from "./basemodel";
import { BranchLocation } from "./branchlocation";

export interface CustomerMasterSetting extends BaseModel {
  customerMasterSettingId: number;
  customerMasterSettingUid: string;
  customerMasterSettingDescription: string;
  branchLocation: BranchLocation;
  branchLocationId: number | null;
  customerCategory: string;
  customerCategoryCode: CustomerCategoryCode;
  billingCycleStartDate: string;
  billingCycleEndDate: string;
  creditHoldingDays: number;
  creaditBalancePerDays: number;
}

export enum CustomerCategoryCode {
  Worker,
  Staff,
  Executive
}
