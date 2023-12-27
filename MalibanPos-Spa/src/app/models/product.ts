import { UnitOfMeasurement } from './unitofmeasurement';
import { Brand } from './brand';
import { Vendor } from './vendor';
import { BaseModel } from './basemodel';

export interface Product extends BaseModel {
  productId: number;
  expiryDate: Date;
  createdBy: string;
  sKU: string;
  barcode: string;
  productName: string;
  productDescription: string;
  categoryId: number;
  subCategoryId: number;
  vendor: Vendor;
  size: string;
  color: string;
  unitWeight: string;
  note: string;
  unitOfMeasurement: UnitOfMeasurement;
  brand: Brand;
  length: number;
  width: number;
  height: number;
  active: boolean;
}
