import { BaseModel } from "./basemodel";
import { BranchLocation } from "./branchlocation";

export interface CalculationMaster extends BaseModel {
  calculationMasterId: number;
  calculationMasterUid: string;
  calculationMasterDescription: string;
  branchLocation: BranchLocation;
  branchLocationId: number | null;
  salePricePrecentageCompany: number;
  salePricePrecentageEmployee: number;
  managerCreditLimitPrecentage: number;
  staffCreditLimitPrecentage: number;
  workerCreditLimitPrecentage: number;
  vatPrecentage: number;
  nbtPrecentage: number;
}
