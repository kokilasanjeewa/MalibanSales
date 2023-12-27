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
import { StockadjustmentService } from '../_services/stockadjustment.service';
import { StockAdjustmentHeader } from '../models/stockadjustmentheader';

@Injectable({
  providedIn: 'root',
})
export class StockAdjustmentListResolver implements Resolve<StockAdjustmentHeader[]> {
  status = true;
  pageNumber = 1;
  pageSize = 5;
  constructor(
    private stockadjustmentService: StockadjustmentService,
    private commonService: CommonService,
    private alertifyService: AlertifyService,
    private router: Router
  ) { }
  resolve(route: ActivatedRouteSnapshot): Observable<StockAdjustmentHeader[]> {
    return this.stockadjustmentService
      .getStockAdjustmentHeaders(this.status, this.pageNumber, this.pageSize)
      .pipe(
        catchError((error) => {
          this.router.navigate(['../stock/']);
          this.alertifyService.error(error);
          return of(null);
        })
      );
  }
}
