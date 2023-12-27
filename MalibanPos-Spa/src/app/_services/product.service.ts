import { environment } from './../../environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AlertifyService } from './alertify.service';
import { Observable, of } from 'rxjs';
import { Product } from '../models/product';
import { PaginatedResult } from '../models/pagination';
import { map } from 'rxjs/operators';
@Injectable({
  providedIn: 'root',
})
export class ProductService {
  baseUrl = environment.apiUrl + 'Product/';
  constructor(private http: HttpClient, private router: Router) { }
  public getData(route: string) {
    return this.http.get(this.createCompleteRoute(route, environment.apiUrl));
  }
  public create(route: string, body) {
    return this.http.post(
      this.createCompleteRoute(route, environment.apiUrl),
      body,
      this.generateHeaders()
    );
  }
  private createCompleteRoute(route: string, envAddress: string) {
    return `${envAddress}/${route}`;
  }
  private generateHeaders = () => {
    return {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
    };
  }
  createProduct(model: any) {
    return this.http.post(this.baseUrl + 'createproduct', model);
  }
  updateProduct(model: any, id: any) {
    return this.http.put(this.baseUrl + id, model);
  }
  getProduct(code: any) {
    return this.http.get<Product>(this.baseUrl + code);
  }
  getProductById(id: any) {
    return this.http.get<Product>(this.baseUrl + 'Id/' + id);
  }
  getProductsByProductName(productName: any) {
    return this.http.get<Product>(this.baseUrl + 'ProductName/' + productName);
  }
  getProducts(
    status = true,
    pageNumber = 1,
    pageSize = 5,
    filterString?: string
  ): Observable<PaginatedResult<Product[]>> {
    const token = localStorage.getItem('token');
    const paginatedResult: PaginatedResult<Product[]> = new PaginatedResult<Product[]>();
    const url = this.baseUrl + 'getallproducts';
    return this.http
      .get<Product[]>(url, {
        observe: 'response',
        headers: new HttpHeaders({ 'Content-Type': 'application/json', Authorization: 'Bearer ' + token }),
        params: new HttpParams()
          .set('status', String(status))
          .set('PageNumber', pageNumber.toString())
          .set('PageSize', pageSize.toString())
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
  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead
      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }
}
