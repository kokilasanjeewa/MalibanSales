import { HttpInterceptor, HttpErrorResponse, HTTP_INTERCEPTORS, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { Observable, throwError } from 'rxjs';
import { Injectable } from '@angular/core';


@Injectable()

export class ConnectionInterceptor implements HttpInterceptor {
  constructor() {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (!window.navigator.onLine) {
      // if there is no internet, throw a HttpErrorResponse error
      // since an error is thrown, the function will terminate here
      const error = {
        status: 0,
        error: {
          description: 'Check Connectivity!'
        },
        statusText: 'Check Connectivity!'
      };
      return throwError(new HttpErrorResponse(error));
    } else {
      // else return the normal request
      return next.handle(req);
    }
 }

}
export const ErrorInterceptorProvider = {
  provide: HTTP_INTERCEPTORS,
  useClass: ConnectionInterceptor,
  multi: true
};
