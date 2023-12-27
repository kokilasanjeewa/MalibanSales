import { BaseModel } from './basemodel';

export interface AdjustmentType extends BaseModel {
  adjustmentTypeId: number;
  adjustmentName: string;
  adjustmentDescription: string;
  adjustmentTypeUid: string;
}
