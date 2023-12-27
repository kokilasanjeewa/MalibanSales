import { Component, OnInit } from '@angular/core';
import { PaginatedResult } from './../../models/pagination';
import { CommonService } from './../../_services/common.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CustomerDetail } from '../../models/customerdetail';
import * as $ from 'jquery';
import { Observable, Subject, of } from 'rxjs';
import { AlertifyService } from './../../_services/alertify.service';
import { CustomerService } from 'src/app/_services/customer.service';
import { Branch } from 'src/app/models/branch';
import { EmployeeDto } from 'src/app/models/employeedto';
import { takeUntil } from 'rxjs/operators';
@Component({
  selector: 'app-mapcustomer',
  templateUrl: './mapcustomer.component.html',
  styleUrls: ['./mapcustomer.component.css']
})
export class MapcustomerComponent implements OnInit {
  customerReportForm: FormGroup;
  selectedFile: File = null;
  branches$: Observable<Branch[]>;
  customers$: Observable<EmployeeDto[]>;
  private destroyed$ = new Subject();

  constructor(
    private commonService: CommonService,
    private fb: FormBuilder,
    private router: Router,
    private toastrService: ToastrService,
    private customerService: CustomerService,
  ) {
    of(
      this.branches$ = this.commonService
        .getList('Common', 'getallbranch')
    );
    of(
      this.customers$ = this.customerService.GetMapCustomer(this.commonService.getUserBranchLocationId())
    );
   }
   Customeradd(event: Event) {
    const Value = (event.target as HTMLInputElement).value;
    const empCode = Value.trim().toLowerCase();
    if (empCode.length>0)
    this.customerService.SearchCustomer(
      empCode,
      Number(this.commonService.getUserBranchLocationId())
    ).pipe(
      takeUntil(this.destroyed$)
    ).subscribe(
      (data) => {
        console.log(data);
      },
      (error) => {
        // this.alertifyService.error(error);
        this.toastrService.error(error);
      },
      () => {
        // this.alertifyService.success('Customer add successfully.');
       // this.myInputField1.nativeElement.focus();
        this.toastrService.success('Customer add successfully.');
      }
    );
  }
  ngOnInit() {
    this.createCustomerForm();

     }

   get f() {
    return this.customerReportForm.controls;
  }
  createCustomerForm() {
    this.customerReportForm = this.fb.group({
      searchCustomer: ['', [Validators.required]],
      branchCode:  ['', [Validators.required]],
    });
  }
  mapCustomerAdd() {
    const formData: any = new FormData();
    formData.append('tomapBranchCodeId', this.customerReportForm.get('branchCode').value);
    formData.append('epfNo', this.customerReportForm.get('searchCustomer').value);
    formData.append('branchCodeId',  Number(this.commonService.getUserBranchLocationId()));
    if (!this.customerReportForm.valid) {
      return false;
    } else {
      this.customerService.PostMapEmployeeByempCode(formData).subscribe(
        (res) => {
          console.log(res);
        },
        (error) => {
          this.toastrService.error(
            JSON.stringify(error).replace('"', '').replace('"', '')
          );
          console.log(error.error);
        },
        () => {
          this.toastrService.success('Data Loading Successfully.');
          this.router.navigate(['/customer/mapcustomer']);
        }
      );
    }
  }



}
