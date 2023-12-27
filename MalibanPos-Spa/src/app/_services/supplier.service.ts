import { CommonService } from 'src/app/_services/common.service';
import { catchError, finalize, map, tap } from 'rxjs/operators';
import { environment } from './../../environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AlertifyService } from './alertify.service';
import { Observable, of } from 'rxjs';
import { Supplier } from '../models/supplier';
import { PaginatedResult } from '../models/pagination';

@Injectable({
  providedIn: 'root',
})
export class SupplierService {
  baseUrl = environment.apiUrl + 'Supplier/';
  constructor(
    private http: HttpClient,
    private commonService: CommonService
  ) { }
  createSupplier(model: any) {
    return this.http.post(this.baseUrl + 'createsupplier', model);
  }
  updateSupplier(model: any, id: any) {
    return this.http.put(this.baseUrl + id, model);
  }
  getSupplierById(supplierId?: any): Observable<Supplier> {
    const url = this.baseUrl;
    return this.http.get<Supplier>(url + supplierId).pipe(
      tap((_) => console.log(`fetched Supplier supplierId=${supplierId}`)),
      catchError(
        this.handleError<Supplier>('getSupplierById supplierId=${supplierId}')
      )
    );
  }

  getSuppliers(
    status = true,
    pageNumber = 1,
    pageSize = 5,
    filterString?: string
  ): Observable<PaginatedResult<Supplier[]>> {
    const token = localStorage.getItem('token');
    const paginatedResult: PaginatedResult<Supplier[]> = new PaginatedResult<Supplier[]>();
    const url = this.baseUrl + 'getallsuppliers';
    return this.http
      .get<Supplier[]>(url, {
        observe: 'response',
        headers: new HttpHeaders({ 'Content-Type': 'application/json', Authorization: 'Bearer ' + token, 'BranchLocationId': this.commonService.getUserBranchLocationId().toString() }),
        params: new HttpParams()
          .set('status', String(status))
          .set('PageNumber', pageNumber.toString())
          .set('PageSize', pageSize.toString())
          .set('Filter', filterString)
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
  deletesupplier(id: any) {
    const token = localStorage.getItem('token');
    return this.http.delete(this.baseUrl + id,
      {
        observe: 'body',
        headers: new HttpHeaders({ 'Content-Type': 'application/json', Authorization: 'Bearer ' + token, 'BranchLocationId': this.commonService.getUserBranchLocationId().toString() }),
      });
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
