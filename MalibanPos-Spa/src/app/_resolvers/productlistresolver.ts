import { CommonService } from 'src/app/_services/common.service';
import { ProductService } from '../_services/product.service';
import { Product } from 'src/app/models/product';
import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  Resolve,
  Router,
  RouterStateSnapshot,
} from '@angular/router';
import { Observable, Subject, of } from 'rxjs';
import { catchError, takeUntil } from 'rxjs/operators';
import { AlertifyService } from '../_services/alertify.service';

@Injectable({
  providedIn: 'root',
})
export class ProductListResolver implements Resolve<Product[]> {
  status = true;
  pageNumber = 1;
  pageSize = 5;
  constructor(
    private productService: ProductService,
    private commonService: CommonService,
    private alertifyService: AlertifyService,
    private router: Router
  ) { }
  resolve(route: ActivatedRouteSnapshot): Observable<Product[]> {
    return this.productService
      .getProducts(this.status, this.pageNumber, this.pageSize, "")
      .pipe(
          catchError((error) => {
          this.router.navigate(['../product/']);
          this.alertifyService.error(error);
          return of(null);
        })
      );
  }
}
