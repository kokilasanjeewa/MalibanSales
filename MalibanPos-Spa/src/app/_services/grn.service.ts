import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of, ReplaySubject, Observable, BehaviorSubject } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { GoodReceptNoteDetail } from '../models/goodreceptnotedetail';
import { GoodReceptNoteHeader } from '../models/goodreceptnoteheader';
import { PaginatedResult } from '../models/pagination';
import { AlertifyService } from './alertify.service';
import { CommonService } from './common.service';

@Injectable({
  providedIn: 'root'
})
export class GrnService {
  baseUrl = environment.apiUrl + 'Grn/';
  /*   private DataStore = new BehaviorSubject<[]>(null);
    data$: Observable<any> = this.DataStore.asObservable(); */
  constructor(private http: HttpClient, private alertifyService: AlertifyService, private commonService: CommonService
  ) { }

  Creategrnitemdetails(model: any) {
    return this.http.post(this.baseUrl + 'creategrnitemdetails', model);
  }
  Creategrnitemdetaillist(model: any) {
    return this.http.post(this.baseUrl + 'creategrnitemdetaillist', model);
  }
  Updategrnitemdetaillist(model: any) {
    return this.http.post(this.baseUrl + 'updategrnitemdetaillist', model);
  }
  GetgrnbyBranchSupplier(supplierId: number,branchLocationId:number)
  {
    return this.http.get(this.baseUrl+'getgrnbysupplier',{
      params: new HttpParams()
      .set('supplierId', String(supplierId))
      .set('branchLocationId', String(branchLocationId)),
    });
  }
  GetGoodReceptNote() {
    return this.http.get(this.baseUrl);
    /*     .subscribe((data: any) => {
          this.DataStore.next(data);
        }); */
  }
  UpdateGrnItemQuantity(model: any) {
    return this.http.post(this.baseUrl + 'quantity/updategrnitemquantity', model);
  }
  GetGoodReceptNoteHeader(status: boolean, receiptOrderId: number) {
    const token = localStorage.getItem('token');
    return this.http.get(this.baseUrl + 'getgoodreceiptheader/' + receiptOrderId, {
      headers: new HttpHeaders({ 'Content-Type': 'application/json', Authorization: 'Bearer ' + token, 'BranchLocationId': this.commonService.getUserBranchLocationId().toString() }),
      params: new HttpParams()
        .set('status', String(status))
        .set('BranchLocationId', this.commonService.getUserBranchLocationId().toString()),
    });
    /*     .subscribe((data: any) => {
          this.DataStore.next(data);
        }); */
  }
  CreateGoodReceptNoteHeader(model: any) {
    return this.http.post(this.baseUrl + 'Check/creategrnheader', model).pipe(
      catchError(this.handleError<[]>('')),
    );
  }
  RemoveGoodReceptNoteItemDetails(model: any) {
    return this.http.post(this.baseUrl + 'removegoodreceptnoteitemdetails', model);
  }
  UpdateGoodReceptNoteItemDetails(model: any) {
    return this.http.post(this.baseUrl + 'updategoodreceptnoteitemdetails', model);
  }
  Overwritestockpricegoodreceptnoteitemdetails(model: any) {
    return this.http.post(this.baseUrl + 'overwritestockpricegoodreceptnoteitemdetails', model);
  }
  DateUpdateGoodReceptNoteItemDetails(model: any) {
    return this.http.post(this.baseUrl + 'dateupdategoodreceptnoteitemdetails', model);
  }
  DiscountUpdateGoodReceptNoteItemDetails(model: any) {
    return this.http.post(this.baseUrl + 'discountupdategoodreceptnoteitemdetails', model);
  }
  VatUpdateGoodReceptNoteItemDetails(model: any) {
    return this.http.post(this.baseUrl + 'vatupdategoodreceptnoteitemdetails', model);
  }
  NbtUpdateGoodReceptNoteItemDetails(model: any) {
    return this.http.post(this.baseUrl + 'nbtupdategoodreceptnoteitemdetails', model);
  }
  GroupDiscountUpdateGoodReceptNoteItemDetails(model: any) {
    return this.http.post(this.baseUrl + 'groupdiscountupdategoodreceptnoteitemdetails', model);
  }
  GroupDiscountAmountUpdateGoodReceptNoteItemDetails(model: any) {
    return this.http.post(this.baseUrl + 'groupdiscountamountupdategoodreceptnoteitemdetails', model);
  }
  ClearGoodReceipt() {
    return this.http.get(this.baseUrl + 'clear');
  }
  getGoodReceiptHeaders(
    status = true,
    pageNumber = 1,
    pageSize = 5,
    filterString?: string
  ): Observable<PaginatedResult<GoodReceptNoteHeader[]>> {
    const token = localStorage.getItem('token');
    const paginatedResult: PaginatedResult<GoodReceptNoteHeader[]> = new PaginatedResult<GoodReceptNoteHeader[]>();
    const url = this.baseUrl + 'getallgoodreceiptheaders';
    return this.http
      .get<GoodReceptNoteHeader[]>(url, {
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
  getGoodReceiptDetails(
    status = true,
    pageNumber = 1,
    pageSize = 5,
    goodReceptNoteHeaderId: number,
  ): Observable<PaginatedResult<GoodReceptNoteDetail[]>> {
    const token = localStorage.getItem('token');
    const paginatedResult: PaginatedResult<GoodReceptNoteDetail[]> = new PaginatedResult<GoodReceptNoteDetail[]>();
    const url = this.baseUrl + 'getallgoodreceiptdetails';
    return this.http
      .get<GoodReceptNoteDetail[]>(url, {
        observe: 'response',
        headers: new HttpHeaders({ 'Content-Type': 'application/json', Authorization: 'Bearer ' + token, 'BranchLocationId': this.commonService.getUserBranchLocationId().toString() }),
        params: new HttpParams()
          .set('status', String(status))
          .set('PageNumber', pageNumber.toString())
          .set('PageSize', pageSize.toString())
          .set('GoodReceptNoteHeaderId', goodReceptNoteHeaderId.toString())
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
