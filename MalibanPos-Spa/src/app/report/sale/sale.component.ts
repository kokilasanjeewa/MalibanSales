import { SaleReportDetail } from './../../models/salereportdetail';
import { Component, OnInit } from '@angular/core';
import { PaginatedResult } from './../../models/pagination';
import { ShopStockDetail } from './../../models/shopstockdetail';
import { ReportService } from './../../_services/report.service';
import { CommonService } from './../../_services/common.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import * as fileSaver from 'file-saver';
@Component({
  templateUrl: './sale.component.html',
  styleUrls: ['./sale.component.css']
})
export class SaleComponent implements OnInit {
  total : number;
  totalStock : number;
  active:boolean=false;
  saleReportForm: FormGroup;
  saleReportDetails: SaleReportDetail[];
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
      this.reportService.getReports<SaleReportDetail>(status, 1, 5, frmDate, toDate, 'Sales Details').subscribe(
        (res: PaginatedResult<SaleReportDetail[]>) => {
          this.saleReportDetails = res.result;
          this.total =this.saleReportDetails.reduce( (x, y) => {
            return x + y.saleValue;
        }, 0);
        this.totalStock =this.saleReportDetails.reduce( (x, y) => {
          return x + y.stockValue;
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
          this.router.navigate(['/report/sale']);
        }
      );
    }
  }
  returnBlob(res): any {
    return new Blob([JSON.parse(res.headers.get('Content-Length'))], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
  }
  stockexcel() {
    const frmDate = this.saleReportForm.get('frmDate').value;
    const toDate = this.saleReportForm.get('toDate').value;
    const status = this.saleReportForm.get('active').value;
    if (!this.saleReportForm.valid) {
      return false;
    } else {
      this.reportService.getExcel(status, 1, 5, frmDate, toDate, 'Sales Details').
        subscribe(response => {
          let file = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
          fileSaver.saveAs(file, 'SaleDetails.xlsx');

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
