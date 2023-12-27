import { Title } from './../../models/title';
import { Supplier } from './../../models/supplier';
import { SupplierService } from 'src/app/_services/supplier.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { CommonService } from 'src/app/_services/common.service';
import { Observable, of } from 'rxjs';

@Component({
  templateUrl: './editsupplier.component.html',
  styleUrls: ['./editsupplier.component.css'],
})
export class EditsupplierComponent implements OnInit {
  editSupplierForm: FormGroup;
  cities: Observable<[]>;
  districts: Observable<[]>;
  provinces: Observable<[]>;
  titles: Observable<Title[]>;
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
  ) {
    this.provinces = this.commonService.getList('Common', 'getallprovinces');
    this.titles = this.commonService.getList('Common', 'getalltitle');
    this.paymentMethod = this.commonService.getList('Common', 'getallpaymentmethod');
    this.typeofGoods = this.commonService.getList('Common', 'getalltypeofgoods');

  }
  ngOnInit() {
    this.cities = this.commonService.getList('Common', 'getallcities');
    this.districts = this.commonService.getList('Common', 'getalldistrict');
    this.supplierService
      .getSupplierById(+this.route.snapshot.params.supplierId)
      .subscribe(
        (s: Supplier) => {
          this.editSupplierForm.patchValue({
            companyname: s.supplierName,
            titleId: s.titleId,
            provinceId: s.provinceId,
            districtId: s.districtId,
            cityId: s.cityId,
            brno: s.brno,
            typeofgood: s.typeofGoodId,
            paymentmethod: s.paymentMethodId,
            firstname: s.firstName,
            lastname: s.lastName,
            email: s.email,
            mobile: s.phone,
            address1: s.address1,
            fax: s.fax,
            active: s.active,
          });
          console.log(s);
        },
        (error) => {
          this.alertifyService.error(error);
        }
      );
    this.editSupplierRegisterForm();
  }
  get f() {
    return this.editSupplierForm.controls;
  }
  editSupplierRegisterForm() {
    this.editSupplierForm = this.fb.group({
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
      uploadImage: [''],
      districtId: ['', [Validators.required]],
      provinceId: ['', [Validators.required]],
      cityId: ['', [Validators.required]],
      typeofgood: [''],
      paymentmethod: ['', [Validators.required]],
      brno: [''],
      active: ['', [Validators.required]]
    });
  }
  onFileChange(event) {
    this.selectedFile = <File>event.target.files[0];
    console.log(this.selectedFile);
  }
  supplierEdit() {
    // this.submitted = true;
    const formData: any = new FormData();
    formData.append('supplierName', this.editSupplierForm.get('companyname').value);
    formData.append("Files", this.selectedFile);
    formData.append('cityId', this.editSupplierForm.get('cityId').value);
    formData.append('titleId', this.editSupplierForm.get('titleId').value);
    formData.append('address1', this.editSupplierForm.get('address1').value);
    formData.append('districtId', this.editSupplierForm.get('districtId').value);
    formData.append('provinceId', this.editSupplierForm.get('provinceId').value);
    formData.append('firstname', this.editSupplierForm.get('firstname').value);
    formData.append('lastname', this.editSupplierForm.get('lastname').value);
    formData.append('typeofGoodId', this.editSupplierForm.get('typeofgood').value);
    formData.append('paymentMethodId', this.editSupplierForm.get('paymentmethod').value);
    formData.append('phone', this.editSupplierForm.get('mobile').value);
    formData.append('mobile', this.editSupplierForm.get('mobile').value);
    formData.append('email', this.editSupplierForm.get('email').value);
    formData.append('brno', this.editSupplierForm.get('brno').value);
    formData.append('fax', this.editSupplierForm.get('fax').value);
    formData.append('branchLocationId', this.commonService.getUserBranchLocationId());
    formData.append('createdBy', this.commonService.getUser());
    formData.append('active', this.editSupplierForm.get('active').value);

    console.log(formData);
    if (!this.editSupplierForm.valid) {
      return false;
    } else {
      //   alert(JSON.stringify(this.editSupplierForm.value));
      this.supplierService.updateSupplier(formData, +this.route.snapshot.params.supplierId).subscribe(
        (next) => { },
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
  // Choose city using select dropdown
  changeProvince(event: Event) {
    const code = (event.target as HTMLInputElement).value
    console.log((event.target as HTMLInputElement).value);
    this.editSupplierForm.controls.provinceId.setValue(
      (event.target as HTMLInputElement).value,
      {
        onlySelf: true,
      }
    );
    this.districts = this.commonService.getListbyCode('Common', 'getalldistrict', code);
  }
  // Choose city using select dropdown
  changeDistricts(event: Event) {
    const code = (event.target as HTMLInputElement).value
    console.log((event.target as HTMLInputElement).value);
    this.editSupplierForm.controls.districtId.setValue(
      (event.target as HTMLInputElement).value,
      {
        onlySelf: true,
      }
    );
    this.cities = this.commonService.getListbyCode('Common', 'getallcities', code);
  }

}
