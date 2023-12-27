import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpParams, HttpClient } from '@angular/common/http';
import { BehaviorSubject, defer, Observable, of, Subject} from 'rxjs';
import { catchError, finalize, map, tap } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Branch } from '../models/branch';
import { BranchLocation } from '../models/branchlocation';
@Injectable({
  providedIn: 'root',
})
export class CommonService {
  jwtHelper = new JwtHelperService();
  baseUrl = environment.apiUrl;
  decodedToken: any;
  branchId: number;
  private DataStore = new BehaviorSubject<number>(null);
  public id: number =JSON.parse(localStorage.getItem('branchLocationId'));

  //private DataStore = new Subject<number>();

  data$: Observable<any> = this.DataStore.asObservable();
  constructor(private httpClient: HttpClient) { }
  public getList(
    apiName: string,
    methodName: string,
    status?: string
  ): Observable<any> {
    const opts = { params: new HttpParams({ fromString: 'status=false' }) };
    /*     const params = new HttpParams()
          .set('status', 'false')
          .set('branchLocationId', 'branchLocationId'); */
    return this.httpClient
      .get<[]>(
        this.baseUrl + apiName + '/' + methodName.toLowerCase(),
        opts
      )
      .pipe(
        tap((_) => console.log(`fetch data`)),
        catchError(this.handleError<[]>('')),
      );
  }
  public getListOption(
    apiName: string,
    methodName: string,
    status?: boolean
  ): Observable<any> {
    const opts = { params: new HttpParams({ fromString: 'status=' + status }) };
    /*     const params = new HttpParams()
          .set('status', 'false')
          .set('branchLocationId', 'branchLocationId'); */
    return this.httpClient
      .get<[]>(
        this.baseUrl + apiName + '/' + methodName.toLowerCase(),
        opts
      )
      .pipe(
        tap((_) => console.log(`fetch data`)),
        catchError(this.handleError<[]>('')),
      );
  }
  getListbyCode(apiName: string, methodName: string, provinceCode: string) {
    const opts = { params: new HttpParams({ fromString: 'status=false' + '&' + 'Code=' + provinceCode }) };
    return this.httpClient.get<[]>(this.baseUrl + apiName + '/' + methodName.toLowerCase(), opts).pipe(
      tap((_) => console.log(`fetch data`)),
      catchError(this.handleError<[]>('')),
    );
  }
  public getListByLocation(
    branchLocationId: number,
    apiName: string,
    methodName: string,
    barCode?: string,
  ): Observable<any> {
    const opts = { params: new HttpParams({ fromString: 'status=false' + '&branchLocationId=' + branchLocationId + '&barCode=' + barCode }) };
    /*     const params = new HttpParams()
          .set('status', 'false')
          .set('branchLocationId', 'branchLocationId'); */
    return this.httpClient
      .get<[]>(
        this.baseUrl + apiName + '/' + methodName.toLowerCase(),
        opts
      )
      .pipe(
        tap((_) => console.log(`fetch data`)),
        catchError(this.handleError<[]>('')),
      );
  }
  public getListByLocationFrmToDate(
    branchLocationId: number,
    apiName: string,
    methodName: string,
    frmDate: Date,
    toDate: Date,
  ): Observable<any> {
    const opts = { params: new HttpParams({ fromString: 'status=false' + '&branchLocationId=' + branchLocationId + '&fromDate=' + frmDate + '&toDate=' + toDate }) };
    /*     const params = new HttpParams()
          .set('status', 'false')
          .set('branchLocationId', 'branchLocationId'); */
    return this.httpClient
      .get<[]>(
        this.baseUrl + apiName + '/' + methodName.toLowerCase(),
        opts
      )
      .pipe(
        tap((_) => console.log(`fetch data`)),
        catchError(this.handleError<[]>('')),
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
  public getUser() {
    const token = localStorage.getItem('token');
    this.decodedToken = this.jwtHelper.decodeToken(token);
    console.log(this.decodedToken);
    return this.decodedToken.unique_name;
  }
  public getUserRoles() {
    const token = localStorage.getItem('token');
    this.decodedToken = this.jwtHelper.decodeToken(token);
    console.log(this.decodedToken);
    return this.decodedToken.role;
  }
  public getUserId() {
    const token = localStorage.getItem('token');
    this.decodedToken = this.jwtHelper.decodeToken(token);
    return this.decodedToken.nameid;
  }
  public getUserBranchLocationId() {
    const get = localStorage.getItem('branchLocationId');
      this.branchId = JSON.parse(get);
      this.DataStore.next(this.branchId);
    const token = localStorage.getItem('token');
   this.decodedToken = this.jwtHelper.decodeToken(token);
    return this.branchId ? this.branchId :this.decodedToken.BranchLocationId;
  }
  public getMapUserBranchLocations():number[] {
    const token = localStorage.getItem('token');
    this.decodedToken = this.jwtHelper.decodeToken(token);
    return JSON.parse(this.decodedToken.MapBranchLocations) as number[];
  }
   public getBranches():BranchLocation[]{
    let branch: any;
    this.getList('Common', 'getallbranch').subscribe(data => {
      const mapBranches=this.getMapUserBranchLocations().length >0 ?  this.getMapUserBranchLocations() : this.getUserBranchLocationId();
      const branches = mapBranches.length >1 ?
      data.filter(array => mapBranches.some(filter => filter == array.branchLocationId))
      : data.filter(d => d.branchLocationId==this.getUserBranchLocationId());
      return branch=branches
      //return branches;
    });
    return branch;
  }
  }
