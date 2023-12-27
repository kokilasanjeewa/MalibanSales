import { PaginatedResult } from './../../models/pagination';
import { PurchaseReturnReportDetail } from './../../models/purchasereturnreportdetail';
import { ReportService } from './../../_services/report.service';
import { CommonService } from './../../_services/common.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import * as fileSaver from 'file-saver';
import { formatDate } from '@angular/common';
import { Observable } from 'rxjs';

interface Post {
  startDate: Date;
  endDate: Date;
}
@Component({
  templateUrl: './purchasereturn.component.html',
  styleUrls: ['./purchasereturn.component.css']
})
export class PurchasereturnComponent implements OnInit {
  total : number;
  post: Post = {
    startDate: new Date(Date.now()),
    endDate: new Date(Date.now())
  };
  stockReportForm: FormGroup;
  shopstockDetail: PurchaseReturnReportDetail[];
  purchasereturns: Observable<[]>;
  constructor(
    private commonService: CommonService,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private toastrService: ToastrService,
    private reportService: ReportService
  ) {

  }

  ngOnInit() {
    this.createCustomerForm();
  }
  stockAdd() {
    const frmDate = this.stockReportForm.get('frmDate').value;
    const toDate = this.stockReportForm.get('toDate').value;
    const status = this.stockReportForm.get('active').value;
    const id = this.stockReportForm.get('purchasereturnId').value;

    if (!this.stockReportForm.valid) {
      return false;
    } else {
      this.reportService.getReportsId<PurchaseReturnReportDetail>(status, 1, 5, frmDate, toDate, 'Purchase Return', id).subscribe(
        (res: PaginatedResult<PurchaseReturnReportDetail[]>) => {
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
          this.router.navigate(['/report/purchasereturn']);
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
    const id = this.stockReportForm.get('purchasereturnId').value;

    if (!this.stockReportForm.valid) {
      return false;
    } else {
      this.reportService.getExcelId(status, 1, 5, frmDate, toDate, 'Purchase Return', id).
        subscribe(response => {
          let file = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
          fileSaver.saveAs(file, 'PurchaseReturn.xlsx');
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
      purchasereturnId: [''],
      active: ['false', [Validators.required]]
    });
  }
  searchPrn() {
    const frmDate = this.stockReportForm.get('frmDate').value;
    const toDate = this.stockReportForm.get('toDate').value;
    this.purchasereturns = this.commonService.getListByLocationFrmToDate(this.commonService.getUserBranchLocationId(), 'Common', 'getallprn', frmDate, toDate);

  }
}
