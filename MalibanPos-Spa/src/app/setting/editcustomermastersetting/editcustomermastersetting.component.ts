import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { Pagination } from 'src/app/models/pagination';
import { CommonService } from 'src/app/_services/common.service';
import * as $ from 'jquery';
import { CustomermasterService } from 'src/app/_services/customermaster.service';
import { CustomerMasterSetting } from 'src/app/models/customermastersetting';

interface CustomerCategory {
  schemeCode: number;
  scheme: string;
  }
@Component({
  selector: 'app-editcustomermastersetting',
  templateUrl: './editcustomermastersetting.component.html',
  styleUrls: ['./editcustomermastersetting.component.css']
})
export class EditcustomermastersettingComponent implements OnInit,AfterViewInit {
  editCustomerMasterSettingForm: FormGroup;
  status = true;
  pageNumber = 1;
  pageSize = 5;
  bsModalRef: BsModalRef;
  branches: Observable<[]>;
  customerCategory: CustomerCategory[];
  pagination: Pagination;
  customerMasterSettingId: number;
  constructor(private customermasterService: CustomermasterService,private modalService: BsModalService, private route: ActivatedRoute,private fb: FormBuilder, private commonService: CommonService,private toastrService: ToastrService, private router: Router) {
    this.branches = this.commonService.getList('Common', 'getallbranch');
    this.customerCategory = [{"schemeCode":1,"scheme":'Worker'},{"schemeCode":2,"scheme":'Staff'},{"schemeCode":3,"scheme":'Executive'}];
  }
  ngOnInit() {
    this.createSmvForm();
    this.customermasterService
    .getCustomermastersetting(+this.route.snapshot.params.calculationMasterId)
    .subscribe((c: CustomerMasterSetting) => {
      this.customerMasterSettingId = c.customerMasterSettingId;
      this.editCustomerMasterSettingForm.controls['branchLocationId'].setValue(c.branchLocationId);
      this.editCustomerMasterSettingForm.controls['creaditBalancePerDay'].setValue(c.creaditBalancePerDays);
      this.editCustomerMasterSettingForm.controls['customerCategory'].setValue(c.customerCategoryCode);
      this.editCustomerMasterSettingForm.controls['billingCycleStartDate'].setValue(c.billingCycleStartDate);
      this.editCustomerMasterSettingForm.controls['billingCycleEndDate'].setValue(c.billingCycleEndDate);
      this.editCustomerMasterSettingForm.controls['creditHoldingDays'].setValue(c.creditHoldingDays);
      this.editCustomerMasterSettingForm.controls['customerMasterSettingDescription'].setValue(c.customerMasterSettingDescription);
      }, (error) => {
      this.toastrService.error(error);
    }
    );
  }
  ngAfterViewInit() {

  }
  get f() {
    return this.editCustomerMasterSettingForm.controls;
  }
  createSmvForm() {
    this.editCustomerMasterSettingForm = this.fb.group({
      creaditBalancePerDay: ['', [Validators.required]],
      branchLocationId: ['', [Validators.required]],
      customerCategory: ['', [Validators.required]],
      billingCycleStartDate: ['', [Validators.required]],
      billingCycleEndDate: ['', [Validators.required]],
      creditHoldingDays: ['', [Validators.required]],
      customerMasterSettingDescription:['', [Validators.required]],
    });
  }
  // Choose Section using select dropdown
  //https://stackblitz.com/edit/angular-6ew9rl?file=app%2Fapp.component.html
  cmsAdd() {
    // this.submitted = true;
    const formData: any = new FormData();
    formData.append('customerMasterSettingDescription', this.editCustomerMasterSettingForm.get('customerMasterSettingDescription').value);
  //  formData.append('customerCategory', this.editCustomerMasterSettingForm.get('customerCategory').value);
    formData.append('CustomerCategoryCode', this.editCustomerMasterSettingForm.get('customerCategory').value);
    formData.append('branchLocationId', this.editCustomerMasterSettingForm.get('branchLocationId').value);
    formData.append('billingCycleStartDate', this.editCustomerMasterSettingForm.get('billingCycleStartDate').value);
    formData.append('billingCycleEndDate', this.editCustomerMasterSettingForm.get('billingCycleEndDate').value);
    formData.append('creditHoldingDays', this.editCustomerMasterSettingForm.get('creditHoldingDays').value);
    formData.append('creaditBalancePerDays', this.editCustomerMasterSettingForm.get('creaditBalancePerDay').value);
    formData.append('createdBy', this.commonService.getUser());
    formData.append('active', true);
    formData.append('IsDeleted', false);
    console.log(formData);
    if (!this.editCustomerMasterSettingForm.valid) {
      return false;
    } else {
      this.customermasterService.updateCustomermastersetting(formData,this.customerMasterSettingId).subscribe(
        (next) => { },
        (error) => {
          this.toastrService.error(
            JSON.stringify(error)
          );
          console.log(error);
        },
        () => {
          this.toastrService.success('Saved Successfully.');
          this.router.navigate(['/setting/customermasterlist']);
        }
      );
    }

  }

}
