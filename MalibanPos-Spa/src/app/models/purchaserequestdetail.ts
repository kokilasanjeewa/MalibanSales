import { BaseModel } from './basemodel';
import { Brand } from './brand';
import { Product } from './product';
import { PurchaseOrderHeader } from './purchaseorderheader';
import { PurchaseRequestHeader } from './purchaserequestheader';
import { UnitOfMeasurement } from './unitofmeasurement';

export interface PurchaseRequestDetail extends BaseModel {
  purchaseRequestDetailId: number;
  purchaseRequestDetailUid: string;
  quantity: number;
  rejectQuantity: number | null;
  stockQuantity: number | null;
  brandId: number | null;
  brand: Brand;
  purchaseRequestHeaderId: number;
  purchaseRequestHeader: PurchaseRequestHeader;
  unitOfMeasurementId: number | null;
  unitOfMeasurement: UnitOfMeasurement;
  productId: number;
  product: Product;
  purchaseOrderHeaderId: number;
  isComfirm: boolean;
  approvedQuantity: number | null;
  purchaseOrderHeader: PurchaseOrderHeader;
}
