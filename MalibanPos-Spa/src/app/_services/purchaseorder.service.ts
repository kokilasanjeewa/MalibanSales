import { PurchaseOrderDetail } from './../models/purchaseorderdetail';
import { PurchaseOrderHeader } from './../models/purchaseorderheader';
import { PaginatedResult } from './../models/pagination';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of, ReplaySubject } from 'rxjs';
import { environment } from '../../environments/environment';
import { CommonService } from './common.service';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class PurchaseorderService {
  baseUrl = environment.apiUrl + 'PurchaseOrder/';
  /*   private DataStore = new ReplaySubject<[]>(null);
    data$: Observable<any> = this.DataStore.asObservable(); */
  constructor(private http: HttpClient, private commonService: CommonService) { }

  CreatepurchaseOrderitemdetails(model: any) {
    return this.http.post(this.baseUrl + 'createpurchaseorderitemdetails', model);
  }
  CreatepurchaseOrderitemdetaillist(model: any) {
    return this.http.post(this.baseUrl + 'createpurchaseorderitemdetaillist', model);
  }
  RemovePurchaseOrderItemDetails(model: any) {
    return this.http.post(this.baseUrl + 'removepurchaseorderitemdetails', model);
  }
  GetPurchaseOrderDetails(id: any) {
    return this.http.get(this.baseUrl + 'Get/' + id);
    /*     .subscribe((data: any) => {
          this.DataStore.next(data);
        }); */
  }
  GetPurchaseOrderHeder(status: boolean, purchaseOrderId: number) {
    const token = localStorage.getItem('token');
    return this.http.get(this.baseUrl + 'getpurchaseorderheader/' + purchaseOrderId, {
      headers: new HttpHeaders({ 'Content-Type': 'application/json', Authorization: 'Bearer ' + token, 'BranchLocationId': this.commonService.getUserBranchLocationId().toString() }),
      params: new HttpParams()
        .set('status', String(status))
        .set('BranchLocationId', this.commonService.getUserBranchLocationId().toString()),
    });
    /*     .subscribe((data: any) => {
          this.DataStore.next(data);
        }); */
  }
  SavePurchaseOrderHeader(model: any) {
    return this.http.post(this.baseUrl + 'savepurchaseorderheader', model);
  }
  GetPurchaseOrder() {
    return this.http.get(this.baseUrl);
    /*    .subscribe((data: any) => {
          this.DataStore.next(data);
        });  */
  }
  ClearPurchaseOrder() {
    return this.http.get(this.baseUrl + 'clear');
  }
  CreatePurchaseOrderHeader(model: any) {
    return this.http.post(this.baseUrl + 'Check/createpurchaseorderheader', model);
  }
  ApprovePurchaseOrderHeader(model: any) {
    return this.http.post(this.baseUrl + 'approve/approvepurchaseorderheader', model);
  }
  UpdatePurchaseOrderHeader(model: any) {
    return this.http.put(this.baseUrl +'update/updatepurchaseorderheader', model);
  }
  ConfirmPurchaseOrderHeader(model: any) {
    return this.http.put(this.baseUrl + 'Confirm/confirmpurchaseorderheader', model);
  }
  GetPurchaseOrderHeaderDetails(purchaseOrderId: number) {
    return this.http.get(this.baseUrl + 'Approve/' + purchaseOrderId);
    /*       .subscribe((data: any) => {
        this.DataStore.next(data);
      }); */
  }
  UpdateApproveStatus(model: any) {
    return this.http.post(this.baseUrl + 'updateapprovestatus', model);
  }
  UpdateApproveQuantity(model: any) {
    return this.http.post(this.baseUrl + 'updateapprovequantity', model);
  }
  getPurchaseOrderHeaders(
    status = true,
    pageNumber = 1,
    pageSize = 5,
    filterString?: string
  ): Observable<PaginatedResult<PurchaseOrderHeader[]>> {
    const token = localStorage.getItem('token');
    const paginatedResult: PaginatedResult<PurchaseOrderHeader[]> = new PaginatedResult<PurchaseOrderHeader[]>();
    const url = this.baseUrl + 'getallpurchaseorderheaders';
    return this.http
      .get<PurchaseOrderHeader[]>(url, {
        observe: 'response',
        headers: new HttpHeaders({ 'Content-Type': 'application/json', Authorization: 'Bearer ' + token, 'BranchLocationId': this.commonService.getUserBranchLocationId().toString() }),
        params: new HttpParams()
          .set('status', String(status))
          .set('PageNumber', pageNumber.toString())
          .set('PageSize', pageSize.toString())
          .set('BranchLocationId', this.commonService.getUserBranchLocationId().toString())
          .set('Filter', filterString),
      })
      .pipe(
        //  tap((_) => console.log('fetched suppliers')),
        //  finalize(() => this.alertifyService.success('Loading successfully.'))
        //  catchError(this.handleError<Supplier[]>('getallsuppliers'))
        // tslint:disable-next-line: no-string-literal
        map((response) => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
            console.log(paginatedResult.result);
          }
          return paginatedResult;
        }),
      );
  }
  getPurchaseOrderDetails(
    status = true,
    pageNumber = 1,
    pageSize = 5,
    purchaseOrderHeaderId: number,
  ): Observable<PaginatedResult<PurchaseOrderDetail[]>> {
    const token = localStorage.getItem('token');
    const paginatedResult: PaginatedResult<PurchaseOrderDetail[]> = new PaginatedResult<PurchaseOrderDetail[]>();
    const url = this.baseUrl + 'getallpurchaseorderdetails';
    return this.http
      .get<PurchaseOrderDetail[]>(url, {
        observe: 'response',
        headers: new HttpHeaders({ 'Content-Type': 'application/json', Authorization: 'Bearer ' + token, 'BranchLocationId': this.commonService.getUserBranchLocationId().toString() }),
        params: new HttpParams()
          .set('status', String(status))
          .set('PageNumber', pageNumber.toString())
          .set('PageSize', pageSize.toString())
          .set('PurchaseOrderHeaderId', purchaseOrderHeaderId.toString())
          .set('BranchLocationId', this.commonService.getUserBranchLocationId().toString()),
      })
      .pipe(
        //  tap((_) => console.log('fetched suppliers')),
        //  finalize(() => this.alertifyService.success('Loading successfully.'))
        //  catchError(this.handleError<Supplier[]>('getallsuppliers'))
        // tslint:disable-next-line: no-string-literal
        map((response) => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
            console.log(paginatedResult.result);
          }
          return paginatedResult;
        }),
      );
  }
  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead
      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }
}
