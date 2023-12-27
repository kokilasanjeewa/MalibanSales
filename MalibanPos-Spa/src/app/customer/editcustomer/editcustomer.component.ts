import { CommonService } from './../../_services/common.service';
import { AlertifyService } from './../../_services/alertify.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import * as $ from 'jquery';
import { Branch } from 'src/app/models/branch';
import { CustomerService } from 'src/app/_services/customer.service';
import { MI_Employer_Employees } from 'src/app/models/mI_employer_employees';
import { formatDate } from '@angular/common';
@Component({
  selector: 'app-editcustomer',
  templateUrl: './editcustomer.component.html',
  styleUrls: ['./editcustomer.component.css'],
})
export class EditcustomerComponent implements OnInit {
  editCustomerForm: FormGroup;
  branches$: Observable<Branch[]>;
  selectedFile: File = null;
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private alertifyService: AlertifyService,
    private commonService: CommonService,
    private customerService: CustomerService
  ) {
    this.branches$ = this.commonService.getList('Common', 'getallbranch');
  }
  ngOnInit() {
    this.customerService
      .getCustomerById(+this.route.snapshot.params.customerId)
      .subscribe(
        (s: MI_Employer_Employees) => {
          this.editCustomerForm.patchValue({
            number: s.number,
            deleted: s.deleted,
            dateOfAppointment: formatDate(
              s.dateOfAppointment,
              'yyyy-MM-dd',
              'en'
            ),
            schemeCode: s.schemeCode,
            ePFNo: s.epfNo,
            dateDeleted: formatDate(s.dateDeleted, 'yyyy-MM-dd', 'en'),
            deletedReason: s.deletedReason,
            dateResigned: formatDate(s.dateResigned, 'yyyy-MM-dd', 'en'),
            location: s.location,
            fullName: s.fullName,
            creditLimit: s.creditLimit,
            creditBalance: s.creditBalance,
            scheme: s.scheme,
            previousCreditBalance: s.previousCreditBalance,
          });
          console.log(s);
        },
        (error) => {
          this.alertifyService.error(error);
        }
      );
    this.editCustomerRegisterForm();
  }
  get f() {
    return this.editCustomerForm.controls;
  }
  editCustomerRegisterForm() {
    this.editCustomerForm = this.fb.group({
      number: ['', [Validators.required]],
      dateOfAppointment: ['', [Validators.required]],
      schemeCode: ['', [Validators.required]],
      ePFNo: ['', [Validators.required]],
      dateDeleted: ['', [Validators.required]],
      deletedReason: ['',[Validators.maxLength(30)]],
      dateResigned: ['', [Validators.required]],
      location: ['', [Validators.required]],
      fullName: ['', [Validators.required]],
      uploadImage: [''],
      creditLimit: ['', [Validators.required]],
      creditBalance: ['', [Validators.required]],
      scheme: ['', [Validators.required]],
      previousCreditBalance: ['', [Validators.required]],
      deleted: ['', [Validators.required]],
    });
  }
  onFileChange(event) {
    this.selectedFile = <File>event.target.files[0];
    console.log(this.selectedFile);
  }
  supplierEdit() {
    // this.submitted = true;
    const formData: any = new FormData();
    formData.append('number', this.editCustomerForm.get('number').value);
    formData.append('Files', this.selectedFile);
    formData.append(
      'dateOfAppointment',
      this.editCustomerForm.get('dateOfAppointment').value
    );
    formData.append(
      'schemeCode',
      this.editCustomerForm.get('schemeCode').value
    );
    formData.append('epfNo', this.editCustomerForm.get('ePFNo').value);
    formData.append(
      'dateDeleted',
      this.editCustomerForm.get('dateDeleted').value
    );
    formData.append(
      'deletedReason',
      this.editCustomerForm.get('deletedReason').value
    );
    formData.append(
      'dateResigned',
      this.editCustomerForm.get('dateResigned').value
    );
    formData.append('location', this.editCustomerForm.get('location').value);
    formData.append(
      'fullName',
      this.editCustomerForm.get('fullName').value
    );
    formData.append(
      'creditLimit',
      this.editCustomerForm.get('creditLimit').value
    );
    formData.append('creditBalance', this.editCustomerForm.get('creditBalance').value);
    formData.append('scheme', this.editCustomerForm.get('scheme').value);
    formData.append('previousCreditBalance', this.editCustomerForm.get('previousCreditBalance').value);
    formData.append('deleted', this.editCustomerForm.get('deleted').value ? 1:0);
    formData.append(
      'branchLocationId',
      this.commonService.getUserBranchLocationId()
    );
    console.log(formData);
    if (!this.editCustomerForm.valid) {
      return false;
    } else {
      //   alert(JSON.stringify(this.editSupplierForm.value));
      this.customerService
        .updateCustomer(formData, +this.route.snapshot.params.customerId)
        .subscribe(
          (next) => {},
          (error) => {
            this.alertifyService.error(
              JSON.stringify(error).replace('"', '').replace('"', '')
            );
            console.log(error.error);
          },
          () => {
            this.alertifyService.success('Saved Successfully.');
            this.router.navigate(['/customer/customerlist']);
          }
        );
    }
  }
}
