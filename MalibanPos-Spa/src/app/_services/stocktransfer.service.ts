import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CommonService } from './common.service';
import { environment } from './../../environments/environment';
import { Observable } from 'rxjs';
import { PaginatedResult } from '../models/pagination';
import { map } from 'rxjs/operators';
import { Store } from '../models/store';
import { BranchLocation } from '../models/branchlocation';
import { ApplicationUser } from '../models/applicationuser';
import { cApprovalType } from '../models/capprovaltype';
import { BaseModel } from '../models/basemodel';
import { Product } from '../models/product';
import { Price } from '../models/price';
import { cDocType } from '../models/transactiondetail';
import { PaymentMethod } from '../models/paymentmethod';

@Injectable({
  providedIn: 'root',
})
export class StocktransferService {
  baseUrl = environment.apiUrl + 'StockTransfer/';
  constructor(private http: HttpClient, private commonService: CommonService) {}

  getStockTransferHeader(status: boolean, stockTransferHeaderId: number) {
    const token = localStorage.getItem('token');
    return this.http.get(this.baseUrl + 'getstockTransferHeader/' + stockTransferHeaderId, {
      headers: new HttpHeaders({ 'Content-Type': 'application/json', Authorization: 'Bearer ' + token, 'BranchLocationId': this.commonService.getUserBranchLocationId().toString() }),
      params: new HttpParams()
        .set('status', String(status))
        .set('BranchLocationId', this.commonService.getUserBranchLocationId().toString()),
    });
  }
  ApproveStockTransferHeader(model: any) {
    return this.http.post(this.baseUrl + 'approve/approvestocktransferheader', model);
  }
  ReceiveStockTransferHeader(model: any) {
    return this.http.post(this.baseUrl + 'receive/receivestocktransferheader', model);
  }
  ApproveStockTransferDetail(lineId: number,status:boolean) {
    const token = localStorage.getItem('token');
    return this.http.get(this.baseUrl + 'approve/' + lineId+'/'+status, {
      headers: new HttpHeaders({ 'Content-Type': 'application/json', Authorization: 'Bearer ' + token, 'BranchLocationId': this.commonService.getUserBranchLocationId().toString() }),
    });
  }
  getStockTransferLIst(
    status = true,
    pageNumber = 1,
    pageSize = 5,
    filterString?: string
  ): Observable<PaginatedResult<StockTransferHeader[]>> {
    const token = localStorage.getItem('token');
    const paginatedResult: PaginatedResult<StockTransferHeader[]> =
      new PaginatedResult<StockTransferHeader[]>();
    const url = this.baseUrl + 'getstocktransferlist';
    return this.http
      .get<StockTransferHeader[]>(url, {
        observe: 'response',
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Authorization: 'Bearer ' + token,
          BranchLocationId: this.commonService
            .getUserBranchLocationId()
            .toString(),
        }),
        params: new HttpParams()
          .set('status', String(status))
          .set('PageNumber', pageNumber.toString())
          .set('PageSize', pageSize.toString())
          .set('Filter', filterString)
          .set(
            'BranchLocationId',
            this.commonService.getUserBranchLocationId().toString()
          ),
      })
      .pipe(
        map((response) => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(
              response.headers.get('Pagination')
            );
            console.log(paginatedResult.result);
          }
          return paginatedResult;
        })
      );
  }
  getProducts(branchLocationId: number, isProduct: boolean) {
    return this.http.get<ProductDetail[]>(
      this.baseUrl + 'getstockproducts/' + branchLocationId + '/' + isProduct
    );
  }
  getProductPrices(branchLocationId: number, productId: number) {
    return this.http.get<PriceDetail[]>(
      this.baseUrl +
        'getproductpricesbyproductId/' +
        branchLocationId +
        '/' +
        productId
    );
  }
  addStockTransferItem(model: any) {
    return this.http.post(this.baseUrl + 'createstocktransferdetails', model);
  }
  createStockTransferHeader(model: any) {
    return this.http.post(this.baseUrl + 'Check/createstocktransferheader', model);
  }
  getcachestocktransferdetails(branchLocationId: number) {
    return this.http.get(this.baseUrl + 'getcachestocktransferdetails/' + branchLocationId);
  }
  removeline(lineId: string) {
    return this.http.delete(this.baseUrl + 'removeline/' + lineId);
  }
}
export interface ProductDetail {
  barcode: string;
}
export interface PriceDetail {
  productId: number;
  priceId: number;
  dSalePrice: number;
  dCostPrice: number;
  maximumRetailPrice: number;
  dStockQty:number;
}
export interface StockTransferDetail extends BaseModel {
  stockTransferDetailId: number;
  stockTransferDetailUid: string;
  transactionDetailId: number;
  productId: number;
  product: Product;
  priceId: number;
  price: Price;
  iDocID: number;
  cDocNo: string;
  dDocDate: string;
  productExpireDate: string;
  cDocType: cDocType;
  cExtRefNum: string;
  dQuantity: number;
  dIssueQuantity: number;
  dTransferQuantity: number;
  dNetQuantity: number;
  approveQuantity: number;
  cApprovalType: cApprovalType;
  dCostPrice: number;
  dSalePrice: number;
  maximumRetailPrice: number;
  barcode: string;
  productName: string;
  stockTransferHeaderId: number;
  stockTransferHeader: StockTransferHeader;
}
export interface StockTransferHeader extends BaseModel {
  stockTransferHeaderId: number;
  stockTransferHeaderUid: string;
  stockTransferHeaderFactoryId: number;
  stockTransferHeaderReferenceNo: string;
  lines: StockTransferDetail[];
  totalCost: number;
  totalDiscountAmount: number | null;
  totalBillAmount: number | null;
  store: Store;
  branchLocationId: number;
  branchLocation: BranchLocation;
  transferBranchLocationId: number;
  transferBranchLocation: BranchLocation;
  id: number;
  applicationUser: ApplicationUser;
  paymentMethodId: number;
  paymentMethod: PaymentMethod;
  isApprove: boolean;
  approvedBy: string;
  isComfirm: boolean;
  authorizedBy: string;
  isProcess: boolean;
  cApprovalType: cApprovalType;
  cDocType:cDocType;
  dateReceived: Date;
  receivedBy: string;

}
export interface GetStockTransferByDateRangeDto extends BaseModel {
  stockTransferDetailId: number;
  productCode: string;
  productName: string;
  transferBranch: string;
  cDocNo: string;
  StockTransferHeaderReferenceNo: string;
  transferDate: string;
  transferUnitPrice: number;
  transferSellingPrice: number;
  transferQty: number;
  productExpireDate: string;
  receiveBranch: string;
  receiveDate: string;
  receiveBy: string;
  transferBy: string;
  receiveUnitPrice: number;
  receiveSellingprice: number;
  receiveQty: number;
  paymentType: string;
  balance: number;
}
