import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CommonService } from './common.service';
import { environment } from './../../environments/environment';
import { Observable } from 'rxjs';
import { PaginatedResult } from '../models/pagination';
import { map } from 'rxjs/operators';
import { ApplicationUser } from '../models/applicationuser';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  baseUrl = environment.apiUrl + 'Auth/';

  constructor(private http: HttpClient, private commonService: CommonService) {}

  getUsers(
    status = false,
    pageNumber = 1,
    pageSize = 5,
    filterString?: string,
    applicationUserId?:string,
  ): Observable<PaginatedResult<ApplicationUser[]>> {
    const token = localStorage.getItem('token');
    const paginatedResult: PaginatedResult<ApplicationUser[]> = new PaginatedResult<
    ApplicationUser[]
    >();
    const url = this.baseUrl + 'getallusers';
    return this.http
      .get<ApplicationUser[]>(url, {
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
          .set('Filter', filterString)
          .set('Id', applicationUserId?.toString())
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
}
