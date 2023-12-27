import { SupplierService } from './../../_services/supplier.service';
import { SupplierProduct } from './../../models/supplierproduct';
import { ProductService } from './../../_services/product.service';
import { CommonService } from './../../_services/common.service';
import { Product } from './../../models/product';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AfterViewInit, Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AlertifyService } from './../../_services/alertify.service';
import { SupplierproductService } from 'src/app/_services/supplierproduct.service';
import { BranchLocation } from 'src/app/models/branchlocation';

@Component({
  templateUrl: './add.component.html',
  styleUrls: ['./add.component.css'],

})
export class AddComponent implements OnInit,AfterViewInit {
  addRequestForm1: FormGroup;
  products: Observable<Product[]>;
  supplierproducts: SupplierProduct[];
  suppliers: Observable<[]>;
  productCode: {};
  supplierId: number;
  productId: number;
  branchLocationId: number;
  branches: Observable<[]>;
 public searchKey:string = '';
  // submitted = false;
  constructor(
    private fb: FormBuilder,
    private alertifyService: AlertifyService,
    private commonService: CommonService,
    private productService: ProductService,
    private supplierproductService: SupplierproductService,
    private supplierService: SupplierService,
  ) {
    this.products = this.commonService.getList('Common', 'getallproduct');
    this.suppliers = this.commonService.getListByLocation(this.commonService.getUserBranchLocationId(), 'Common', 'getallsupplier');
    this.branches = this.commonService.getListByLocation(this.commonService.getUserBranchLocationId(), 'Common', 'getbranch');


  }
  ngAfterViewInit(){
    this.branches = this.commonService.getListByLocation(this.commonService.getUserBranchLocationId(), 'Common', 'getbranch');
   // this.commonService.data$.subscribe(data => {console.log(data)});

  }
  ngOnInit() {
    this.createSupplierRegisterForm();
  }
   get f() {
    return this.addRequestForm1.controls;
  }
  createSupplierRegisterForm() {
    this.addRequestForm1 = this.fb.group({
      branchLocationId: [this.commonService.getUserBranchLocationId(), [Validators.required]],
      supplierId: ['', [Validators.required]],
      productId: ['', [Validators.required]],
      // productName: ['', [Validators.required]],
      //barCode:['', [Validators.required]],
    });
  }
  changeSupplier() {
    this.branches = this.commonService.getListByLocation(this.commonService.getUserBranchLocationId(), 'Common', 'getbranch');
   // this.suppliers = this.commonService.getListByLocation(this.addRequestForm1.get('branchLocationId').value, 'Common', 'getallsupplier');
  }
  addSupplier() {
    this.supplierId = this.addRequestForm1.get('supplierId').value;
    this.branchLocationId = this.addRequestForm1.get('branchLocationId').value;
    console.log(this.supplierId);
    this.supplierproductService.getProductBySupplierId(this.supplierId, this.branchLocationId).subscribe((data) => {
      console.log(data);
      this.supplierproducts = data;
      this.alertifyService.success("Loaded Successfully");
    }, (error) => {
      console.log(error);
      this.supplierproducts = null;
      this.alertifyService.error(error);
    }
    );
    // filter supplier product by supplier
  //  this.products=this.supplierproductService.getProductFilterBySupplierId(this.supplierId, this.branchLocationId);
  }
  addProduct(e) {
    console.log(this.supplierId);
    console.log(e.target.value);
    this.productCode = e.target.value;
    //this.searchKey =e.target.value;
    this.productService.getProduct(this.productCode).subscribe((data) => {
      this.addRequestForm1.patchValue({
        productId : data.productId,
        barCode: data.barcode,
        // productName: data.productName,
      });
    },(error) => {
      this.alertifyService.error(error);
    },() => {
      this.alertifyService.success("product found .");
    });
  }
  itemListAdd1() {
        // Process submit data here
    console.log('before formdata');
    const formData: any = new FormData();
    formData.append('supplierId', this.addRequestForm1.get('supplierId').value);
    formData.append('productId', this.addRequestForm1.get('productId').value);
    formData.append('branchLocationId', this.addRequestForm1.get('branchLocationId').value);
    //alert(JSON.stringify(this.addRequestForm1.value));
    console.log('after formdata');
    if (!this.addRequestForm1.valid) {
      this.alertifyService.message('Please check with the order list.');
      return false;
    }
    else {
      console.log('before create supplier');
      this.supplierproductService.createSupplierProduct(formData)
        .subscribe((data) => {
          console.log(data);
          this.alertifyService.success("Added successfully.");

        }, (error) => {
          console.log(error.errorMessage);
          this.alertifyService.error(error.errorMessage);
        }, () => {
          //this.addRequestForm1.reset();
                this.supplierproductService.getProductBySupplierId(this.addRequestForm1.get('supplierId').value, this.addRequestForm1.get('branchLocationId').value).subscribe((data) => {
            console.log(data);
            this.supplierproducts = data;
          });
              }
        );

    }
  }
  clear(){
    this.supplierproducts=null;
  }
  removeProduct(sp: SupplierProduct) {
    const formData: any = new FormData();
    formData.append('supplierId', sp.supplier.supplierId);
    formData.append('productId', sp.product.productId);
    formData.append('branchLocationId', sp.branchLocation.branchLocationId);
    this.alertifyService.confirm("Please Confirm.", "Are you sure you want to delete this line?", () => {
      this.supplierproductService.deleteSupplierProduct(formData)
        .subscribe((data) => {
          console.log(data);
          this.alertifyService.success("Deleted successfully.");
        }, (error) => {
          console.log(error);
          this.alertifyService.error(error);
        }, () => {
          this.supplierproductService.getProductBySupplierId(sp.supplier.supplierId, sp.branchLocation.branchLocationId).subscribe((data) => {
            console.log(data);
            this.supplierproducts = data;
          }
          );
        });
    },
      () => {
        this.supplierproductService.getProductBySupplierId(sp.supplier.supplierId, sp.branchLocation.branchLocationId).subscribe((data) => {
          console.log(data);
          this.supplierproducts = data;
        });
      });
  }
}
