import { PurchaseRequestDetail } from './../models/purchaserequestdetail';
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

@Injectable({
  providedIn: 'root',
})
export class PurchaseRequestDetailResolver implements Resolve<PurchaseRequestDetail[]> {
  status = true;
  pageNumber = 1;
  pageSize = 5;
  constructor(
    private purchaseService: PurchaseService,
    private commonService: CommonService,
    private alertifyService: AlertifyService,
    private router: Router,
    private route: ActivatedRoute
  ) { }
  resolve(route: ActivatedRouteSnapshot): Observable<PurchaseRequestDetail[]> {
    console.log('resolver works');
    console.log(route.params['purchaseRequestHeaderId']);
    return this.purchaseService.getPurchaseRequestDetails
      (this.status, this.pageNumber, this.pageSize, route.params['purchaseRequestHeaderId'])
      .pipe(
        catchError((error) => {
          //  this.router.navigate(['../stock/']);
          this.alertifyService.error(error);
          return of(null);
        })
      );
  }
}
