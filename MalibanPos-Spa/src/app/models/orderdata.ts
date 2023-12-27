import { SaleHeader } from './saleheader';
import { Data } from './data';

export interface OrderData extends Data {
  totalQuantity: number;
  computeTotal: number;
  computeReturnTotal: number;
  computeDiscountValue: number;
  computeGrandTotalValue: number;
  saleHeader: number;
  billActive: boolean;
}
export interface CartReturnData {
  cart1:OrderData;
  paymentType:number;
}
