import { BaseModel } from './basemodel';

export interface PaymentMethod extends BaseModel {
  paymentMethodId: number;
  methodType: string;
  paymentDescription: string;
  paymentMethodUid: string;
}
