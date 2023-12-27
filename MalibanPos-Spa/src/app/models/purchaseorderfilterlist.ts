export interface PurchaseOrderListReport {
  deliveryDate: string;
  pOCreatedDate: string;
  unitCost: number;
  unitCostWithOrOutVAT: number;
  maximumRetailPrice: number;
  dSalePrice: number;
  dCostPrice: number;
  total: number;
  quantity: number;
  goodReceiptQuantity: number;
  productName: string;
  productCode: string;
  branchCode: string;
  branchName: string;
  supplierName: string;
  pONumber: string;
  goodReceptNoteNumber: string;
  status: string;
  vatPrecentage: number;
  pricePrecentage: number;
}
