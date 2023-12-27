import { BaseModel } from './basemodel';
import { Product } from './product';

export interface Vendor extends BaseModel {
  vendorId: number;
  vendorUid: string;
  vendorName: string;
  vendorCode: string;
  vendorDescription: string;
  product: Product[];
}