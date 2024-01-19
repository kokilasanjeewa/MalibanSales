import { BaseModel } from './basemodel';
import { Product } from './product';
import { Supplier } from './supplier';

export interface BranchLocation extends BaseModel {
  branchLocationId: number;
  branchCode: string;
  branchName: string;
  address: string;
  contactNumbers: string;
  longitude: string;
  latitude: string;
  email:string|null;
}

