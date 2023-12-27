import { SupplierProduct } from './../models/supplierproduct';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { environment } from '../../environments/environment';
import { AlertifyService } from './alertify.service';
import { Product } from '../models/product';

@Injectable({
  providedIn: 'root'
})
export class SupplierproductService {
  baseUrl = environment.apiUrl + 'SupplierProduct/';
  constructor(private http: HttpClient,
    private alertifyService: AlertifyService) { }
  createSupplierProduct(model: any) {
    return this.http.post(this.baseUrl + 'createsupplierproduct', model)
  }
  deleteSupplierProduct(model: any) {
    return this.http.post(this.baseUrl + 'deletesupplierproduct', model)
  }
  getProductBySupplierId(supplierId: number,branchLocationId: number) {
    return this.http.get<SupplierProduct[]>(this.baseUrl + supplierId + '/' + branchLocationId);
  }
  getProductFilterBySupplierId(supplierId: number,branchLocationId: number) {
    return this.http.get<Product[]>(this.baseUrl +'FilterProduct/'+supplierId + '/' + branchLocationId);
  }
  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      // TODO: send the error to remote logging infrastructure
      console.log(JSON.stringify(error.errorMessage)); // log to console instead
      this.alertifyService.error(JSON.stringify(error))
      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }
}
