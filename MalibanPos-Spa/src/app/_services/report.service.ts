import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CommonService } from './common.service';
import { environment } from './../../environments/environment';
import { Observable, pipe } from 'rxjs';
import { PaginatedResult } from '../models/pagination';
import { map } from 'rxjs/operators';
import { POListCriteria } from '../models/polistcriteria';
@Injectable({
  providedIn: 'root',
})
export class ReportService {
  baseUrl = environment.apiUrl + 'Report/';
  constructor(private http: HttpClient, private commonService: CommonService) { }
  getReports<T>(
    status = true,
    pageNumber = 1,
    pageSize = 5,
    frmDate: Date,
    toDate: Date,
    reportName: string
  ): Observable<PaginatedResult<T[]>> {
    const token = localStorage.getItem('token');
    const paginatedResult: PaginatedResult<T[]> = new PaginatedResult<T[]>();
    const url = this.baseUrl + 'getreportdetails';
    return this.http
      .get<T[]>(url, {
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
          .set('FromDate', frmDate.toString())
          .set('ToDate', toDate.toString())
          .set('reportName', reportName)
          .set(
            'branchLocationId',
            this.commonService.getUserBranchLocationId().toString()
          ),
      })
      .pipe(
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
  getReportsId<T>(
    status = true,
    pageNumber = 1,
    pageSize = 5,
    frmDate: Date,
    toDate: Date,
    reportName: string,
    id?: number,
    schemeCode?: number,
    pOListCriteria?: string
  ): Observable<PaginatedResult<T[]>> {

    const token = localStorage.getItem('token');
    const paginatedResult: PaginatedResult<T[]> = new PaginatedResult<T[]>();
    const url = this.baseUrl + 'getreportdetails';
    return this.http
      .get<T[]>(url, {
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
          .set('FromDate', frmDate.toString())
          .set('ToDate', toDate.toString())
          .set('reportName', reportName)
          .set('schemeCode', schemeCode.toString())
          .set('pOListCriteria',pOListCriteria)
          .set('branchLocationId',this.commonService.getUserBranchLocationId().toString())
          .set('id', id.toString()),
      })
      .pipe(
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
  getExcel(
    status = true,
    pageNumber = 1,
    pageSize = 5,
    frmDate: Date,
    toDate: Date,
    reportName: string
  ) {
    const token = localStorage.getItem('token');
    const url = this.baseUrl + 'getexcelreport';
    return this.http.get(url, {
      observe: 'body',
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
        .set('FromDate', frmDate.toString())
        .set('ToDate', toDate.toString())
        .set('reportName', reportName)
        .set('fileName', reportName)
        .set(
          'branchLocationId',
          this.commonService.getUserBranchLocationId().toString()
        ),
      responseType: 'arraybuffer',
    });
  }
  getExcelId(
    status = true,
    pageNumber = 1,
    pageSize = 5,
    frmDate: Date,
    toDate: Date,
    reportName: string,
    schemeCode?: number,
    id?: string
  ) {
    const token = localStorage.getItem('token');
    const url = this.baseUrl + 'getexcelreport';
    return this.http.get(url, {
      observe: 'body',
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
        .set('FromDate', frmDate.toString())
        .set('ToDate', toDate.toString())
        .set('reportName', reportName)
        .set('schemeCode', schemeCode.toString())
        .set('fileName', reportName)
        .set(
          'branchLocationId',
          this.commonService.getUserBranchLocationId().toString()
        )
        .set('id', id),
      responseType: 'arraybuffer',
    });
  }
}
