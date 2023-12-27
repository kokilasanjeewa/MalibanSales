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
import { CalculationMaster } from '../models/calculationmaster';
import { CustomermasterService } from '../_services/customermaster.service';

@Injectable({
  providedIn: 'root',
})
export class CustomerMasterListResolver implements Resolve<CalculationMaster[]> {
  status = true;
  pageNumber = 1;
  pageSize = 5;
  constructor(
    private customerService:CustomermasterService ,
    private alertifyService: AlertifyService,
    private router: Router
  ) { }
  resolve(route: ActivatedRouteSnapshot): Observable<CalculationMaster[]> {
    return this.customerService.getAllCustomerMasterSetting(this.status, this.pageNumber, this.pageSize,"")
      .pipe(
        catchError((error) => {
          this.router.navigate(['../setting/']);
          this.alertifyService.error(error);
          return of(null);
        })
      );
  }
}
