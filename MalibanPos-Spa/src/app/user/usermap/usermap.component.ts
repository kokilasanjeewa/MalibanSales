import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, of } from 'rxjs';
import { Branch } from 'src/app/models/branch';
import { EmployeeDto } from 'src/app/models/employeedto';
import { CommonService } from 'src/app/_services/common.service';
import { CustomerService } from 'src/app/_services/customer.service';

@Component({
  selector: 'app-usermap',
  templateUrl: './usermap.component.html',
  styleUrls: ['./usermap.component.css']
})
export class UsermapComponent implements OnInit {
  customerReportForm: FormGroup;
  selectedFile: File = null;
  branches$: Observable<Branch[]>;
  customers$: Observable<EmployeeDto[]>;

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
