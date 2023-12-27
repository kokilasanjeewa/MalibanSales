import { PaginatedResult } from './../../models/pagination';
import { ReportService } from './../../_services/report.service';
import { CommonService } from './../../_services/common.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SaleSummaryReportDetail } from 'src/app/models/salesummaryreportdetail';
import * as fileSaver from 'file-saver';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
@Component({
  selector: 'app-salesummarybycustomer',
  templateUrl: './salesummarybycustomer.component.html',
  styleUrls: ['./salesummarybycustomer.component.css'],
})
export class SalesummarybycustomerComponent implements OnInit {
  saleReportForm: FormGroup;
  saleReportDetails: SaleSummaryReportDetail[];
  total : number;
  active:boolean=false;

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
    const id = this.saleReportForm.get('customerCode').value;
    const frmDate = this.saleReportForm.get('frmDate').value;
    const toDate = this.saleReportForm.get('toDate').value;
    const status = this.saleReportForm.get('active').value;
    if (!this.saleReportForm.valid) {
      return false;
    } else {
      this.reportService.getReportsId<SaleSummaryReportDetail>(status, 1, 5, frmDate, toDate, 'Sales Summary By Customer', id,0).subscribe(
        (res: PaginatedResult<SaleSummaryReportDetail[]>) => {
          this.saleReportDetails = res.result;
          this.total =this.saleReportDetails.reduce( (x, y) => {
            return x + (y.totalAmount-y.customerReturnAmount);
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
          this.router.navigate(['/report/salesummarybycustomer']);
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
      this.reportService.getExcel(status, 1, 5, frmDate, toDate, 'Sales Summary Details').
        subscribe(response => {
          let file = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
          fileSaver.saveAs(file, 'SalesSummaryDetails.xlsx');

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
      customerCode: ['', [Validators.required]],
      frmDate: ['', [Validators.required]],
      toDate: ['', [Validators.required]],
      active: [this.active, [Validators.required]]
    });
  }
}
