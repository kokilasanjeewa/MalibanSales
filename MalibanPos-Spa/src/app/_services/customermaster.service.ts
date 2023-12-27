import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { PaginatedResult } from '../models/pagination';
import { CommonService } from './common.service';
import { environment } from './../../environments/environment';
import { CustomerMasterSetting } from '../models/customermastersetting';

@Injectable({
  providedIn: 'root'
})
export class CustomermasterService {
  baseUrl = environment.apiUrl + 'CustomerMasterSetting/';
  /*   private DataStore = new ReplaySubject<[]>(null);
    data$: Observable<any> = this.DataStore.asObservable(); */
  constructor(private http: HttpClient, private commonService: CommonService) {
}
Createcustomermastersetting(model: any) {
  return this.http.post(this.baseUrl + 'createcustomermastersetting', model);
}
updateCustomermastersetting(model: any, id: any) {
  return this.http.put(this.baseUrl + id, model);
}
getCustomermastersetting(id: any) {
  return this.http.get(this.baseUrl + id );
}
getAllCustomerMasterSetting(
  status = true,
  pageNumber = 1,
  pageSize = 5,
  filterString?: string,
): Observable<PaginatedResult<CustomerMasterSetting[]>> {
  const token = localStorage.getItem('token');
  const paginatedResult: PaginatedResult<CustomerMasterSetting[]> = new PaginatedResult<CustomerMasterSetting[]>();
  const url = this.baseUrl + 'getallcustomermastersetting';
  return this.http
    .get<CustomerMasterSetting[]>(url, {
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
private handleError<T>(operation = 'operation', result?: T) {
  return (error: any): Observable<T> => {
    // TODO: send the error to remote logging infrastructure
    console.error(error); // log to console instead
    // Let the app keep running by returning an empty result.
    return of(result as T);
  };
}
}
