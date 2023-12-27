import { BranchLocation } from './branchlocation';
import { Product } from './product';
import { Supplier } from './supplier';


export interface SupplierProduct {
  product: Product;
  supplier: Supplier;
  branchLocation: BranchLocation;
}