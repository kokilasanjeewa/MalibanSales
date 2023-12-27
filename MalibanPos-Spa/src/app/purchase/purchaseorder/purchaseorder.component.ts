import { Data } from "./../../models/data";
import { takeUntil } from "rxjs/operators";
import { PriceService } from "./../../_services/price.service";
import { SupplierPrice } from "./../../models/supplierprice";
import { PurchaseorderService } from "./../../_services/purchaseorder.service";
import { ProductService } from "./../../_services/product.service";
import { CommonService } from "src/app/_services/common.service";
import { formatDate } from "@angular/common";
import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { AlertifyService } from "src/app/_services/alertify.service";
import { ActivatedRoute, Router } from "@angular/router";
import { Observable, Subject } from "rxjs";
import { BranchLocation } from "src/app/models/branchlocation";
import { error } from "@angular/compiler/src/util";
interface Post {
  startDate: Date;
  endDate: Date;
}
@Component({
  templateUrl: "./purchaseorder.component.html",
  styleUrls: [
    "./purchaseorder.component.css",
    "./purchaseorder.component.scss",
  ],
})
export class PurchaseorderComponent implements OnInit {
  selectedItem: number;
  selectedProductcode: string;
  post: Post = {
    startDate: new Date(Date.now()),
    endDate: new Date(Date.now()),
  };
  addPOForm: FormGroup;
  addorderListForm: FormGroup;
  products: Observable<SupplierPrice[]>;
  supplierPrices: Observable<SupplierPrice[]>;
  branches: Observable<BranchLocation[]>;
  payments: Observable<[]>;
  suppliers: Observable<[]>;
  indents: Observable<[]>;
  productId: any;
  lines: {};
  requestlist: Observable<[]>;
  subtotal: number;
  discount: number;
  total: number;
  show: boolean = true;
  orderHeader: number;
  // submitted = false;
  private destroyed$ = new Subject();
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private alertifyService: AlertifyService,
    private commonService: CommonService,
    private productService: ProductService,
    private purchaseorderService: PurchaseorderService,
    private priceService: PriceService
  ) {
    // this.products = this.commonService.getList('Common', 'getallproduct');
    this.branches = this.commonService.getListByLocation(
      this.commonService.getUserBranchLocationId(),
      "Common",
      "getbranch"
    );
    this.suppliers = this.commonService.getListByLocation(
      this.commonService.getUserBranchLocationId(),
      "Common",
      "getallsupplier"
    );
    this.payments = this.commonService.getList("Common", "getallpaymentmethod");
    this.requestlist = this.commonService.getListByLocation(
      this.commonService.getUserBranchLocationId(),
      "Common",
      "getallpurchaserequest"
    );
  }
  ngOnInit() {
    this.createSupplierRegisterForm();
    this.initPurchaseOrder();
    console.log(this.commonService.getUser());
  }
  initPurchaseOrder() {
    this.purchaseorderService
      .GetPurchaseOrder()
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: (data: Data) => {
          this.lines = data.lines;
          console.log(data);
          //  this.count = data.count;
          //  this.totalQuantity = data.totalQuantity;
          this.subtotal = data.computeTotalValue;
          this.discount = data.computeDiscountValue;
          this.total = data.computeGrandTotalValue;
          //  this.count = data.count;
        },
        error: (error) => {},
        complete: () => {},
      });
  }
  get f() {
    return this.addPOForm.controls;
  }
  get f1() {
    return this.addorderListForm.controls;
  }
  createSupplierRegisterForm() {
    const now = new Date();
    console.log(now.toISOString());
    this.addPOForm = this.fb.group({
      branchLocationId: ["", [Validators.required]],
      supplierId: ["", [Validators.required]],
      paymentMethodId: ["", [Validators.required]],
      indentId: [""],
      // https://stackblitz.com/edit/angular-kucypd
      poDate: [
        formatDate(this.post.startDate, "yyyy-MM-dd", "en"),
        [Validators.required],
      ],
      deliveryDate: ["", [Validators.required]],
      description: [""],
      paymentId: [""],
    });
    this.addorderListForm = this.fb.group({
      productName: ["", [Validators.required]],
      quantity: ["", [Validators.required]],
      barCode: ["", [Validators.required]],
      supplierPriceId: ["", [Validators.required]],
      productExpiryDate: [, [Validators.required]],
    });
  }
  // Choose city using select dropdown
  changeProvince(event: Event) {
    console.log((event.target as HTMLInputElement).value);
    this.addPOForm.controls.provinceId.setValue(
      (event.target as HTMLInputElement).value,
      {
        onlySelf: true,
      }
    );
  }
  poAdd() {
    const formData: any = new FormData();
    formData.append(
      "branchLocationId",
      this.addPOForm.get("branchLocationId").value
    );
    formData.append("supplierId", this.addPOForm.get("supplierId").value);
    formData.append(
      "paymentMethodId",
      this.addPOForm.get("paymentMethodId").value
    );
    formData.append("paymentId", this.addPOForm.get("paymentId").value);
    formData.append("poDate", this.addPOForm.get("poDate").value);
    formData.append("description", this.addPOForm.get("description").value);
    formData.append("deliveryDate", this.addPOForm.get("deliveryDate").value);
    formData.append("createdBy", this.commonService.getUser());
    formData.append("id", this.commonService.getUserId());
    console.log(formData);
    if (!this.addPOForm.valid) {
      return false;
    } else {
      // alert(JSON.stringify(this.addPOForm.value));

      this.purchaseorderService
        .CreatePurchaseOrderHeader(formData)
        .pipe(takeUntil(this.destroyed$))
        .subscribe({
          next: (res: any) => {
            console.log(JSON.stringify(res));
            this.orderHeader = res.purchaseOrderId;
            this.alertifyService.success(
              JSON.stringify(res)
                .replace('"message":"', "")
                .replace('"', "")
                .replace("{", "")
                .replace("}", "")
            );
          },
          error: (error) => {
            this.alertifyService.error(
              JSON.stringify(error).replace('"', "").replace('"', "")
            );
            this.router.navigate(["../purchase/purchaseorder"]);
            console.log(error.error);
          },
          complete: () => {
            //  this.alertifyService.success('Saved Successfully.');
            // this.router.navigate(['../purchase/purchaseorderlist'], { queryParams: { orderId: this.saleHeader, customerId: this.customer.epfNo } });
            // this.router.navigate(['../purchase/purchaseorderreport',this.orderHeader], { queryParams: { purchaseOrderId: this.orderHeader } });
            this.router.navigate([
              "../purchase/purchaseorderreport",
              this.orderHeader,
            ]);
            // this.router.navigate(['../purchase/purchaseorderlist']);
          },
        });
    }
  }
  itemListAdd() {
    const formData: any = new FormData();
    formData.append("barCode", this.addorderListForm.get("barCode").value);
    formData.append("quantity", this.addorderListForm.get("quantity").value);
    formData.append(
      "branchLocationId",
      this.addPOForm.get("branchLocationId").value
    );
    formData.append(
      "supplierPriceId",
      this.addorderListForm.get("supplierPriceId").value
    );
    formData.append("supplierId", this.addPOForm.get("supplierId").value);
    formData.append("createdBy", this.commonService.getUser());
    formData.append(
      "productExpiryDate",
      this.addorderListForm.get("productExpiryDate").value
    );

    console.log(formData);
    if (!this.addorderListForm.valid) {
      return false;
    } else {
      // alert(JSON.stringify(this.addorderListForm.value));
      this.purchaseorderService
        .CreatepurchaseOrderitemdetails(formData)
        .pipe(takeUntil(this.destroyed$))
        .subscribe({
          next: (data: Data) => {
            this.lines = data.lines;
            console.log("purchase order list");
            console.log(data);
            //  this.count = data.count;
            //  this.totalQuantity = data.totalQuantity;
            this.subtotal = data.computeTotalValue;
            this.discount = data.computeDiscountValue;
            this.total = data.computeGrandTotalValue;
            //  this.count = data.count;
          },
          error: (error) => {},
          complete: () => {},
        });
    }
  }
  onFileChange(event) {
    // this.selectedFile = <File>event.target.files[0];
    //  console.log(this.selectedFile);
  }

  add() {
    //  console.log(e.target.value);
    // this.productId = e.target.value;
    this.productId = this.selectedProductcode;
    let productId: number | undefined;
    this.productService
      .getProduct(this.selectedProductcode)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: (data) => {
          this.addorderListForm.patchValue({
            productName: data.productName,
            barCode: data.barcode,
          });
          productId = data.productId;

          console.log(productId);
        },
        error: (error) => {},
        complete: () => {
          this.supplierPrices =
            this.priceService.getSupplierPriceBySupplierProductId(
              this.selectedItem,
              this.addPOForm.get("branchLocationId").value,
              productId
            );
          /*  .subscribe((data: any) => {
          this.supplierPrices=data;
          console.log(data)
        }); */
        },
      });
  }
  changeSupplier() {
    const formData: any = new FormData();
    formData.append(
      "branchLocationId",
      this.addPOForm.get("branchLocationId").value
    );
    formData.append("supplierId", this.selectedItem);
    console.log(this.selectedItem);
    this.products = this.priceService.getProductPriceBySupplierId(
      this.selectedItem,
      this.addPOForm.get("branchLocationId").value
    );
    if (this.show) {
      this.addPOForm.patchValue({
        branchLocationId: this.addPOForm.get("branchLocationId").value,
        supplierId: this.addPOForm.get("supplierId").value,
      });
      this.show = false;
    }
    console.log(this.products);
  }
  remove(id: any, supplierPriceId: any, productExpiryDate: any) {
    const formData: any = new FormData();
    formData.append("barCode", id);
    formData.append("supplierPriceId", supplierPriceId);
    formData.append("productExpiryDate", productExpiryDate);
    formData.append(
      "branchLocationId",
      this.addPOForm.get("branchLocationId").value
    );
    this.alertifyService.confirm(
      "Please Confirm.",
      "Are you sure, you want to delete this line?",
      () => {
        this.purchaseorderService
          .RemovePurchaseOrderItemDetails(formData)
          .pipe(takeUntil(this.destroyed$))
          .subscribe({
            next: (data: Data) => {
              this.lines = data.lines;
              console.log(data);
              //  this.count = data.count;
              //  this.totalQuantity = data.totalQuantity;
              this.subtotal = data.computeTotalValue;
              this.discount = data.computeDiscountValue;
              this.total = data.computeGrandTotalValue;
              //  this.count = data.count;
            },
            error: (error) => {},
            complete: () => {},
          });
        this.alertifyService.success("Deleted successfully.");
      },
      () => {
        this.initPurchaseOrder();
      }
    );
  }
  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }
}
