import { CommonService } from 'src/app/_services/common.service';
import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  Resolve,
  Router,
  RouterStateSnapshot,
} from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AlertifyService } from '../_services/alertify.service';
import { CustomerService } from '../_services/customer.service';
import { MI_Employer_Employee } from '../models/mI_employer_employee';

@Injectable({
  providedIn: 'root',
})
export class CustomerListResolver implements Resolve<MI_Employer_Employee[]> {
  //status = true;
  pageNumber = 1;
  pageSize = 5;
  constructor(
    private customerService: CustomerService,
    private commonService: CommonService,
    private alertifyService: AlertifyService,
    private router: Router
  ) { }
  resolve(route: ActivatedRouteSnapshot): Observable<MI_Employer_Employee[]> {
    return this.customerService
      .getCustomers(this.pageNumber, this.pageSize, "")
      .pipe(
        catchError((error) => {
          this.router.navigate(['../customer/']);
          this.alertifyService.error(error);
          return of(null);
        })
      );
  }
}
