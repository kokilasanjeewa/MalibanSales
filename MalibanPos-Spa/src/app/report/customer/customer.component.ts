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
import { CustomerService } from 'src/app/_services/customer.service';

@Component({
  selector: 'app-customer',
  templateUrl: './customer.component.html',
  styleUrls: ['./customer.component.css']
})
export class CustomerComponent implements OnInit {
  customerReportForm: FormGroup;
  customerDetail: CustomerDetail[];
  schemes: Observable<[]>;
  jwtHelper = new JwtHelperService();
  total : number;
  items$$: Observable<any[]>;
  active:boolean=false;

  constructor(
    private commonService: CommonService,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private authService: AuthService,
    private toastrService: ToastrService,
    private reportService: ReportService,
    private customerService: CustomerService,
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
      this.reportService.getReportsId<CustomerDetail>(status, 1, 5, frmDate, toDate, 'Customer Deduction Details',0,id).subscribe(
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
          this.router.navigate(['/report/customer']);
        }
      );
    }
  }
  returnBlob(res): any {
    return new Blob([JSON.parse(res.headers.get('Content-Length'))], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
  }
  customerexcel() {
    const frmDate = this.customerReportForm.get('frmDate').value;
    const toDate = this.customerReportForm.get('toDate').value;
    const status = this.customerReportForm.get('active').value;
    const id = this.customerReportForm.get('schemeCode').value;

    if (!this.customerReportForm.valid) {
      return false;
    } else {
      this.reportService.getExcelId(status, 1, 5, frmDate, toDate, 'Customer Deduction Details',id,'').
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
      active: [this.active, [Validators.required]]
    });
  }
 /*  customerpayment() {
    const frmDate = this.customerReportForm.get('frmDate').value;
    const toDate = this.customerReportForm.get('toDate').value;
    const status = this.customerReportForm.get('active').value;
    const id = this.customerReportForm.get('schemeCode').value;
    if (!this.customerReportForm.valid) {
      return false;
    } else {

      this.alertifyService.confirm("Please Confirm.", "Are you sure you want to Month end Process?", () => {
        this.reportService.getExcelId(status, 1, 5, frmDate, toDate, 'Customer Deduction Process',id,'').
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
  } */
  ChangeScheme(){
    this.customerService.SearchDate(this.customerReportForm.get('schemeCode').value,this.commonService.getUserBranchLocationId()).subscribe(
    {
      next:(res) => {
        this.items$$ = res;
        console.log(res);
      },
      error: (error) => console.log(error),
      complete: () => {

      }
    }
    )
  }
}
