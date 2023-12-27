import { BaseModel } from './basemodel';

export interface Department extends BaseModel {
  departmentId: number;
  departmentUid: string;
  name: string;
  description: string;
}
