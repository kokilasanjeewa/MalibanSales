import { BaseModel } from './basemodel';

export interface Province extends BaseModel {
  provinceId: number;
  provinceCode: number | null;
  provinceNameEn: string;
  provinceNameSl: string;
  provinceNameTm: string;
  provinceUid: string;
}
