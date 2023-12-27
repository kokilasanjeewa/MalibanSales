import { BaseModel } from './basemodel';

export interface ReturnReason extends BaseModel {
  returnTypeId: number;
  returnType: string;
  returnDescription: string;
  returnTypeUid: string;
}
