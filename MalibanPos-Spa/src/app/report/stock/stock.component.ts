import { ShopStockDetail } from './../../models/shopstockdetail';
import { TransactionDetail } from './../../models/transactiondetail';
import { PaginatedResult } from './../../models/pagination';
import { ReportService } from './../../_services/report.service';
import { CommonService } from './../../_services/common.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { formatDate } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import * as fileSaver from 'file-saver';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
interface Post {
  startDate: Date;
  endDate: Date;
}
@Component({
  selector: 'app-stock',
  templateUrl: './stock.component.html',
  styleUrls: ['./stock.component.css']
})
export class StockComponent implements OnInit {
  total: number;
  post: Post = {
    startDate: new Date(Date.now()),
    endDate: new Date(Date.now())
  };
  stockReportForm: FormGroup;
  shopstockDetail: ShopStockDetail[];
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
  stockAdd() {
    const frmDate = this.stockReportForm.get('frmDate').value;
    const toDate = this.stockReportForm.get('toDate').value;
    const status = this.stockReportForm.get('active').value;
    if (!this.stockReportForm.valid) {
      return false;
    } else {
      this.reportService.getReports<ShopStockDetail>(status, 1, 5, frmDate, toDate, 'Stock Details')
      .pipe(takeUntil(this.destroyed$))
      .subscribe(
        (res: PaginatedResult<ShopStockDetail[]>) => {
          this.shopstockDetail = res.result;
          this.total =this.shopstockDetail.reduce( (x, y) => {
            return x + y.saleValue;
        }, 0);
        },
        (error) => {
          this.toastrService.error(
            JSON.stringify(error).replace('"', '').replace('"', '')
          );
          console.log(error.error);
        },
        () => {
          this.toastrService.success('Data Loading Successfully.');
          this.router.navigate(['/report/stock']);
        }
      );
    }
  }
  returnBlob(res): any {
    return new Blob([JSON.parse(res.headers.get('Content-Length'))], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
  }
  stockexcel() {
    const frmDate = this.stockReportForm.get('frmDate').value;
    const toDate = this.stockReportForm.get('toDate').value;
    const status = this.stockReportForm.get('active').value;
    if (!this.stockReportForm.valid) {
      return false;
    } else {
      this.reportService.getExcel(status, 1, 5, frmDate, toDate, 'Stock Details')
      .pipe(takeUntil(this.destroyed$))
      .subscribe(response => {
          let file = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
          fileSaver.saveAs(file, 'StockDetails.xlsx');

        },
          error => {
            console.error(`Error: ${error.message}`);
          });
    }
  }
  get f() {
    return this.stockReportForm.controls;
  }
  createCustomerForm() {
    this.stockReportForm = this.fb.group({
      frmDate: [formatDate(this.post.startDate, 'yyyy-MM-dd', 'en'), [Validators.required]],
      toDate: [formatDate(this.post.startDate, 'yyyy-MM-dd', 'en'), [Validators.required]],
      active: ['false', [Validators.required]]
    });
  }
}
