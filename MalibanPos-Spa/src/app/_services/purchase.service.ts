import { CommonService } from './common.service';
import { PurchaseRequestHeader } from './../models/purchaserequestheader';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of, AsyncSubject } from 'rxjs';
import { environment } from '../../environments/environment';
import { PaginatedResult } from '../models/pagination';
import { map } from 'rxjs/operators';
import { PurchaseRequestDetail } from '../models/purchaserequestdetail';

@Injectable({
  providedIn: 'root'
})
export class PurchaseService {
  baseUrl = environment.apiUrl + 'Purchase/';
  constructor(private http: HttpClient, private commonService: CommonService
  ) { }

  CreatepurchaseRequestitemdetails(model: any) {
    return this.http.post(this.baseUrl + 'createpurchaserequestitemdetails', model);
  }
 
  RemovePurchaseRequestItemDetails(model: any) {
    return this.http.post(this.baseUrl + 'removepurchaserequestitemdetails', model);
  }
  GetPurchase() {
    return this.http.get(this.baseUrl);
  }
  ClearPurchase() {
    return this.http.get(this.baseUrl + 'clear');
  }
  CreatePurchaseRequestHeader(model: any) {
    return this.http.post(this.baseUrl + 'Check/createpurchaserequestheader', model);
  }
  getPurchaseRequestHeaders(
    status = true,
    pageNumber = 1,
    pageSize = 5
  ): Observable<PaginatedResult<PurchaseRequestHeader[]>> {
    const token = localStorage.getItem('token');
    const paginatedResult: PaginatedResult<PurchaseRequestHeader[]> = new PaginatedResult<PurchaseRequestHeader[]>();
    const url = this.baseUrl + 'getallpurchaserequestheaders';
    return this.http
      .get<PurchaseRequestHeader[]>(url, {
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
  getPurchaseRequestDetails(
    status = true,
    pageNumber = 1,
    pageSize = 5,
    purchaseRequestHeaderId: number,
  ): Observable<PaginatedResult<PurchaseRequestDetail[]>> {
    const token = localStorage.getItem('token');
    const paginatedResult: PaginatedResult<PurchaseRequestDetail[]> = new PaginatedResult<PurchaseRequestDetail[]>();
    const url = this.baseUrl + 'getallpurchaserequestdetails';
    return this.http
      .get<PurchaseRequestDetail[]>(url, {
        observe: 'response',
        headers: new HttpHeaders({ 'Content-Type': 'application/json', Authorization: 'Bearer ' + token, 'BranchLocationId': this.commonService.getUserBranchLocationId().toString() }),
        params: new HttpParams()
          .set('status', String(status))
          .set('PageNumber', pageNumber.toString())
          .set('PageSize', pageSize.toString())
          .set('PurchaseRequestHeaderId', purchaseRequestHeaderId.toString())
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
