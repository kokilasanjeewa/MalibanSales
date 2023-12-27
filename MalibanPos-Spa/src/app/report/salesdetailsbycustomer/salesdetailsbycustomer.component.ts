import { SaleReportDetailByCustomer } from './../../models/salereportdetailbycustomer';
import { PaginatedResult } from './../../models/pagination';
import { ReportService } from './../../_services/report.service';
import { CommonService } from './../../_services/common.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import * as fileSaver from 'file-saver';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
@Component({
  selector: 'app-salesdetailsbycustomer',
  templateUrl: './salesdetailsbycustomer.component.html',
  styleUrls: ['./salesdetailsbycustomer.component.css']
})
export class SalesdetailsbycustomerComponent implements OnInit {
  saleReportForm: FormGroup;
  saleReportDetails: SaleReportDetailByCustomer[];
  total: number;
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
      this.reportService.getReportsId<SaleReportDetailByCustomer>(status, 1, 5, frmDate, toDate, 'Sales Details By Customer', id,0).subscribe(
        (res: PaginatedResult<SaleReportDetailByCustomer[]>) => {
          this.saleReportDetails = res.result;
          this.total =this.saleReportDetails.reduce( (x, y) => {
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
          this.router.navigate(['/report/salesdetailsbycustomer']);
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
    const id = this.saleReportForm.get('customerCode').value;

    if (!this.saleReportForm.valid) {
      return false;
    } else {
      this.reportService.getExcelId(status, 1, 5, frmDate, toDate, 'Sales Details By Customer',0,id).
        subscribe(response => {
          let file = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
          fileSaver.saveAs(file, 'SalesDetailsByCustomer.xlsx');

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
