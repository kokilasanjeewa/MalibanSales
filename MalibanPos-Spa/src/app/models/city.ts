import { BaseModel } from './basemodel';

export interface City extends BaseModel {
  cityId: number;
  cityNameEn: string;
  cityNameSl: string;
  cityNameTm: string;
  postalCode: string;
  latitude: number;
  longitude: number;
  cityUid: string;
  districtCode: number | null;
}
