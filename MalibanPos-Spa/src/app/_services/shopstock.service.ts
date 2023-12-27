import { Injectable } from '@angular/core';
import { CommonService } from 'src/app/_services/common.service';
import { catchError, finalize, map, tap } from 'rxjs/operators';
import { environment } from './../../environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Router } from '@angular/router';
import { AlertifyService } from './alertify.service';
import { Observable, of } from 'rxjs';
import { PaginatedResult } from '../models/pagination';
import { Shopstock } from '../models/shopstock';
import { TransactionDetail } from '../models/transactiondetail';
@Injectable({
  providedIn: 'root'
})
export class ShopstockService {
  baseUrl = environment.apiUrl + 'Stock/';
  constructor(
    private http: HttpClient,
    private commonService: CommonService
  ) { }
  getShopStocks(
    status = true,
    pageNumber = 1,
    pageSize = 5
  ): Observable<PaginatedResult<Shopstock[]>> {
    const token = localStorage.getItem('token');
    const paginatedResult: PaginatedResult<Shopstock[]> = new PaginatedResult<Shopstock[]>();
    const url = this.baseUrl + 'getallstocks';
    return this.http
      .get<Shopstock[]>(url, {
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
  getStockDetails(
    status = true,
    pageNumber = 1,
    pageSize = 5
  ): Observable<PaginatedResult<TransactionDetail[]>> {
    const token = localStorage.getItem('token');
    const paginatedResult: PaginatedResult<TransactionDetail[]> = new PaginatedResult<TransactionDetail[]>();
    const url = this.baseUrl + 'getallstockdetails';
    return this.http
      .get<TransactionDetail[]>(url, {
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

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead
      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }
}
