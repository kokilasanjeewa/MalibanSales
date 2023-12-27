import { BaseModel } from './basemodel';
import { SaleHeader } from './saleheader';

export interface Payment extends BaseModel {
  paymentId: number;
  paymentUid: string;
  paymentType: PaymentType;
  allowed: boolean;
  saleHeader: SaleHeader[];
}
 enum PaymentType {
  Credit,
  Cash
}
