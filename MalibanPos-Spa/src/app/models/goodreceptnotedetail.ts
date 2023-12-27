import { BaseModel } from './basemodel';
import { GoodReceptNoteHeader } from './goodreceptnoteheader';
import { Product } from './product';
import { PurchaseOrderHeader } from './purchaseorderheader';
import { StockDetail } from './stockdetail';
import { StockIssueDetail } from './stockissuedetail';
import { SupplierPrice } from './supplierprice';

export interface GoodReceptNoteDetail extends BaseModel {
  goodReceptNoteDetailId: number;
  goodReceptNoteDetailUid: string;
  costPrice: number;
  retailPrice: number;
  pricePrecentage: number;
  itemQuantity: number;
  balanceItemQuantity: number | null;
  freeItemQuantity: number | null;
  invoiceItemDate: string;
  groupDiscountAmount: number | null;
  productExpiryDate: string;
  batchNumber: number | null;
  goodReceptNoteLine: number | null;
  productId: number;
  supplierPriceId: number;
  goodReceptNoteHeaderId: number | null;
  goodReceptNoteHeader: GoodReceptNoteHeader;
  product: Product;
  supplierPrice: SupplierPrice;
  stockIssueDetails: StockIssueDetail[];
  stockDetails: StockDetail[];
  purchaseOrderHeader: PurchaseOrderHeader;
  purchaseOrderHeaderId: number;
  vatPrecentage: number;
  nbtPrecentage: number;

}
