import { Shopstock } from './../models/shopstock';
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
import { ShopstockService } from '../_services/shopstock.service';
import { TransactionDetail } from '../models/transactiondetail';

@Injectable({
  providedIn: 'root',
})
export class ShopStockListResolver implements Resolve<TransactionDetail[]> {
  status = true;
  pageNumber = 1;
  pageSize = 5;
  constructor(
    private shopstockService: ShopstockService,
    private commonService: CommonService,
    private alertifyService: AlertifyService,
    private router: Router
  ) { }
  resolve(route: ActivatedRouteSnapshot): Observable<TransactionDetail[]> {
    return this.shopstockService
      .getStockDetails(this.status, this.pageNumber, this.pageSize)
      .pipe(
        catchError((error) => {
          this.router.navigate(['../stock/']);
          this.alertifyService.error(error);
          return of(null);
        })
      );
  }
}
