import { AuthService } from './../../_services/auth.service';
import { PaginatedResult } from './../../models/pagination';
import { CommonService } from './../../_services/common.service';
import { ReportService } from './../../_services/report.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CustomerDetail } from '../../models/customerdetail';
import * as fileSaver from 'file-saver';
import { Observable } from 'rxjs';
import { AlertifyService } from './../../_services/alertify.service';
import { JwtHelperService } from '@auth0/angular-jwt';
@Component({
  selector: 'app-customerunpaid',
  templateUrl: './customerunpaid.component.html',
  styleUrls: ['./customerunpaid.component.css']
})
export class CustomerunpaidComponent implements OnInit {
  customerReportForm: FormGroup;
  customerDetail: CustomerDetail[];
  schemes: Observable<[]>;
  jwtHelper = new JwtHelperService();
  total : number;
  active:boolean=false;

  constructor(
    private commonService: CommonService,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private authService: AuthService,
    private toastrService: ToastrService,
    private reportService: ReportService,
    private alertifyService: AlertifyService,
  ) {
    this.schemes = this.commonService.getList('Common', 'getallscheme');
   }

  ngOnInit() {
    this.createCustomerForm();
   }
  customerAdd() {
    const frmDate = this.customerReportForm.get('frmDate').value;
    const toDate = this.customerReportForm.get('toDate').value;
    const status = this.customerReportForm.get('active').value;
    const id = this.customerReportForm.get('schemeCode').value;
    if (!this.customerReportForm.valid) {
      return false;
    } else {
      this.reportService.getReportsId<CustomerDetail>(status, 1, 5, frmDate, toDate, 'Customer Deduction Unpaid Details',0,id).subscribe(
        (res: PaginatedResult<CustomerDetail[]>) => {
          this.customerDetail = res.result;
          this.total =this.customerDetail.reduce( (x, y) => {
            return x + y.total;
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
          this.router.navigate(['/report/unpaidcustomer']);
        }
      );
    }
  }
  returnBlob(res): any {
    return new Blob([JSON.parse(res.headers.get('Content-Length'))], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
  }
  customerpayment() {
    const frmDate = this.customerReportForm.get('frmDate').value;
    const toDate = this.customerReportForm.get('toDate').value;
    const status = this.customerReportForm.get('active').value;
    const id = this.customerReportForm.get('schemeCode').value;
    if (!this.customerReportForm.valid) {
      return false;
    } else {

      this.alertifyService.confirm("Please Confirm.", "Are you sure you want to Month end Process?", () => {
        this.reportService.getExcelId(status, 1, 5, frmDate, toDate, 'Customer Deduction Process',id,this.commonService.getUserId()).
        subscribe(response => {
          let file = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
          fileSaver.saveAs(file, 'CustomerDeductionDetails.xlsx');
          this.alertifyService.success("Month end Process Successfully.");
        },
          error => {
            console.error(`Error: ${error.message}`);
          });
      },() => {

      });
    }
  }
  customerexcel() {
    const frmDate = this.customerReportForm.get('frmDate').value;
    const toDate = this.customerReportForm.get('toDate').value;
    const status = this.customerReportForm.get('active').value;
    const id = this.customerReportForm.get('schemeCode').value;

    if (!this.customerReportForm.valid) {
      return false;
    } else {
      this.reportService.getExcelId(status, 1, 5, frmDate, toDate, 'Customer Deduction Unpaid Details',id,'').
        subscribe(response => {
          let file = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
          fileSaver.saveAs(file, 'CustomerDeductionDetails.xlsx');

        },
          error => {
            console.error(`Error: ${error.message}`);
          });
    }
  }
  get f() {
    return this.customerReportForm.controls;
  }
  createCustomerForm() {
    this.customerReportForm = this.fb.group({
      schemeCode: ['', [Validators.required]],
      frmDate: ['', [Validators.required]],
      toDate: ['', [Validators.required]],
      active: [this.active]
    });
  }

}
