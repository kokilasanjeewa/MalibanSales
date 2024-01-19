import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { StockTransferHeader, StocktransferService } from 'src/app/_services/stocktransfer.service';


@Component({
  selector: 'app-stocktransferreport',
  templateUrl: './stocktransferreport.component.html',
  styleUrls: ['./stocktransferreport.component.css']
})
export class StocktransferreportComponent implements OnInit,OnDestroy {
  stockTransferHeaderId: number;
  stockTransferHeader: StockTransferHeader;
  total: number;
  private destroyed$ = new Subject();

  constructor(
    private route: ActivatedRoute,
    private alertifyService: AlertifyService,
    private stocktransferService: StocktransferService,
  ) {}
ngOnInit() {
  localStorage.removeItem("postData");
  this.stockTransferHeaderId = +this.route.snapshot.params.stockTransferHeaderId;
  this.stocktransferService
    .getStockTransferHeader(true, +this.stockTransferHeaderId)
    .pipe(takeUntil(this.destroyed$))
    .subscribe({
      next: (res: StockTransferHeader) => {
        this.stockTransferHeader = res;
        // console.log(res);
      },
      error: () => {},
      complete: () => {},
    });
}
  ngOnDestroy() {
    this.destroyed$.next();
    this.destroyed$.complete();
  }
}
