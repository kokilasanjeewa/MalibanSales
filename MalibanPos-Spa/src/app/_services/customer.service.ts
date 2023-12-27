import { Customer } from './../models/customer';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { BehaviorSubject, Observable, of, ReplaySubject, Subject } from 'rxjs';
import { catchError, map,tap } from 'rxjs/operators';
import { environment } from './../../environments/environment';
import { AlertifyService } from './../_services/alertify.service';
import { ToastrService } from 'ngx-toastr';
import { MI_Employer_Employee } from '../models/mI_employer_employee';
import { PaginatedResult } from '../models/pagination';
import { CommonService } from './common.service';
import { MI_Employer_Employees } from '../models/mI_employer_employees';
import { EmployeeDto } from '../models/employeedto';
import { ReportDate } from '../models/reportdate';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  //private DataStore = new ReplaySubject<[]>(null);
 // private DataStore = new Subject<[]>();

 // data$: Observable<any> = this.DataStore.asObservable();
  baseUrl = environment.apiUrl + 'Customer/';

  constructor(private http: HttpClient,
    private alertifyService: AlertifyService,
    private commonService : CommonService,
    private toastrService: ToastrService) { }
    createCustomer(model: any) {
      return this.http.post(this.baseUrl + 'initialemployee', model);
    }
    uploadExcel(model: any) {
      return this.http.post(this.baseUrl + 'uploadexcel', model);
    }
    getCustomerById(customerId?: any): Observable<MI_Employer_Employees> {
      const url = this.baseUrl;
      return this.http.get<MI_Employer_Employees>(url + customerId).pipe(
        tap((_) => console.log(`fetched Customer getCustomerById=${customerId}`)),
        catchError(
          this.handleError<MI_Employer_Employees>('getCustomerById customerId=${customerId}')
        )
      );
    }
    updateCustomer(model: any, id: any) {
      return this.http.put(this.baseUrl + id, model);
    }
  GetCustomer(empcode: any, branchlocationId: number):Observable<any> {
    return this.http.get<any>(this.baseUrl + branchlocationId + '/' + empcode)/* .subscribe((data: any) => {
     // this.DataStore.next(data);
    }, (error) => {
      this.alertifyService.error(error);
    }, () => {
      //  this.alertifyService.success('Customer add successfully.');
      this.toastrService.success('Customer add successfully.');
    }); */
  }
  SearchCustomer(empcode: any, branchCode: number):Observable<any> {
    return this.http.get<EmployeeDto[]>(this.baseUrl+'search/' + branchCode + '/' + empcode)/* .subscribe((data: any) => {
     // this.DataStore.next(data);
    }, (error) => {
      this.alertifyService.error(error);
    }, () => {
      //  this.alertifyService.success('Customer add successfully.');
      this.toastrService.success('Customer add successfully.');
    }); */
  }
  SearchDate(schemeCode: any, branchCode: number):Observable<any> {
    return this.http.get<ReportDate[]>(this.baseUrl+'searchdate/' + branchCode + '/' + schemeCode)
  }
  GetMapCustomer(branchlocationId: number):Observable<EmployeeDto[]> {
    return this.http.get<EmployeeDto[]>(this.baseUrl+'searchmap/' + branchlocationId).pipe(
      tap(data => console.log(data)),
      catchError(this.handleError<any>(`err`))
    );/* .subscribe((data: any) => {
     // this.DataStore.next(data);
    }, (error) => {
      this.alertifyService.error(error);
    }, () => {
      //  this.alertifyService.success('Customer add successfully.');
      this.toastrService.success('Customer add successfully.');
    }); */
  }
  GetCustomerBySaleHeaderId(saleHeaderId: number,branchlocationId: number) {
    return this.http.get<Customer>(this.baseUrl +"employee/"+saleHeaderId + '/' +branchlocationId);
  }
  GetCustomerBySaleHeaderIdAny(saleHeaderId: number,branchlocationId: number) {
    return this.http.get<any>(this.baseUrl +"employee/"+saleHeaderId + '/' +branchlocationId);
  }
  PostMapEmployeeByempCode(model: any) {
    return this.http.post(this.baseUrl + 'postmapemployeebyempcode', model);
  }
   private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead
      this.toastrService.error(error);
      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }
  getCustomers(
   // status = true,
    pageNumber = 1,
    pageSize = 5,
    filterString?: string
  ): Observable<PaginatedResult<MI_Employer_Employee[]>> {
    const token = localStorage.getItem('token');
    const paginatedResult: PaginatedResult<MI_Employer_Employee[]> = new PaginatedResult<MI_Employer_Employee[]>();
    const url = this.baseUrl + 'getallcustomers';
    return this.http
      .get<MI_Employer_Employee[]>(url, {
        observe: 'response',
        headers: new HttpHeaders({ 'Content-Type': 'application/json', Authorization: 'Bearer ' + token, 'BranchLocationId': this.commonService.getUserBranchLocationId().toString() }),
        params: new HttpParams()
         // .set('status', String(status))
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
}
