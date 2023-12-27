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

interface CustomerCategory {
  schemeCode: number;
  scheme: string;
  }
@Component({
  selector: 'app-customermastersetting',
  templateUrl: './customermastersetting.component.html',
  styleUrls: ['./customermastersetting.component.css']
})
export class CustomermastersettingComponent implements OnInit,AfterViewInit {
  addSmvForm: FormGroup;
  status = true;
  pageNumber = 1;
  pageSize = 5;
  bsModalRef: BsModalRef;
  branches: Observable<[]>;
  customerCategory: CustomerCategory[];
  pagination: Pagination;
  public regexReactiveForms = /(\w)*$/g;
  constructor(private customermasterService: CustomermasterService,private modalService: BsModalService, private route: ActivatedRoute,private fb: FormBuilder, private commonService: CommonService,private toastrService: ToastrService, private router: Router) {
    this.branches = this.commonService.getList('Common', 'getallbranch');
    this.customerCategory = [{"schemeCode":1,"scheme":'Worker'},{"schemeCode":2,"scheme":'Staff'},{"schemeCode":3,"scheme":'Executive'}];
  }
  ngOnInit() {
    this.createSmvForm();
  }
  ngAfterViewInit() {

  }
  get f() {
    return this.addSmvForm.controls;
  }
  createSmvForm() {
    this.addSmvForm = this.fb.group({
      creaditBalancePerDay: ['', [Validators.required, Validators.pattern(this.regexReactiveForms)]],
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
    formData.append('customerMasterSettingDescription', this.addSmvForm.get('customerMasterSettingDescription').value);
    formData.append('customerCategoryCode', this.addSmvForm.get('customerCategory').value);
    formData.append('branchLocationId', this.addSmvForm.get('branchLocationId').value);
    formData.append('billingCycleStartDate', this.addSmvForm.get('billingCycleStartDate').value);
    formData.append('billingCycleEndDate', this.addSmvForm.get('billingCycleEndDate').value);
    formData.append('creditHoldingDays', this.addSmvForm.get('creditHoldingDays').value);
    formData.append('creaditBalancePerDays', this.addSmvForm.get('creaditBalancePerDay').value);
    formData.append('createdBy', this.commonService.getUser());
    formData.append('active', true);
    formData.append('IsDeleted', false);
    console.log(formData);
    if (!this.addSmvForm.valid) {
      return false;
    } else {
      this.customermasterService.Createcustomermastersetting(formData).subscribe(
        (next) => { },
        (error) => {
          this.toastrService.error(
            JSON.stringify(error.errorMessage).replace('"', '').replace('"', '')
          );
          console.log(error.error);
        },
        () => {
          this.toastrService.success('Saved Successfully.');
          this.router.navigate(['/setting/customermaster']);
        }
      );
    }

  }

}
