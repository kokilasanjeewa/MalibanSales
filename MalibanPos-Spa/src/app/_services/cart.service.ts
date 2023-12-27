import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of, ReplaySubject } from 'rxjs';
import { environment } from './../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  baseUrl = environment.apiUrl + 'Sale/';
  /*   private DataStore = new ReplaySubject<[]>(null);
    data$: Observable<any> = this.DataStore.asObservable(); */
  constructor(private http: HttpClient) { }

  CreateSaleItemDetails(model: any) {
    return this.http.post(this.baseUrl + 'createsaleitemdetails', model);
  }
  CreateSaleItemQuantity(model: any) {
    return this.http.post(this.baseUrl + 'quantity/createsaleitemquantity', model);
  }
  FifoCreateSaleItemQuantity(model: any) {
    return this.http.post(this.baseUrl + 'fifoquantity/fifocreatesaleitemquantity', model);
  }
  RemoveSaleItemDetails(model: any) {
    return this.http.post(this.baseUrl + 'removesaleitemdetails', model);
  }
  RemoveSaleItemQuantity(model: any) {
    return this.http.post(this.baseUrl + 'removesaleitemquantity', model);
  }

  CreateSaleHeader(model: any) {
    return this.http.post(this.baseUrl + 'Check/createsaleitem', model);
  }
  PrintSaleHeader(saleHeader: any) {
    return this.http.get(this.baseUrl + 'Check/print/' + saleHeader);
  }
  CreateSaleReturnHeader(model: any) {
    return this.http.post(this.baseUrl + 'SaleReturn/createsalereturnitem', model);
  }
  GetReceiptDetails(saleHeader: any) {
    return this.http.get(this.baseUrl + 'Receipt/' + saleHeader);
    /*       .subscribe((data: any) => {
        this.DataStore.next(data);
      }); */
  }
  GetReturnDetails(saleHeader: any,branchlocationId: number) {
    return this.http.get(this.baseUrl + 'Return/' + saleHeader+'/'+branchlocationId );
    /*       .subscribe((data: any) => {
        this.DataStore.next(data);
      }); */
  }
  GetCart() {
    return this.http.get(this.baseUrl);
    /*       .subscribe((data: any) => {
          this.DataStore.next(data);
        }); */
  }
  ClearCart() {
    return this.http.get(this.baseUrl + 'clear');
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
