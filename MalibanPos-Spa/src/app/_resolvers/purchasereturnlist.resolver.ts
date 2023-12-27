import { PurchaseReturnHeader } from './../models/purchasereturnheader';
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
import { PurchasereturnService } from '../_services/purchasereturn.service';

@Injectable({
  providedIn: 'root',
})
export class PurchaseReturnListResolver implements Resolve<PurchaseReturnHeader[]> {
  status = true;
  pageNumber = 1;
  pageSize = 5;
  constructor(
    private purchasereturnService: PurchasereturnService,
    private commonService: CommonService,
    private alertifyService: AlertifyService,
    private router: Router
  ) { }
  resolve(route: ActivatedRouteSnapshot): Observable<PurchaseReturnHeader[]> {
    return this.purchasereturnService.getPurchaseReturnHeaders
      (this.status, this.pageNumber, this.pageSize)
      .pipe(
        catchError((error) => {
          this.router.navigate(['../purchase/purchasereturnlist']);
          this.alertifyService.error(error);
          return of(null);
        })
      );
  }
}
