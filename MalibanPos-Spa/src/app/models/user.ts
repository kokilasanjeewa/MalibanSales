import { BaseModel } from './basemodel';
import { Product } from './product';

export interface User extends BaseModel {
  vendorId: number;
  vendorUid: string;
  vendorName: string;
  vendorCode: string;
  vendorDescription: string;
  product: Product[];
}