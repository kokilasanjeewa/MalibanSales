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

@Injectable({
  providedIn: 'root',
})
export class SupplierListResolver implements Resolve<Supplier[]> {
  status = true;
  pageNumber = 1;
  pageSize = 5;
  constructor(
    private supplierService: SupplierService,
    private commonService: CommonService,
    private alertifyService: AlertifyService,
    private router: Router
  ) { }
  resolve(route: ActivatedRouteSnapshot): Observable<Supplier[]> {
    return this.supplierService
      .getSuppliers(this.status, this.pageNumber, this.pageSize, "")
      .pipe(
        catchError((error) => {
          this.router.navigate(['../supplier/']);
          this.alertifyService.error(error);
          return of(null);
        })
      );
  }
}
