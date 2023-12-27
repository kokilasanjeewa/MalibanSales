import { SupplierPrice } from './../../models/supplierprice';
import { PriceService } from './../../_services/price.service';
import { SupplierProduct } from './../../models/supplierproduct';
import { Product } from './../../models/product';
import { ProductService } from './../../_services/product.service';
import { CommonService } from './../../_services/common.service';
import { Component, OnInit } from '@angular/core';
import { AlertifyService } from './../../_services/alertify.service';
import { SupplierproductService } from 'src/app/_services/supplierproduct.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable, Subject } from 'rxjs';
import { BranchLocation } from 'src/app/models/branchlocation';
import { takeUntil } from 'rxjs/operators';

@Component({
  templateUrl: './addsupplierprice.component.html',
  styleUrls: ['./addsupplierprice.component.css']
})
export class AddsupplierpriceComponent implements OnInit {
  addSupplierPriceForm: FormGroup;
  products: Observable<SupplierProduct[]>;
  supplierprices: SupplierPrice[];
  suppliers: Observable<[]>;
  productCode: {};
  supplierId: number;
  branchLocationId: number;
  selectedItem: number;
  public val: string;
  branches:Observable<[]>;
  // submitted = false;
  salePrice:number=0;
  private destroyed$ = new Subject();
  constructor(
    private fb: FormBuilder,
    private alertifyService: AlertifyService,
    private commonService: CommonService,
    private productService: ProductService,
    private supplierproductService: SupplierproductService,
    private priceService: PriceService
  ) {
    this.branches = this.commonService.getListByLocation(this.commonService.getUserBranchLocationId(), 'Common', 'getbranch');
    //this.products = this.commonService.getList('Common', 'getallproduct');
    this.suppliers = this.commonService.getListByLocation(this.commonService.getUserBranchLocationId(), 'Common', 'getallsupplier');
  }

