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
import { StockadjustmentService } from '../_services/stockadjustment.service';
import { StockAdjustmentDetail } from '../models/stockadjustmentdetail';

@Injectable({
  providedIn: 'root',
})
export class StockAdjustmentDetailResolver implements Resolve<StockAdjustmentDetail[]> {
  status = true;
  pageNumber = 1;
  pageSize = 5;
  constructor(
    private stockadjustmentService: StockadjustmentService,
    private commonService: CommonService,
    private alertifyService: AlertifyService,
    private router: Router,
    private route: ActivatedRoute
  ) { }
  resolve(route: ActivatedRouteSnapshot): Observable<StockAdjustmentDetail[]> {
    console.log('StockaAdjustmentDetailResolver resolver works');
    console.log(route.params['stockadjustmentHeaderId']);
    return this.stockadjustmentService.getStockAdjustmentDetails
      (this.status, this.pageNumber, this.pageSize, route.params['stockadjustmentHeaderId'])
      .pipe(
        catchError((error) => {
          //  this.router.navigate(['../stock/']);
          this.alertifyService.error(error);
          return of(null);
        })
      );
  }
}
