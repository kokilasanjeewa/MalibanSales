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
import { PurchaseOrderHeader } from '../models/purchaseorderheader';
import { PurchaseorderService } from '../_services/purchaseorder.service';

@Injectable({
  providedIn: 'root',
})
export class PurchaseOrderListResolver implements Resolve<PurchaseOrderHeader[]> {
  status = true;
  pageNumber = 1;
  pageSize = 5;
  constructor(
    private purchaseorderService: PurchaseorderService,
    private commonService: CommonService,
    private alertifyService: AlertifyService,
    private router: Router
  ) { }
  resolve(route: ActivatedRouteSnapshot): Observable<PurchaseOrderHeader[]> {
    return this.purchaseorderService.getPurchaseOrderHeaders
      (this.status, this.pageNumber, this.pageSize,"")
      .pipe(
        catchError((error) => {
          this.router.navigate(['../purchase/purchaseorderlist']);
          this.alertifyService.error(error);
          return of(null);
        })
      );
  }
}
