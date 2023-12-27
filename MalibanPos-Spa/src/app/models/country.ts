import { BaseModel } from './basemodel';

export interface Country extends BaseModel {
  countryId: number;
  countryName: string;
  iSO: string;
  niceName: string;
  iSO3: string;
  phoneCode: string;
  countryCode: string;
  countryUid: string;
}