  ngOnInit() {
    this.createSupplierRegisterForm();
  }
  get f() {
    return this.addSupplierPriceForm.controls;
  }
  priceMatchvalidator(g: FormGroup) {
    return g.get('unitPrice').value <= g.get('maximumRetailPrice').value
      ? null
      : { mismatch: true };
  }
  profitMatchvalidator(g: FormGroup) {
    const profit = g.get('profit').value;
    const substring ="%";
    if(profit.includes(substring)){
      return  (Number(g.get('maximumRetailPrice').value-g.get('unitPrice').value) * Number(profit.replace("%","")))/100 <= Number(g.get('maximumRetailPrice').value-g.get('unitPrice').value)?null: { pmismatch: true };
    }
    return Number(g.get('profit').value) <= Number(g.get('maximumRetailPrice').value-g.get('unitPrice').value)
      ? null
      : { pmismatch: true };
  }
  createSupplierRegisterForm() {
    this.addSupplierPriceForm = this.fb.group({
      supplierId: ['', [Validators.required]],
      productId: ['', [Validators.required]],
      productName: ['', [Validators.required]],
      branchLocationId: [this.commonService.getUserBranchLocationId(), [Validators.required]],
      priceDate: ['', [Validators.required]],
      maximumRetailPrice: ['', [Validators.required]],
      unitPrice: ['', [Validators.required]],
      profit: ['', [Validators.required]],
    },
    { validator: [this.priceMatchvalidator,this.profitMatchvalidator] }
    );
  }
  changeBranch() {
    this.suppliers = this.commonService.getListByLocation(this.addSupplierPriceForm.get('branchLocationId').value, 'Common', 'getallsupplier');
  }
  changeSupplier() {
    this.supplierId = this.addSupplierPriceForm.get('supplierId').value;
    this.branchLocationId = this.addSupplierPriceForm.get('branchLocationId').value;
    this.products =this.supplierproductService.getProductBySupplierId(this.supplierId,this.branchLocationId);
    this.priceService.getProductPriceBySupplierId(this.supplierId, this.branchLocationId)
    .pipe(takeUntil(this.destroyed$))
    .subscribe((data) => {
      console.log("supplier prices");
      console.log(data);
      this.supplierprices = data;
      this.alertifyService.success("Loaded Successfully");
    }, (error) => {
      console.log(error);
      this.supplierprices = null;
      this.alertifyService.error(error);
    }
    );
  }
  changeProduct(val) {
    console.log(this.supplierId);
    console.log(val);
    console.log(this.selectedItem);
   this.productCode = val;
    this.productService.getProductById(this.productCode)
    .pipe(takeUntil(this.destroyed$))
    .subscribe((data) => {
      this.addSupplierPriceForm.patchValue({
        productId: data.productId,
        productName: data.productName,
      });
    });
    this.priceService.getSupplierPriceBySupplierProductId(this.supplierId, this.branchLocationId,+this.productCode)
    .pipe(takeUntil(this.destroyed$))
    .subscribe((data) => {
      console.log("supplier product prices");
      console.log(data);
      this.supplierprices = data;
      this.alertifyService.success("Loaded Successfully");
    }, (error) => {
      console.log(error);
      this.supplierprices = null;
      this.alertifyService.error(error);
    }
    );
  }
  addSupplierPrice() {
    const formData: any = new FormData();
    formData.append('supplierId', this.addSupplierPriceForm.get('supplierId').value);
    formData.append('productId', this.addSupplierPriceForm.get('productId').value);
    formData.append('branchLocationId', this.addSupplierPriceForm.get('branchLocationId').value);
    formData.append('priceDate', this.addSupplierPriceForm.get('priceDate').value);
    formData.append('maximumRetailPrice', this.addSupplierPriceForm.get('maximumRetailPrice').value);
    formData.append('unitPrice', this.addSupplierPriceForm.get('unitPrice').value);
    const profit = this.addSupplierPriceForm.get('profit').value;
    const substring ="%";
    if(profit.includes(substring)){
      formData.append('profit',(Number(profit.replace("%","")) * Number(this.addSupplierPriceForm.get('maximumRetailPrice').value-this.addSupplierPriceForm.get('unitPrice').value))/100);
    }
    else {
      formData.append('profit',profit);
    }
    formData.append('createdBy', this.commonService.getUser());
    console.log('after formdata');
    if (!this.addSupplierPriceForm.valid) {
      this.alertifyService.message('Please verify the input .');
      return false;
    }
    else {
      console.log('before create supplier');
       this.priceService.createSupplierPrice(formData)
       .pipe(takeUntil(this.destroyed$))
        .subscribe((data) => {
          console.log(data);
          this.alertifyService.success("Added successfully.");

        }, (error) => {
          console.log(error.errorMessage);
          this.alertifyService.error(error.errorMessage);
        }, () => {
          this.priceService.getProductPriceBySupplierId(this.addSupplierPriceForm.get('supplierId').value, this.addSupplierPriceForm.get('branchLocationId').value
          )
            .subscribe((data) => {
              console.log(data);
              this.supplierprices = data;
            }
            );
        }
        );

    }
  }
  removePrice(sp: SupplierPrice) {
    console.log(sp.productId);
    console.log(sp);
    const formData: any = new FormData();
    formData.append('supplierId', this.supplierId);
    formData.append('productId', JSON.stringify(sp.productId));
    formData.append('branchLocationId', this.commonService.getUserBranchLocationId());
    this.alertifyService.confirm("Please Confirm.", "Are you sure you want to delete this supplier price?", () => {
      this.priceService.deleteSupplierPrice(formData)
      .pipe(takeUntil(this.destroyed$))
        .subscribe((data) => {
          console.log(data);
          this.alertifyService.success("Deleted successfully.");
          this.addSupplierPriceForm.patchValue({
            productId: sp.productId,
          });
        }, (error) => {
          this.alertifyService.error(error);
        }, () => {
          this.priceService.getProductPriceBySupplierId(this.supplierId, this.commonService.getUserBranchLocationId())
          .pipe(takeUntil(this.destroyed$))
          .subscribe((data) => {
            console.log(data);
            this.supplierprices = data;
          }
          );
        });
    },
      () => {
        this.priceService.getProductPriceBySupplierId(sp.supplierId, this.commonService.getUserBranchLocationId())
        .pipe(takeUntil(this.destroyed$))
        .subscribe((data) => {
          console.log(data);
          this.supplierprices = data;
        });
      });
  }
  onProfitChange(){
    const profit = this.addSupplierPriceForm.get('profit').value;
    const substring ="%";
    if(profit.includes(substring)){
      this.salePrice= Number(this.addSupplierPriceForm.get('unitPrice').value)+(Number(profit.replace("%","")) * Number(this.addSupplierPriceForm.get('maximumRetailPrice').value-this.addSupplierPriceForm.get('unitPrice').value))/100;
    }
    else {
      this.salePrice=Number(this.addSupplierPriceForm.get('unitPrice').value)+Number(this.addSupplierPriceForm.get('profit').value);
    }
  }
}
