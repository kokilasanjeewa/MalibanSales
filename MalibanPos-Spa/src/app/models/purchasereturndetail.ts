import { Product } from './product';
import { UnitOfMeasurement } from './unitofmeasurement';
import { PurchaseReturnHeader } from './purchasereturnheader';
import { BaseModel } from './basemodel';
import { SupplierPrice } from './supplierprice';

export interface PurchaseReturnDetail extends BaseModel {
  purchaseReturnDetailId: number;
  purchaseReturnDetailUid: string;
  purchaseReturnHeaderId: number;
  unitOfMeasurementId: number;
  productId: number;
  supplierPriceId: number;
  barcode: string;
  returnQuantity: number;
  returnPrice: number;
  subTotal: number;
  freeQuantity: number;
  purchaseReturnHeader: PurchaseReturnHeader;
  unitOfMeasurement: UnitOfMeasurement;
  product: Product;
  supplierPrice: SupplierPrice;
}
