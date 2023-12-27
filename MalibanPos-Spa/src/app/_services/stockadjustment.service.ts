import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of, ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { PaginatedResult } from '../models/pagination';
import { StockAdjustmentDetail } from '../models/stockadjustmentdetail';
import { StockAdjustmentHeader } from '../models/stockadjustmentheader';
import { environment } from './../../environments/environment';
import { CommonService } from './common.service';
@Injectable({
  providedIn: 'root',
})
export class StockadjustmentService {
  baseUrl = environment.apiUrl + 'StockAdjustment/';
  /*   private DataStore = new ReplaySubject<[]>(null);
    data$: Observable<any> = this.DataStore.asObservable(); */
  constructor(private http: HttpClient, private commonService: CommonService) {}

  CreateStockaAdjustmentItemDetails(model: any) {
    return this.http.post(
      this.baseUrl + 'createstockadjustmentitemdetails',
      model
    );
  }
  UploadStockAdjustmentDetails(model: any) {
    return this.http.post(this.baseUrl + 'uploadstockadjustmentdetails', model);
  }
  CreateStockaAdjustmentItemQuantity(model: any) {
    return this.http.post(
      this.baseUrl + 'quantity/createstockadjustmentitemquantity',
      model
    );
  }
  UpdateApproveStatus(model: any) {
    return this.http.post(this.baseUrl + 'updateapprovestatus', model);
  }
  RemoveStockaAdjustmentItemDetails(model: any) {
    return this.http.post(
      this.baseUrl + 'removestockadjustmentitemdetails',
      model
    );
  }
  RemoveStockaAdjustmentItemQuantity(model: any) {
    return this.http.post(
      this.baseUrl + 'removestockadjustmentitemquantity',
      model
    );
  }
  CreateStockaAdjustmentHeader(model: any) {
    return this.http.post(
      this.baseUrl + 'Check/createstockadjustmentitem',
      model
    );
  }
  UpdateStockaAdjustmentHeader(model: any) {
    return this.http.post(
      this.baseUrl + 'Update/updatestockadjustmentitem',
      model
    );
  }
  ConfirmStockaAdjustmentHeader(model: any) {
    return this.http.post(
      this.baseUrl + 'Confirm/confirmstockadjustmentitem',
      model
    );
  }
  getstockadjustmentitemquantitybyproduct(model: any) {
    return this.http.post(
      this.baseUrl + 'getstockadjustmentitemquantitybyproduct',
      model
    );
  }
  GetStockaAdjustmentDetails(stockadjustmentHeader: any) {
    return this.http.get(this.baseUrl + 'Receipt/' + stockadjustmentHeader);
    /*       .subscribe((data: any) => {
        this.DataStore.next(data);
      }); */
  }
  GetStockaAdjustmentHeaderDetails(stockadjustmentHeader: any) {
    return this.http.get(this.baseUrl + 'Approve/' + stockadjustmentHeader);
    /*       .subscribe((data: any) => {
        this.DataStore.next(data);
      }); */
  }
  GetStockaAdjustment() {
    return this.http.get(this.baseUrl);
    /*       .subscribe((data: any) => {
          this.DataStore.next(data);
        }); */
  }
  ClearCart() {
    return this.http.get(this.baseUrl + 'clear');
  }
  getStockAdjustmentHeaders(
    status = true,
    pageNumber = 1,
    pageSize = 5
  ): Observable<PaginatedResult<StockAdjustmentHeader[]>> {
    const token = localStorage.getItem('token');
    const paginatedResult: PaginatedResult<StockAdjustmentHeader[]> =
      new PaginatedResult<StockAdjustmentHeader[]>();
    const url = this.baseUrl + 'getallstockadjustmentheaders';
    return this.http
      .get<StockAdjustmentHeader[]>(url, {
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
          .set(
            'BranchLocationId',
            this.commonService.getUserBranchLocationId().toString()
          ),
      })
      .pipe(
        //  tap((_) => console.log('fetched suppliers')),
        //  finalize(() => this.alertifyService.success('Loading successfully.'))
        //  catchError(this.handleError<Supplier[]>('getallsuppliers'))
        // tslint:disable-next-line: no-string-literal
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
  getStockAdjustmentDetails(
    status = true,
    pageNumber = 1,
    pageSize = 5,
    stockadjustmentHeaderId: number
  ): Observable<PaginatedResult<StockAdjustmentDetail[]>> {
    const token = localStorage.getItem('token');
    const paginatedResult: PaginatedResult<StockAdjustmentDetail[]> =
      new PaginatedResult<StockAdjustmentDetail[]>();
    const url = this.baseUrl + 'getallstockadjustmentdetails';
    return this.http
      .get<StockAdjustmentDetail[]>(url, {
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
          .set('StockAdjustmentHeaderId', stockadjustmentHeaderId.toString())
          .set(
            'BranchLocationId',
            this.commonService.getUserBranchLocationId().toString()
          ),
      })
      .pipe(
        //  tap((_) => console.log('fetched suppliers')),
        //  finalize(() => this.alertifyService.success('Loading successfully.'))
        //  catchError(this.handleError<Supplier[]>('getallsuppliers'))
        // tslint:disable-next-line: no-string-literal
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
  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead
      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }
}
