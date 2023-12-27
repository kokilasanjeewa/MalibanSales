import { BaseModel } from './basemodel';

export interface District extends BaseModel {
  districtId: number;
  districtNameEn: string;
  districtNameSl: string;
  districtNameTm: string;
  provinceCode: number | null;
  districtCode: number | null;
  districtUid: string;
}
