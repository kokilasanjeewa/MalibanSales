import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PaginatedResult } from 'src/app/models/pagination';
import { CommonService } from 'src/app/_services/common.service';
import { ReportService } from 'src/app/_services/report.service';
import { ToastrService } from 'ngx-toastr';
import * as fileSaver from 'file-saver';
import { ProductService } from 'src/app/_services/product.service';
import { GetBinByItemCodeDto } from 'src/app/models/gettransactionbyItemcodedto';
import { formatDate } from '@angular/common';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { GetStockTransferByDateRangeDto } from 'src/app/_services/stocktransfer.service';
@Component({
  selector: 'app-stocktransfer',
  templateUrl: './stocktransfer.component.html',
  styleUrls: ['./stocktransfer.component.css']
})
export class StocktransferComponent implements OnInit {
  saleReportForm: FormGroup;
  stockTransferDetails: GetStockTransferByDateRangeDto[];
  active:boolean=false;
  private destroyed$ = new Subject();
  constructor(
    private commonService: CommonService,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private toastrService: ToastrService,
    private reportService: ReportService
  ) { }

  ngOnInit() {
    this.createCustomerForm();
  }
  saleAdd() {
    const frmDate = this.saleReportForm.get('frmDate').value;
    const toDate = this.saleReportForm.get('toDate').value;
    const status = this.saleReportForm.get('active').value;

    if (!this.saleReportForm.valid) {
      return false;
    } else {
        this.reportService.getReportsId<GetStockTransferByDateRangeDto>(status, 1, 5, frmDate, toDate, 'Stock Transfer',0,0)
        .pipe(takeUntil(this.destroyed$))
        .subscribe(
          (res: PaginatedResult<GetStockTransferByDateRangeDto[]>) => {
            console.log(res)
            this.stockTransferDetails = res.result;
          },
          (error) => {
            this.toastrService.error(
              JSON.stringify(error).replace('"', '').replace('"', '')
            );
            console.log(error.error);
          },
          () => {
            this.toastrService.success('Data Loading Successfully.');
            this.router.navigate(['/report/stocktransfer']);
          }
        );
      }

  }
  returnBlob(res): any {
    return new Blob([JSON.parse(res.headers.get('Content-Length'))], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
  }
  stockTransferexcel() {
    const frmDate = this.saleReportForm.get('frmDate').value;
    const toDate = this.saleReportForm.get('toDate').value;
    const status = this.saleReportForm.get('active').value;
    if (!this.saleReportForm.valid) {
      return false;
    } else {
      // alert(JSON.stringify(this.saleReportForm.value));
      this.reportService.getExcelId(status, 1, 5, frmDate, toDate, 'Stock Transfer',0,'')
      .pipe(takeUntil(this.destroyed$))
      .subscribe(response => {
          let file = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
          fileSaver.saveAs(file, 'StockTransfer'+  formatDate(new Date(), 'yyyy-MM-dd hh:mm:ss', 'en').toString()+'.xlsx');

        },
          error => {
            console.error(`Error: ${error.message}`);
          });
    }
  }
  get f() {
    return this.saleReportForm.controls;
  }
  createCustomerForm() {
    this.saleReportForm = this.fb.group({
      frmDate: ['', [Validators.required]],
      toDate: ['', [Validators.required]],
      active: [this.active, [Validators.required]]
    });
  }
}

/* Date
Trans Type
Factory
DocNo
Approve Status
Receive Status
Item Code
Transfer Qty
Unit Price
Sale Price
Sale Value
Stock Value */
