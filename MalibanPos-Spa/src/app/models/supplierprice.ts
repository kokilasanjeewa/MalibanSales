import { BaseModel } from './basemodel';
import { Product } from './product';
import { Supplier } from './supplier';

export interface SupplierPrice extends BaseModel {
  supplierPriceId: number;
  supplierPriceUid: string;
  unitPrice: number;
  profit: number;
  maximumRetailPrice: number;
  discountPrice: number;
  discountPrecentage: number;
  promotionPrice: number;
  promotionPrecentage: number;
  rowVersion: string;
  taxPrecentage: number | null;
  taXMethod: TaXMethod;
  statusType: StatusType;
  product: Product;
  productId: number;
  branchLocationId: number;
  supplierId: number;
  supplier: Supplier;
  priceDate: Date;
}
enum StatusType { Add, Change, TypingMistake }
enum TaXMethod { NOVAT, SVAT, VAT }
