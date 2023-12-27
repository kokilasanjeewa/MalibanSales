import { BaseModel } from './basemodel';

export interface Section extends BaseModel {
  sectionId: number;
  sectionUid: string;
  name: string;
  description: string;
}
