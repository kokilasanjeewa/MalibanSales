import { PriceService } from './../_services/price.service';
import { SupplierPrice } from './../models/supplierprice';
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
export class SupplierPriceListResolver implements Resolve<SupplierPrice[]> {
  status = true;
  pageNumber = 1;
  pageSize = 5;
  constructor(
    private priceService: PriceService,
    private commonService: CommonService,
    private alertifyService: AlertifyService,
    private router: Router
  ) { }
  resolve(route: ActivatedRouteSnapshot): Observable<SupplierPrice[]> {
    return this.priceService
      .getSupplierPrices(this.status, this.pageNumber, this.pageSize,"")
      .pipe(
        catchError((error) => {
          this.router.navigate(['../supplier/']);
          this.alertifyService.error(error);
          return of(null);
        })
      );
  }
}
