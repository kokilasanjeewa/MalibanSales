import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs/internal/Observable';
import { CalculationMaster } from 'src/app/models/calculationmaster';
import { Pagination } from 'src/app/models/pagination';
import { CommonService } from 'src/app/_services/common.service';
import { CompanymastersettingService } from 'src/app/_services/companymastersetting.service';

@Component({
  selector: 'app-editcompanymastersetting',
  templateUrl: './editcompanymastersetting.component.html',
  styleUrls: ['./editcompanymastersetting.component.css']
})
export class EditcompanymastersettingComponent implements OnInit,AfterViewInit {
  editCompanyMasterSettingForm: FormGroup;
  status = true;
  pageNumber = 1;
  pageSize = 5;
  bsModalRef: BsModalRef;
  branches: Observable<[]>;
  pagination: Pagination;
  calculationMasterId: number;
  constructor(private companymastersettingService: CompanymastersettingService,private modalService: BsModalService, private route: ActivatedRoute,private fb: FormBuilder, private commonService: CommonService,private toastrService: ToastrService, private router: Router) {
    this.branches = this.commonService.getList('Common', 'getallbranch');
  }

  ngOnInit() {
    this.createCompanyMasterSettingForm();
    this.companymastersettingService
    .getCompanyMaster(+this.route.snapshot.params.calculationMasterId)
    .subscribe((c: CalculationMaster) => {
      this.calculationMasterId = c.calculationMasterId;
      this.editCompanyMasterSettingForm.controls['branchLocationId'].setValue(c.branchLocationId);
      this.editCompanyMasterSettingForm.controls['salePricePrecentageCompany'].setValue(c.salePricePrecentageCompany);
      this.editCompanyMasterSettingForm.controls['salePricePrecentageEmployee'].setValue(c.salePricePrecentageEmployee);
      this.editCompanyMasterSettingForm.controls['staffCreditLimitPrecentage'].setValue(c.staffCreditLimitPrecentage);
      this.editCompanyMasterSettingForm.controls['workerCreditLimitPrecentage'].setValue(c.workerCreditLimitPrecentage);
      this.editCompanyMasterSettingForm.controls['managerCreditLimitPrecentage'].setValue(c.managerCreditLimitPrecentage);
      this.editCompanyMasterSettingForm.controls['vatPrecentage'].setValue(c.vatPrecentage);
      this.editCompanyMasterSettingForm.controls['nbtPrecentage'].setValue(c.nbtPrecentage);
      }, (error) => {
      this.toastrService.error(error);
    }
    );
  }
  ngAfterViewInit() {

  }
  get f() {
    return this.editCompanyMasterSettingForm.controls;
  }
  createCompanyMasterSettingForm() {
    this.editCompanyMasterSettingForm = this.fb.group({
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
  smvEdit() {
    // this.submitted = true;
    const formData: any = new FormData();
    formData.append('branchLocationId', this.editCompanyMasterSettingForm.get('branchLocationId').value);
    formData.append('salePricePrecentageCompany', this.editCompanyMasterSettingForm.get('salePricePrecentageCompany').value);
    formData.append('salePricePrecentageEmployee', this.editCompanyMasterSettingForm.get('salePricePrecentageEmployee').value);
    formData.append('staffCreditLimitPrecentage', this.editCompanyMasterSettingForm.get('staffCreditLimitPrecentage').value);
    formData.append('workerCreditLimitPrecentage', this.editCompanyMasterSettingForm.get('workerCreditLimitPrecentage').value);
    formData.append('managerCreditLimitPrecentage', this.editCompanyMasterSettingForm.get('managerCreditLimitPrecentage').value);
    formData.append('nbtPrecentage', this.editCompanyMasterSettingForm.get('nbtPrecentage').value);
    formData.append('vatPrecentage', this.editCompanyMasterSettingForm.get('vatPrecentage').value);
    formData.append('createdBy', this.commonService.getUser());
    formData.append('active', true);
    formData.append('IsDeleted', false);
    console.log(formData);
    if (!this.editCompanyMasterSettingForm.valid) {
      return false;
    } else {
      this.companymastersettingService.updateCompanyMaster(formData, this.calculationMasterId).subscribe(
        (next) => { },
        (error) => {
          this.toastrService.error(
            JSON.stringify(error)
          );
         // console.log(error);
        },
        () => {
          this.toastrService.success('Saved Successfully.');
          this.router.navigate(['/setting/companymasterlist']);
        }
      );
    }

  }
}
