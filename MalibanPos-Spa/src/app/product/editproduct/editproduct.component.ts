import { Product } from './../../models/product';
import { ProductService } from './../../_services/product.service';
import { CommonService } from './../../_services/common.service';
import { AlertifyService } from './../../_services/alertify.service';
import { Vendor } from './../../models/vendor';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import * as $ from 'jquery';
import { formatDate } from '@angular/common';

@Component({
  templateUrl: './editproduct.component.html',
  styleUrls: ['./editproduct.component.css']
})
export class EditproductComponent implements OnInit {
  editProductForm: FormGroup;
  subcategories: Observable<[]>;
  brands: Observable<[]>;
  uoms: Observable<[]>;
  vendors: Observable<Vendor[]>;
  categories: Observable<[]>;
  selectedFile: File = null;
  public loading = false;
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private alertifyService: AlertifyService,
    private commonService: CommonService,
    private productService: ProductService
  ) {
    this.categories = this.commonService.getList('Common', 'getallcategory');
    this.subcategories = this.commonService.getList('Common', 'getallsubcategory');
    this.brands = this.commonService.getList('Common', 'getallbrand');
    this.uoms = this.commonService.getList('Common', 'getalluOm');
    this.vendors = this.commonService.getList('Common', 'getallvendor');
  }
  ngOnInit() {
    $('.custom-file-input').on('change', function () {
      const fileName = $(this).val().split('\\').pop();
      $(this).next('.custom-file-label').html(fileName);
    });
    this.productService
      .getProductById(+this.route.snapshot.params.productId)
      .subscribe((p: Product) => {
        this.editProductForm.patchValue({
          expireDate: formatDate(p.expiryDate, 'yyyy-MM-dd', 'en'),
          sKU: p.sKU,
          barcode: p.barcode,
          productName: p.productName,
          productDescription: p.productDescription,
          categoryId: p.categoryId,
          subCategoryId: p.subCategoryId,
          vendorId: p.vendor?.vendorId,
          size: p.size,
          color: p.color,
          unitWeight: p.unitWeight,
          note: p.note,
          unitOfMeasurementId: p.unitOfMeasurement?.unitOfMeasurementId,
          brandId: p.brand?.brandId,
          length: p.length,
          width: p.width,
          height: p.height,
          active: p.active,

        });
      }, (error) => {
        this.alertifyService.error(error);
      }
      );
    this.editProductRegisterForm();
  }
  get f() {
    return this.editProductForm.controls;
  }
  editProductRegisterForm() {
    this.editProductForm = this.fb.group({
      expireDate: ['', [Validators.required]],
      sKU: [''],
      barcode: [''],
      productName: ['', [Validators.required]],
      productDescription: [''],
      categoryId: [''],
      subCategoryId: [''],
      vendorId: ['', [Validators.required]],
      size: [''],
      color: [''],
      unitWeight: [0],
      note: [''],
      unitOfMeasurementId: ['', [Validators.required]],
      brandId: ['', [Validators.required]],
      length: [''],
      width: [''],
      height: [''],
      active: ['true', [Validators.required]],
    });
  }
  onFileChange(event) {
    this.selectedFile = <File>event.target.files[0];
    console.log(this.selectedFile);
  }
  productEdit() {
    const formData: any = new FormData();
    // formData.append('Files', this.selectedFile, this.selectedFile.name);
    formData.append('Files', this.selectedFile);
    formData.append('barcode', String(this.editProductForm.get('barcode').value).trim());
    formData.append('expireDate', this.editProductForm.get('expireDate').value);
    formData.append('sKU', this.editProductForm.get('sKU').value);
    formData.append('productName', this.editProductForm.get('productName').value);
    formData.append('productDescription', this.editProductForm.get('productDescription').value);
    formData.append('categoryId', this.editProductForm.get('categoryId').value);
    formData.append('subCategoryId', this.editProductForm.get('subCategoryId').value);
    formData.append('vendorId', this.editProductForm.get('vendorId').value);
    formData.append('size', this.editProductForm.get('size').value);
    formData.append('color', this.editProductForm.get('color').value);
    formData.append('unitWeight', this.editProductForm.get('unitWeight').value);
    formData.append('note', this.editProductForm.get('note').value);
    formData.append('subCategoryId', this.editProductForm.get('subCategoryId').value);
    formData.append('unitOfMeasurementId', this.editProductForm.get('unitOfMeasurementId').value);
    formData.append('brandId', this.editProductForm.get('brandId').value);
    formData.append('length', this.editProductForm.get('length').value);
    formData.append('width', this.editProductForm.get('width').value);
    formData.append('height', this.editProductForm.get('height').value);
    formData.append('modifiedBy', this.commonService.getUser());
    formData.append('active', this.editProductForm.get('active').value);
    console.log(formData);
    if (!this.editProductForm.valid) {
      return false;
    } else {
    //  alert(JSON.stringify(this.editProductForm.value));
    
      this.productService.updateProduct(formData, +this.route.snapshot.params.productId).subscribe(
        (next) => { },
        (error) => {
          this.alertifyService.error(
            JSON.stringify(error).replace('"', '').replace('"', '')
          );
          console.log(error.error);
        },
        () => {
          this.alertifyService.success('Saved Successfully.');
          this.router.navigate(['/product/productlist']);
        }
      );
    }
  }
}
