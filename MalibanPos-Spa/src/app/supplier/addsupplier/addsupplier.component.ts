import { CommonService } from './../../_services/common.service';
import { AlertifyService } from './../../_services/alertify.service';
import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SupplierService } from 'src/app/_services/supplier.service';
import { Observable } from 'rxjs';
import * as $ from 'jquery';
import { map } from 'rxjs/operators';
import { BranchLocation } from 'src/app/models/branchlocation';

@Component({
  templateUrl: './addsupplier.component.html',
  styleUrls: ['./addsupplier.component.css'],
})
export class AddsupplierComponent implements OnInit {
  addSupplierForm: FormGroup;
  cities: Observable<[]>;
  districts: Observable<[]>;
  provinces: Observable<[]>;
  titles: Observable<[]>;
  paymentMethod: Observable<[]>;
  typeofGoods: Observable<[]>;
  mobnumberPattern = '^((\\+94-?)|0)?[0-9]{10}$';
  selectedFile: File = null;
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private alertifyService: AlertifyService,
    private commonService: CommonService,
    private supplierService: SupplierService
  ) {}

  ngOnInit() {
    this.provinces = this.commonService.getList('Common', 'getallprovinces');
    this.titles = this.commonService.getList('Common', 'getalltitle');
    this.paymentMethod = this.commonService.getList(
      'Common',
      'getallpaymentmethod'
    );
    this.typeofGoods = this.commonService.getList(
      'Common',
      'getalltypeofgoods'
    );
    this.createSupplierRegisterForm();
    $('.custom-file-input').on('change', function () {
      const fileName = $(this).val().split('\\').pop();
      $(this).next('.custom-file-label').html(fileName);
    });
    console.log(this.commonService.getUser());
  }
  get f() {
    return this.addSupplierForm.controls;
  }
  createSupplierRegisterForm() {
    this.addSupplierForm = this.fb.group({
      companyname: ['', [Validators.required]],
      titleId: ['', [Validators.required]],
      firstname: ['', [Validators.required]],
      lastname: ['', [Validators.required]],
      email: ['', [Validators.email]],
      phone: [
        '',
        [
          Validators.minLength(10),
          Validators.maxLength(12),
          Validators.pattern(this.mobnumberPattern),
        ],
      ],
      mobile: [
        '',
        [
          Validators.required,
          Validators.minLength(10),
          Validators.maxLength(12),
          Validators.pattern(this.mobnumberPattern),
        ],
      ],
      address1: ['', [Validators.required]],
      fax: [''],
      districtId: ['', [Validators.required]],
      provinceId: ['', [Validators.required]],
      cityId: ['', [Validators.required]],
      typeofgood: [''],
      paymentmethod: ['', [Validators.required]],
      brno: [''],
      branchLocationId: [
        this.commonService.getUserBranchLocationId(),
        [Validators.required],
      ],
      active: ['true', [Validators.required]],
    });
  }
  // Choose city using select dropdown
  changeProvince(event: Event) {
    const code = (event.target as HTMLInputElement).value;
    console.log((event.target as HTMLInputElement).value);
    this.addSupplierForm.controls.provinceId.setValue(
      (event.target as HTMLInputElement).value,
      {
        onlySelf: true,
      }
    );
    this.districts = this.commonService.getListbyCode(
      'Common',
      'getalldistrict',
      code
    );
  }
  // Choose city using select dropdown
  changeDistricts(event: Event) {
    const code = (event.target as HTMLInputElement).value;
    console.log((event.target as HTMLInputElement).value);
    this.addSupplierForm.controls.districtId.setValue(
      (event.target as HTMLInputElement).value,
      {
        onlySelf: true,
      }
    );
    this.cities = this.commonService.getListbyCode(
      'Common',
      'getallcities',
      code
    );
  }
  supplierAdd() {
    // this.submitted = true;
    const formData: any = new FormData();
    formData.append(
      'supplierName',
      this.addSupplierForm.get('companyname').value
    );
    formData.append('Files', this.selectedFile);
    formData.append('cityId', this.addSupplierForm.get('cityId').value);
    formData.append('titleId', this.addSupplierForm.get('titleId').value);
    formData.append('address1', this.addSupplierForm.get('address1').value);
    formData.append('districtId', this.addSupplierForm.get('districtId').value);
    formData.append('provinceId', this.addSupplierForm.get('provinceId').value);
    formData.append('firstname', this.addSupplierForm.get('firstname').value);
    formData.append('lastname', this.addSupplierForm.get('lastname').value);
    formData.append(
      'typeofGoodId',
      this.addSupplierForm.get('typeofgood').value
    );
    formData.append(
      'paymentMethodId',
      this.addSupplierForm.get('paymentmethod').value
    );
    formData.append('phone', this.addSupplierForm.get('phone').value);
    formData.append('mobile', this.addSupplierForm.get('mobile').value);
    formData.append('email', this.addSupplierForm.get('email').value);
    formData.append('fax', this.addSupplierForm.get('fax').value);
    formData.append('brno', this.addSupplierForm.get('brno').value);
    formData.append(
      'branchLocationId',
      this.commonService.getUserBranchLocationId()
    );
    formData.append('createdBy', this.commonService.getUser());
    formData.append('active', this.addSupplierForm.get('active').value);

    console.log(formData);
    if (!this.addSupplierForm.valid) {
      return false;
    } else {
      //  alert(JSON.stringify(this.addSupplierForm.value));
      this.supplierService.createSupplier(formData).subscribe(
        (next) => {},
        (error) => {
          this.alertifyService.error(
            JSON.stringify(error).replace('"', '').replace('"', '')
          );
          console.log(error.error);
        },
        () => {
          this.alertifyService.success('Saved Successfully.');
          this.router.navigate(['/supplier/supplierlist']);
        }
      );
    }
  }
  onFileChange(event) {
    this.selectedFile = <File>event.target.files[0];
    console.log(this.selectedFile);
  }
}
