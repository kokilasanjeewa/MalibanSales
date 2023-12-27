import { Data } from './../../models/data';
import { PurchaseService } from './../../_services/purchase.service';
import { ProductService } from './../../_services/product.service';
import { Product } from './../../models/product';
import { CommonService } from './../../_services/common.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AlertifyService } from './../../_services/alertify.service';
import { Observable} from 'rxjs';

@Component({
  templateUrl: './purchase.component.html',
  styleUrls: ['./purchase.component.css']
})
export class PurchaseComponent implements OnInit {
  addRequestForm: FormGroup;
  addItemListForm: FormGroup;
  products: Observable<Product[]>;
  branches: Observable<[]>;
  departments: Observable<[]>;
  sections: Observable<[]>;
  productCode: {};
  lines: {};
  count: number;
  data: Data;
  // submitted = false;
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private alertifyService: AlertifyService,
    private commonService: CommonService,
    private productService: ProductService,
    private purchaseService: PurchaseService
  ) {
    this.products = this.commonService.getList('Common', 'getallproduct');
    // this.branches = this.commonService.getList('Common', 'getallbranch');
    this.branches = this.commonService.getListByLocation(this.commonService.getUserBranchLocationId(), 'Common', 'getbranch');
    this.departments = this.commonService.getList('Common', 'getalldepartment');
    this.sections = this.commonService.getList('Common', 'getallsection');

  }

  ngOnInit() {
    this.createSupplierRegisterForm();
    this.createItemList();
    this.initPurchase()
    console.log(this.commonService.getUser());
  }
  initPurchase() {
    this.purchaseService.GetPurchase().subscribe((data: Data) => {
      this.lines = data.lines;
      this.count = data.count;
      //  this.totalQuantity = data.totalQuantity;
      //  this.subtotal = data.computeTotal;
      //  this.discount = data.computeDiscountValue;
      //  this.total = data.computeGrandTotalValue;
      //  this.count = data.count;
    });

  }
  get f() {
    return this.addRequestForm.controls;
  }
  get g() {
    return this.addItemListForm.controls;
  }
  createSupplierRegisterForm() {
    this.addRequestForm = this.fb.group({
      branchLocationId: ['', [Validators.required]],
      departmentId: ['', [Validators.required]],
      sectionId: ['', [Validators.required]],
      expectedDate: ['', [Validators.required]],
      description: [''],
    });
  }
  createItemList() {
    this.addItemListForm = this.fb.group({
      productId: ['', [Validators.required]],
      quantity: ['', [Validators.required]],
      productName: ['', [Validators.required]],
    });
  }
  // Choose city using select dropdown
  changeProvince(event: Event) {
    console.log((event.target as HTMLInputElement).value);
    this.addRequestForm.controls.provinceId.setValue(
      (event.target as HTMLInputElement).value,
      {
        onlySelf: true,
      }
    );
  }
  requestAdd() {
    const formData: any = new FormData();
    formData.append('branchLocationId', this.addRequestForm.get('branchLocationId').value);
    formData.append('departmentId', this.addRequestForm.get('departmentId').value);
    formData.append('sectionId', this.addRequestForm.get('sectionId').value);
    formData.append('expectedDate', this.addRequestForm.get('expectedDate').value);
    formData.append('description', this.addRequestForm.get('description').value);
    formData.append('createdBy', this.commonService.getUser());
    if (!this.addRequestForm.valid && this.count > 0) {
      this.alertifyService.message('Please check with the request list.');
      return false;
    } else {
      this.purchaseService.CreatePurchaseRequestHeader(formData).subscribe(
        (res) => {
          console.log(res);
        //  this.router.navigate(['/request/requestlist']);
          },
        (error) => {
          this.alertifyService.error(
            JSON.stringify(error.text).replace('"', '').replace('"', '')
          );
        },() => {
          this.alertifyService.success('Saved Successfully.')
          this.router.navigate(['/request/requestlist']);
        }
      );
    }

  }
  itemListAdd() {
    const formData: any = new FormData();
    formData.append('barCode', this.productCode);
    formData.append('quantity', this.addItemListForm.get('quantity').value);
    formData.append('createdBy', this.commonService.getUser());
    console.log(formData);
    if (!this.addItemListForm.valid) {
      return false;
    } else {
      // alert(JSON.stringify(this.addItemListForm.value));
      this.purchaseService.CreatepurchaseRequestitemdetails(formData).subscribe(
        (data: Data) => {
          this.lines = data.lines;
          this.count = data.count;
        }
      );
    }
  }
  onFileChange(event) {
    // this.selectedFile = <File>event.target.files[0];
    //  console.log(this.selectedFile);
  }
  add(e) {
    console.log(e.target.value);
    this.productCode = e.target.value
    this.productService.getProduct(e.target.value).
      subscribe((data) => {
        this.addItemListForm.patchValue({
          productName: data.productName,
        });
      })
  }
  remove(id: any) {
    const formData: any = new FormData();
    formData.append('barCode', id);
    this.alertifyService.confirm("Please Confirm.", "Are you sure you want to delete this line?", () => {
      this.purchaseService.RemovePurchaseRequestItemDetails(formData).subscribe();
      this.initPurchase();
      this.alertifyService.success("Deleted successfully.");
    }, () => {
      this.initPurchase();
     });
  }
}
