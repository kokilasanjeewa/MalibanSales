import { PurchaseService } from '../_services/purchase.service';
import { CommonService } from 'src/app/_services/common.service';
import { Injectable } from '@angular/core';
import {
  ActivatedRoute,
  ActivatedRouteSnapshot,
  Resolve,
  Router,
} from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AlertifyService } from '../_services/alertify.service';
import { PurchaseOrderDetail } from '../models/purchaseorderdetail';
import { PurchaseorderService } from '../_services/purchaseorder.service';

@Injectable({
  providedIn: 'root',
})
export class PurchaseOrderDetailResolver implements Resolve<PurchaseOrderDetail[]> {
  status = true;
  pageNumber = 1;
  pageSize = 5;
  constructor(
    private purchaseorderService: PurchaseorderService,
    private commonService: CommonService,
    private alertifyService: AlertifyService,
    private router: Router,
    private route: ActivatedRoute
  ) { }
  resolve(route: ActivatedRouteSnapshot): Observable<PurchaseOrderDetail[]> {
    console.log('PurchaseOrderDetailResolver resolver works');
    console.log(route.params['purchaseOrderHeaderId']);
    return this.purchaseorderService.getPurchaseOrderDetails
      (this.status, this.pageNumber, this.pageSize, route.params['purchaseOrderHeaderId'])
      .pipe(
        catchError((error) => {
          //  this.router.navigate(['../stock/']);
          this.alertifyService.error(error);
          return of(null);
        })
      );
  }
}
