import { PurchaseService } from './../_services/purchase.service';
import { PurchaseRequestHeader } from './../models/purchaserequestheader';
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

@Injectable({
  providedIn: 'root',
})
export class PurchaseRequestListResolver implements Resolve<PurchaseRequestHeader[]> {
  status = true;
  pageNumber = 1;
  pageSize = 5;
  constructor(
    private purchaseService: PurchaseService,
    private commonService: CommonService,
    private alertifyService: AlertifyService,
    private router: Router
  ) { }
  resolve(route: ActivatedRouteSnapshot): Observable<PurchaseRequestHeader[]> {
    return this.purchaseService.getPurchaseRequestHeaders
      (this.status, this.pageNumber, this.pageSize)
      .pipe(
        catchError((error) => {
          this.router.navigate(['../stock/']);
          this.alertifyService.error(error);
          return of(null);
        })
      );
  }
}
