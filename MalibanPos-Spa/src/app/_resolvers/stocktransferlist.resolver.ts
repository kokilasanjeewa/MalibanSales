import { CommonService } from 'src/app/_services/common.service';
import { SupplierService } from '../_services/supplier.service';
import { Supplier } from 'src/app/models/supplier';
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
import { StockTransferHeader, StocktransferService } from '../_services/stocktransfer.service';

@Injectable({
  providedIn: 'root',
})
export class StockTransferListResolver implements Resolve<StockTransferHeader[]> {
  status = true;
  pageNumber = 1;
  pageSize = 5;
  constructor(
    private supplierService: SupplierService,
    private stocktransferService: StocktransferService,
    private commonService: CommonService,
    private alertifyService: AlertifyService,
    private router: Router
  ) { }
  resolve(route: ActivatedRouteSnapshot): Observable<StockTransferHeader[]> {
    return this.stocktransferService
      .getStockTransferLIst(this.status, this.pageNumber, this.pageSize, "")
      .pipe(
        catchError((error) => {
          this.router.navigate(['../stock/stocktransferlist']);
          this.alertifyService.error(error);
          return of(null);
        })
      );
  }
}

