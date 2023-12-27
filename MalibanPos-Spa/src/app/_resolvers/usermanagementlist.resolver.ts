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
import { UserService } from '../_services/user.service';
import { ApplicationUser } from '../models/applicationuser';

@Injectable({
  providedIn: 'root',
})
export class UserManagementListResolver implements Resolve<ApplicationUser[]> {
  status = true;
  pageNumber = 1;
  pageSize = 5;
  constructor(
    private userService: UserService,
    private commonService: CommonService,
    private alertifyService: AlertifyService,
    private router: Router
  ) { }
  resolve(route: ActivatedRouteSnapshot): Observable<ApplicationUser[]> {
    return this.userService
      .getUsers(this.status, this.pageNumber, this.pageSize,"",this.commonService.getUserId())
      .pipe(
        catchError((error) => {
          this.router.navigate(['../manage/']);
          this.alertifyService.error(error);
          return of(null);
        })
      );
  }
}
