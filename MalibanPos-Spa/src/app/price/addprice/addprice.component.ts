import { PriceService } from './../../_services/price.service';
import { CommonService } from 'src/app/_services/common.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { Observable } from 'rxjs';

@Component({
  templateUrl: './addprice.component.html',
  styleUrls: ['./addprice.component.css']
})
export class AddpriceComponent implements OnInit {
  addPriceForm: FormGroup;
  products: Observable<[]>;
  branches: Observable<[]>;
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private alertifyService: AlertifyService,
    private commonService: CommonService,
    private priceService: PriceService
  ) {
        this.branches = this.commonService.getList('Common', 'getallbranch');
        this.products = this.commonService.getList('Common', 'getallproduct');
 }

  ngOnInit() {
    this.createPriceRegisterForm();
  }
  get f() {
    return this.addPriceForm.controls;
  }
  createPriceRegisterForm() {
    this.addPriceForm = this.fb.group({
      unitPrice: ['', [Validators.required]],
      maximumRetailPrice: ['', [Validators.required]],
      discountPrice: [''],
      discountPrecentage: [''],
      promotionPrice: [''],
      promotionPrecentage: [''],
      productId: ['', [Validators.required]],
      branchLocationId: ['', [Validators.required]],
      taxPrecentage: [''],
      active: ['true', [Validators.required]],
    });
  }
  priceAdd() {
    const formData: any = new FormData();
    // formData.append('Files', this.selectedFile, this.selectedFile.name);
    formData.append('unitPrice', this.addPriceForm.get('unitPrice').value);
    formData.append('maximumRetailPrice', this.addPriceForm.get('maximumRetailPrice').value);
    formData.append('discountPrice', this.addPriceForm.get('discountPrice').value);
    formData.append('taxPrecentage', this.addPriceForm.get('taxPrecentage').value);
    formData.append('discountPrecentage', this.addPriceForm.get('discountPrecentage').value);
    formData.append('promotionPrice', this.addPriceForm.get('promotionPrice').value);
    formData.append('promotionPrecentage', this.addPriceForm.get('promotionPrecentage').value);
    formData.append('productId', this.addPriceForm.get('productId').value);
    formData.append('branchLocationId', this.addPriceForm.get('branchLocationId').value);
    formData.append('createdBy', this.commonService.getUser());
    formData.append('active', this.addPriceForm.get('active').value);
    console.log(formData);
    if (!this.addPriceForm.valid) {
      return false;
    } else {
     // alert(JSON.stringify(this.addPriceForm.value));
      this.priceService.createPrice(formData).subscribe(
        (next) => { },
        (error) => {
          this.alertifyService.error(
            JSON.stringify(error).replace('"', '').replace('"', '')
          );
          console.log(error.error);
        },
        () => {
          this.alertifyService.success('Saved Successfully.');
          this.router.navigate(['/price/pricelist']);
        }
      );
    }
  }
}
