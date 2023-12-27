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
import { GrnService } from '../_services/grn.service';
import { GoodReceptNoteHeader } from '../models/goodreceptnoteheader';

@Injectable({
  providedIn: 'root',
})
export class GoodReceiptListResolver implements Resolve<GoodReceptNoteHeader[]> {
  status = true;
  pageNumber = 1;
  pageSize = 5;
  constructor(
    private grnService: GrnService,
    private commonService: CommonService,
    private alertifyService: AlertifyService,
    private router: Router
  ) { }
  resolve(route: ActivatedRouteSnapshot): Observable<GoodReceptNoteHeader[]> {
    return this.grnService.getGoodReceiptHeaders
      (this.status, this.pageNumber, this.pageSize,"")
      .pipe(
        catchError((error) => {
          this.router.navigate(['../grn/grnlist']);
          this.alertifyService.error(error);
          return of(null);
        })
      );
  }
}
