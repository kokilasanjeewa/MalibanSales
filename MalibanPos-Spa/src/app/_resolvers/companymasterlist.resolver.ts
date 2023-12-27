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
import { CalculationMaster } from '../models/calculationmaster';
import { CompanymastersettingService } from '../_services/companymastersetting.service';

@Injectable({
  providedIn: 'root',
})
export class CompanyMasterListResolver implements Resolve<CalculationMaster[]> {
  status = true;
  pageNumber = 1;
  pageSize = 5;
  constructor(
    private companyService: CompanymastersettingService,
    private commonService: CommonService,
    private alertifyService: AlertifyService,
    private router: Router
  ) { }
  resolve(route: ActivatedRouteSnapshot): Observable<CalculationMaster[]> {
    return this.companyService.getAllCompanyMasterSetting(this.status, this.pageNumber, this.pageSize,"")
      .pipe(
        catchError((error) => {
          this.router.navigate(['../setting/']);
          this.alertifyService.error(error);
          return of(null);
        })
      );
  }
}
