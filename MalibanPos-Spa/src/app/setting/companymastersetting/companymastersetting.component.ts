import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs/internal/Observable';
import { Pagination } from 'src/app/models/pagination';
import { CommonService } from 'src/app/_services/common.service';
import { CompanymastersettingService } from 'src/app/_services/companymastersetting.service';

@Component({
  selector: 'app-companymastersetting',
  templateUrl: './companymastersetting.component.html',
  styleUrls: ['./companymastersetting.component.css']
})
export class CompanymastersettingComponent implements OnInit,AfterViewInit {
  addCompanyMasterSettingForm: FormGroup;
  status = true;
  pageNumber = 1;
  pageSize = 5;
  bsModalRef: BsModalRef;
  branches: Observable<[]>;
  pagination: Pagination;
 // public regexReactiveForms = /(\w)*$/g;
  constructor(private companymastersettingService: CompanymastersettingService,private modalService: BsModalService, private route: ActivatedRoute,private fb: FormBuilder, private commonService: CommonService,private toastrService: ToastrService, private router: Router) {
    this.branches = this.commonService.getList('Common', 'getallbranch');
  }

  ngOnInit() {
    this.createCompanyMasterSettingForm();
/*     this.styleService.getStyles(this.status, this.pageNumber, this.pageSize).subscribe((data) => {
      console.log(data);
      this.styles = data.result;
      this.pagination = data.pagination;
    }); */
  }
  ngAfterViewInit() {

  }
/*    openModalWithComponent(list: ptStyle[], pagination: Pagination) {
    const initialState = {
      list,
      pagination,
      title: ''
    };
    this.bsModalRef = this.modalService.show(StylelistComponent, { initialState, class: 'gray modal-lg' });
    //this.bsModalRef.content.closeBtnName = 'Close';
    this.bsModalRef.content.updateStyleAdd.subscribe((data) => {
      this.style = data;
      this.addSmvForm.patchValue({ styleId: this.style.idStyle });
      console.log(this.style);
    });
  } */
  get f() {
    return this.addCompanyMasterSettingForm.controls;
  }
  createCompanyMasterSettingForm() {
    this.addCompanyMasterSettingForm = this.fb.group({
      branchLocationId: ['', [Validators.required]],
      salePricePrecentageCompany: ['', [Validators.required]],
      salePricePrecentageEmployee: ['', [Validators.required]],
      staffCreditLimitPrecentage: ['', [Validators.required]],
      workerCreditLimitPrecentage: ['', [Validators.required]],
      managerCreditLimitPrecentage: ['', [Validators.required]],
      vatPrecentage: ['', [Validators.required]],
      nbtPrecentage: ['', [Validators.required]],
    });
  }
  // Choose Section using select dropdown
  //https://stackblitz.com/edit/angular-6ew9rl?file=app%2Fapp.component.html
  smvAdd() {
    // this.submitted = true;
    const formData: any = new FormData();
    formData.append('branchLocationId', this.addCompanyMasterSettingForm.get('branchLocationId').value);
    formData.append('salePricePrecentageCompany', this.addCompanyMasterSettingForm.get('salePricePrecentageCompany').value);
    formData.append('salePricePrecentageEmployee', this.addCompanyMasterSettingForm.get('salePricePrecentageEmployee').value);
    formData.append('staffCreditLimitPrecentage', this.addCompanyMasterSettingForm.get('staffCreditLimitPrecentage').value);
    formData.append('workerCreditLimitPrecentage', this.addCompanyMasterSettingForm.get('workerCreditLimitPrecentage').value);
    formData.append('managerCreditLimitPrecentage', this.addCompanyMasterSettingForm.get('managerCreditLimitPrecentage').value);
    formData.append('nbtPrecentage', this.addCompanyMasterSettingForm.get('nbtPrecentage').value);
    formData.append('vatPrecentage', this.addCompanyMasterSettingForm.get('vatPrecentage').value);
    formData.append('createdBy', this.commonService.getUser());
    formData.append('active', true);
    formData.append('IsDeleted', false);
    console.log(formData);
    if (!this.addCompanyMasterSettingForm.valid) {
      return false;
    } else {
      this.companymastersettingService.Createcompanymastersetting(formData).subscribe(
        (next) => { },
        (error) => {
          this.toastrService.error(
            JSON.stringify(error.errorMessage).replace('"', '').replace('"', '')
          );
         // console.log(error);
        },
        () => {
          this.toastrService.success('Saved Successfully.');
          this.router.navigate(['/setting/companymaster']);
        }
      );
    }

  }
}
