import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { PaginatedResult } from '../models/pagination';
import { Price } from '../models/price';
import { SupplierPrice } from '../models/supplierprice';
import { CommonService } from './common.service';
import { SupplierPriceSupplier } from '../models/supplierpricesupplier';

@Injectable({
  providedIn: 'root'
})
export class PriceService {
  baseUrl = environment.apiUrl + 'Price/';
  constructor(private http: HttpClient, private router: Router,private commonService: CommonService) { }
  createPrice(model: any) {
    return this.http.post(this.baseUrl + 'createprice', model);
  }
  createSupplierPrice(model: any) {
    return this.http.post(this.baseUrl + 'supplierprice/createsupplierprice', model);
  }
  deleteSupplierPrice(model: any) {
    return this.http.post(this.baseUrl + 'supplierprice/deletesupplierprice', model)
  }
  getProductPriceBySupplierId(supplierId: number,branchLocationId: number) {
    return this.http.get<SupplierPrice[]>(this.baseUrl + 'supplierprice/' + supplierId+'/'+branchLocationId);
  }
  getSupplierPriceBySupplierProductId(supplierId: number,branchLocationId: number,productId: number) {
    return this.http.get<SupplierPrice[]>(this.baseUrl + 'supplierprice/' + supplierId+'/'+branchLocationId+'/'+productId);
  }
  getallsupplierpricesuppliers(branchLocationId: number,status: boolean): Observable<SupplierPriceSupplier[]>{
    return this.http.get<SupplierPriceSupplier[]>(this.baseUrl + 'getallsupplierpricesuppliers',
    {
      observe: 'body',
      params: new HttpParams()
        .set('status', String(status))
        .set('branchLocationId', branchLocationId.toString()),
    });
  }
  getPrices(
    status = true,
    pageNumber = 1,
    pageSize = 5
  ): Observable<PaginatedResult<Price[]>> {
    const paginatedResult: PaginatedResult<Price[]> = new PaginatedResult<Price[]>();
    const url = this.baseUrl + 'getallsuppliers';
    return this.http
      .get<Price[]>(url, {
        observe: 'response',
        params: new HttpParams()
          .set('status', String(status))
          .set('PageNumber', pageNumber.toString())
          .set('PageSize', pageSize.toString()),
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
  getSupplierPrices(
    status = true,
    pageNumber = 1,
    pageSize = 5,
    filterString?: string,
  ): Observable<PaginatedResult<SupplierPrice[]>> {
    const token = localStorage.getItem('token');
    const paginatedResult: PaginatedResult<SupplierPrice[]> = new PaginatedResult<SupplierPrice[]>();
    const url = this.baseUrl + 'getallsupplierprices';
    return this.http
      .get<SupplierPrice[]>(url, {
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
}
