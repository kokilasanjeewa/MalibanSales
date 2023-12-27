import { BaseModel } from './basemodel';

export interface Title extends BaseModel {
  titleId: number;
  titleName: string;
  description: string;
  titleUid: string;
}
