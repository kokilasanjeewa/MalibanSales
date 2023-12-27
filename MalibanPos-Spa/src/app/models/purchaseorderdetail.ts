import { BaseModel } from './basemodel';
import { Product } from './product';
import { PurchaseOrderHeader } from './purchaseorderheader';
import { SupplierPrice } from './supplierprice';

export interface PurchaseOrderDetail extends BaseModel {
  purchaseOrderDetailId: number;
  purchaseOrderDetailUid: string;
  purchaseOrderHeaderId: number;
  productId: number | null;
  supplierPriceId: number;
  modifyDate: string;
  quantity: number;
  approveQuantity: number;
  cApprovalType:number
  amount: number;
  currentQuantity: number | null;
  returnQuantity: number | null;
  bonusQuantity: number | null;
  gRNQuantity: number | null;
  freeQuantity: number | null;
  purchaseOrderHeaders: PurchaseOrderHeader;
  product: Product;
  supplierPrice: SupplierPrice;
  productExpiryDate: string;

}
