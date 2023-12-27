import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Observable, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { RolePermissionCustom } from '../models/rolepermissioncustom';

@Injectable({
  providedIn: 'root',
})
export class MenuService {
  jwtHelper = new JwtHelperService();
  baseUrl = environment.apiUrl ;
  decodedToken: any;
  constructor(private httpClient: HttpClient) {}
  // get/menu/{id}
  getMenuId(id: any) {
    return this.httpClient.get<RolePermissionCustom[]>(this.baseUrl + 'Menu/' + id);
  }
  
  createPermission(model: any) {
    return this.httpClient.post(this.baseUrl + 'Menu', model);
  }
  deletePermission(id: any) {
    return this.httpClient.delete(this.baseUrl + 'Menu/'+id);
  }
   updateCompany(model: any) {
    return this.httpClient.put(this.baseUrl + 'Menu', model);
  }
  getMenuById(apiName: string, methodName: string, id: number) {
     return this.httpClient
      .get<[]>(this.baseUrl + apiName + '/' + methodName.toLowerCase()+ '/'+id)
      .pipe(
        tap((_) => console.log(`fetch data`)),
        catchError(this.handleError<[]>(''))
      );
  }
  public getListByLocation(
    branchLocationId: number,
    apiName: string,
    methodName: string,
    barCode?: string
  ): Observable<any> {
    const opts = {
      params: new HttpParams({
        fromString:
          'status=false' +
          '&branchLocationId=' +
          branchLocationId +
          '&barCode=' +
          barCode,
      }),
    };
    /*     const params = new HttpParams()
          .set('status', 'false')
          .set('branchLocationId', 'branchLocationId'); */
    return this.httpClient
      .get<[]>(this.baseUrl + apiName + '/' + methodName.toLowerCase(), opts)
      .pipe(
        tap((_) => console.log(`fetch data`)),
        catchError(this.handleError<[]>(''))
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
