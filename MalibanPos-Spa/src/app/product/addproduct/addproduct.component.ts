import { Product } from 'src/app/models/product';
import { ProductService } from './../../_services/product.service';
import { CommonService } from './../../_services/common.service';
import { Component, OnInit } from '@angular/core';
import {  AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { Observable, of } from 'rxjs';
import { Vendor } from 'src/app/models/vendor';
import * as $ from 'jquery';
import { SubcategoryService } from 'src/app/_services/subcategory.service';

@Component({
  selector: 'app-addproduct',
  templateUrl: './addproduct.component.html',
  styleUrls: ['./addproduct.component.css'],
})
export class AddproductComponent implements OnInit {
  addProductForm: FormGroup;
  subcategories: Observable<[]>;
  brands: Observable<[]>;
  uoms: Observable<[]>;
  vendors: Observable<Vendor[]>;
  searchProduct: Product;
  categories: Observable<[]>;
  selectedFile: File = null;
  mismatch: boolean;
  public loading = false;
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private alertifyService: AlertifyService,
    private commonService: CommonService,
    private productService: ProductService,
    private subcategoryService:SubcategoryService,
  ) {
    this.categories = this.commonService.getList('Common', 'getallcategory');
   // this.subcategories = this.commonService.getList('Common', 'getallsubcategory');
    this.brands = this.commonService.getList('Common', 'getallbrand');
    this.uoms = this.commonService.getList('Common', 'getalluOm');
    this.vendors = this.commonService.getList('Common', 'getallvendor');
  }
  ngOnInit() {
    this.createProductRegisterForm();
    $('.custom-file-input').on('change', function () {
      const fileName = $(this).val().split('\\').pop();
      $(this).next('.custom-file-label').html(fileName);
    });
  }
  get f() {
    return this.addProductForm.controls;
  }

  createProductRegisterForm() {
    this.addProductForm = this.fb.group({
      expireDate: [''],
      sKU: [''],
      barcode: ['', [Validators.required]],
      productName: ['', [Validators.required,Validators.maxLength(50)]],
      productDescription: [''],
      categoryId: [''],
      subCategoryId: [''],
      vendorId: ['', [Validators.required]],
      size: [''],
      color: [''],
      unitWeight: [''],
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
  changeCategory(event){
    let categoryId =event.target.value;
   // console.log(categoryId);
    this.subcategories=this.subcategoryService.GetSubcategories(categoryId);
  }
  onKeydown(event) {
    if (event.key === "Enter") {
      this.productService.getProduct(event.target.value).subscribe((data) => {
        this.searchProduct = data;
        if(this.searchProduct.barcode)
        {
          this.mismatch =true;
        }
        else {
          this.mismatch =false;
        }
      },(error) => {
        this.mismatch =false;
      });
    }
}
/*   barcodeMatchvalidator(): ValidatorFn {
    return (control:AbstractControl) : ValidationErrors | null => {
      const value = control.value;
        if (value===this.searchProduct?.barcode) {
          return {mismatch : true};
      }
    }
  } */
  productAdd() {
    const formData: any = new FormData();
    // formData.append('Files', this.selectedFile, this.selectedFile.name);
    formData.append('Files', this.selectedFile);
    formData.append('barcode', String(this.addProductForm.get('barcode').value).trim());
    formData.append('expireDate', this.addProductForm.get('expireDate').value);
    formData.append('sKU', this.addProductForm.get('sKU').value);
    formData.append('productName', this.addProductForm.get('productName').value);
    formData.append('productDescription', this.addProductForm.get('productDescription').value);
    formData.append('categoryId', this.addProductForm.get('categoryId').value);
    formData.append('subCategoryId', this.addProductForm.get('subCategoryId').value);
    formData.append('vendorId', this.addProductForm.get('vendorId').value);
    formData.append('size', this.addProductForm.get('size').value);
    formData.append('color', this.addProductForm.get('color').value);
    formData.append('unitWeight', this.addProductForm.get('unitWeight').value);
    formData.append('note', this.addProductForm.get('note').value);
    formData.append('subCategoryId', this.addProductForm.get('subCategoryId').value);
    formData.append('unitOfMeasurementId', this.addProductForm.get('unitOfMeasurementId').value);
    formData.append('brandId', this.addProductForm.get('brandId').value);
    formData.append('length', this.addProductForm.get('length').value);
    formData.append('width', this.addProductForm.get('width').value);
    formData.append('height', this.addProductForm.get('height').value);
    formData.append('createdBy', this.commonService.getUser());
    formData.append('active', this.addProductForm.get('active').value);
    console.log(formData);
    if (!this.addProductForm.valid) {
      return false;
    } else {
      // alert(JSON.stringify(this.addProductForm.value));
      this.productService.createProduct(formData).subscribe(
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
