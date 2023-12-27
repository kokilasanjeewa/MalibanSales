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
import { GoodReceptNoteDetail } from '../models/goodreceptnotedetail';
import { GrnService } from '../_services/grn.service';

@Injectable({
  providedIn: 'root',
})
export class GoodReceiptDetailResolver implements Resolve<GoodReceptNoteDetail[]> {
  status = true;
  pageNumber = 1;
  pageSize = 5;
  constructor(
    private grnService: GrnService,
    private commonService: CommonService,
    private alertifyService: AlertifyService,
    private router: Router,
    private route: ActivatedRoute
  ) { }
  resolve(route: ActivatedRouteSnapshot): Observable<GoodReceptNoteDetail[]> {
    console.log('GoodReceiptDetailResolver resolver works');
    console.log(route.params['goodReceptNoteHeaderId']);
    return this.grnService.getGoodReceiptDetails
      (this.status, this.pageNumber, this.pageSize, route.params['goodReceptNoteHeaderId'])
      .pipe(
        catchError((error) => {
          //  this.router.navigate(['../stock/']);
          this.alertifyService.error(error);
          return of(null);
        })
      );
  }
}
