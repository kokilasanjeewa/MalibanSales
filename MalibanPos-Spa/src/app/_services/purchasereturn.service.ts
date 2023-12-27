import { PurchaseReturnHeader } from './../models/purchasereturnheader';
import { Injectable } from '@angular/core';
import { PurchaseOrderDetail } from './../models/purchaseorderdetail';
import { PurchaseOrderHeader } from './../models/purchaseorderheader';
import { PaginatedResult } from './../models/pagination';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, of, ReplaySubject } from 'rxjs';
import { environment } from '../../environments/environment';
import { CommonService } from './common.service';
import { map } from 'rxjs/operators';
@Injectable({
  providedIn: 'root'
})
export class PurchasereturnService {
  baseUrl = environment.apiUrl + 'PurchaseReturn/';
  /*   private DataStore = new ReplaySubject<[]>(null);
    data$: Observable<any> = this.DataStore.asObservable(); */
  constructor(private http: HttpClient, private commonService: CommonService) { }

  CreatepurchaseReturnitemdetails(model: any) {
    return this.http.post(this.baseUrl + 'createpurchasereturnitemdetails', model);
  }
  RemovePurchaseReturnItemDetails(model: any) {
    return this.http.post(this.baseUrl + 'removepurchasereturnitemdetails', model);
  }
  GetPurchaseOrderDetails(id: any) {
    return this.http.get(this.baseUrl + 'Get/' + id);
    /*     .subscribe((data: any) => {
          this.DataStore.next(data);
        }); */
  }
  GetPurchaseReturnHeder(status: boolean, purchaseReturnId: number) {
    const token = localStorage.getItem('token');
    return this.http.get(this.baseUrl + 'getpurchasereturnheader/' + purchaseReturnId, {
      headers: new HttpHeaders({ 'Content-Type': 'application/json', Authorization: 'Bearer ' + token, 'BranchLocationId': this.commonService.getUserBranchLocationId().toString() }),
      params: new HttpParams()
        .set('status', String(status))
        .set('BranchLocationId', this.commonService.getUserBranchLocationId().toString()),
    });
    /*     .subscribe((data: any) => {
          this.DataStore.next(data);
        }); */
  }
  GetPurchaseReturnHederByGRNId(status: boolean, goodReceiptNoteHeaderId: number) {
    const token = localStorage.getItem('token');
    return this.http.get(this.baseUrl + 'getpurchasereturnheaderbygrnid/' + goodReceiptNoteHeaderId, {
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
  GetPurchaseReturn() {
    return this.http.get(this.baseUrl);
    /*    .subscribe((data: any) => {
          this.DataStore.next(data);
        });  */
  }
  ClearPurchaseOrder() {
    return this.http.get(this.baseUrl + 'clear');
  }

  CreatePurchaseReturnHeader(model: any) {
    return this.http.post(this.baseUrl + 'Check/createpurchasereturnheader', model);
  }
  getPurchaseReturnHeaders(
    status = true,
    pageNumber = 1,
    pageSize = 5
  ): Observable<PaginatedResult<PurchaseReturnHeader[]>> {
    const token = localStorage.getItem('token');
    const paginatedResult: PaginatedResult<PurchaseReturnHeader[]> = new PaginatedResult<PurchaseReturnHeader[]>();
    const url = this.baseUrl + 'getallpurchasereturnheaders';
    return this.http
      .get<PurchaseReturnHeader[]>(url, {
        observe: 'response',
        headers: new HttpHeaders({ 'Content-Type': 'application/json', Authorization: 'Bearer ' + token, 'BranchLocationId': this.commonService.getUserBranchLocationId().toString() }),
        params: new HttpParams()
          .set('status', String(status))
          .set('PageNumber', pageNumber.toString())
          .set('PageSize', pageSize.toString())
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
