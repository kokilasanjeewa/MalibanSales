import { SupplierProduct } from "./../../models/supplierproduct";
import { TransactionDetail } from "./../../models/transactiondetail";
import { Product } from "./../../models/product";
import { PriceService } from "./../../_services/price.service";
import { ProductService } from "./../../_services/product.service";
import { CommonService } from "./../../_services/common.service";
import { formatDate } from "@angular/common";
import { Component, OnInit } from "@angular/core";
import { FormGroup, Validators, FormBuilder } from "@angular/forms";
import { Observable, Subject } from "rxjs";
import { ActivatedRoute, Data, Router } from "@angular/router";
import { AlertifyService } from "src/app/_services/alertify.service";
import { PurchasereturnService } from "src/app/_services/purchasereturn.service";
import { SupplierproductService } from "src/app/_services/supplierproduct.service";
import { GrnService } from "src/app/_services/grn.service";
import { distinct, takeUntil } from "rxjs/operators";
import { BranchLocation } from "src/app/models/branchlocation";
interface Post {
  startDate: Date;
  endDate: Date;
}
@Component({
  templateUrl: "./purchasereturn.component.html",
  styleUrls: ["./purchasereturn.component.css"],
})
export class PurchasereturnComponent implements OnInit {
  selectedItem: number;
  selectedCode: string;
  selectedCostPrice: number;
  post: Post = {
    startDate: new Date(Date.now()),
    endDate: new Date(Date.now()),
  };
  addPOForm: FormGroup;
  addorderListForm: FormGroup;
  products: Observable<SupplierProduct[]>;
  branches: Observable<BranchLocation[]>;
  payments: Observable<[]>;
  returnReasons: Observable<[]>;
  suppliers: Observable<[]>;
  indents: Observable<[]>;
  productId: any;
  lines: TransactionDetail[];
  subtotal: number;
  discount: number;
  total: number;
  show: boolean = true;
  orderHeader: number;
  goodReceptNoteHeaderId: number;
  items$: Observable<TransactionDetail[]>;
  private destroyed$ = new Subject();

  // submitted = false;
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private alertifyService: AlertifyService,
    private commonService: CommonService,
    private productService: ProductService,
    private purchasereturnService: PurchasereturnService,
    private supplierproductService: SupplierproductService,
    private priceService: PriceService,
    private grnService: GrnService
  ) {
    //this.products = this.commonService.getList('Common', 'getallproduct');
    // this.branches = this.commonService.getList('Common', 'getallbranch');
    this.branches = this.commonService.getListByLocation(
      this.commonService.getUserBranchLocationId(),
      "Common",
      "getbranch"
    );
    // this.getBranches();
    this.suppliers = this.commonService.getListByLocation(
      this.commonService.getUserBranchLocationId(),
      "Common",
      "getallsupplier"
    );
    this.payments = this.commonService.getList("Common", "getallpaymentmethod");
    this.returnReasons = this.commonService.getList(
      "Common",
      "getallreturntype"
    );
  }
  /* changeBranch(){
    this.suppliers = this.commonService.getListByLocation(this.addPOForm.get('branchLocationId').value, 'Common', 'getallsupplier');

  } */
  ngOnInit() {
    this.createSupplierRegisterForm();
    this.initPurchaseOrder();
    console.log(this.commonService.getUser());
  }
  initPurchaseOrder() {
    this.purchasereturnService
      .GetPurchaseReturn()
      .pipe(takeUntil(this.destroyed$))
      .subscribe((data: Data) => {
        this.lines = data.lines;
        console.log(data);
        //  this.count = data.count;
        //  this.totalQuantity = data.totalQuantity;
        this.subtotal = data.computeTotalValue;
        this.discount = data.computeDiscountValue;
        this.total = data.computeGrandTotalValue;
        //  this.count = data.count;
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
      goodReceptNoteHeaderId: ["", [Validators.required]],
      returnTypeId: ["", [Validators.required]],
      // https://stackblitz.com/edit/angular-kucypd
      poDate: [
        formatDate(this.post.startDate, "yyyy-MM-dd", "en"),
        [Validators.required],
      ],
      description: [""],
    });
    this.addorderListForm = this.fb.group({
      productId: ["", [Validators.required]],
      productName: ["", [Validators.required]],
      quantity: ["", [Validators.required]],
      dCostPrice: ["", [Validators.required]],
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
    formData.append("returnTypeId", this.addPOForm.get("returnTypeId").value);
    formData.append("purchaseReturnDate", this.addPOForm.get("poDate").value);
    formData.append(
      "goodReceptNoteHeaderId",
      this.addPOForm.get("goodReceptNoteHeaderId").value
    );
    formData.append("description", this.addPOForm.get("description").value);
    formData.append("createdBy", this.commonService.getUser());
    console.log(formData);
    if (!this.addPOForm.valid) {
      return false;
    } else {
      // alert(JSON.stringify(this.addPOForm.value));
      this.purchasereturnService
        .CreatePurchaseReturnHeader(formData)
        .pipe(takeUntil(this.destroyed$))
        .subscribe(
          (res: any) => {
            console.log(JSON.stringify(res));
            this.orderHeader = res.purchaseReturnId;
            this.alertifyService.success(
              JSON.stringify(res)
                .replace('"message":"', "")
                .replace('"', "")
                .replace("{", "")
                .replace("}", "")
            );
          },
          (error) => {
            this.alertifyService.error(
              JSON.stringify(error).replace('"', "").replace('"', "")
            );
            this.router.navigate(["../purchase/purchasereturn"]);
            console.log(error.error);
          },
          () => {
            //  this.alertifyService.success('Saved Successfully.');
            // this.router.navigate(['../purchase/purchaseorderlist'], { queryParams: { orderId: this.saleHeader, customerId: this.customer.epfNo } });
            /*     this.router.navigate(['../purchase/purchasereturnreport'], { queryParams: { purchaseReturnId: this.orderHeader } }); */
            this.router.navigate(["../purchase/purchasereturnlist"]);
          }
        );
    }
  }
  itemListAdd() {
    const formData: any = new FormData();
    formData.append("productId", this.addorderListForm.get("productId").value);
    formData.append("quantity", this.addorderListForm.get("quantity").value);
    formData.append("priceId", this.addorderListForm.get("dCostPrice").value);
    formData.append(
      "branchLocationId",
      this.addPOForm.get("branchLocationId").value
    );
    formData.append("goodReceptNoteHeaderId", this.goodReceptNoteHeaderId);
    formData.append("createdBy", this.commonService.getUser());
    console.log(formData);
    if (!this.addorderListForm.valid) {
      return false;
    } else {
      //alert(JSON.stringify(this.addorderListForm.value));
      this.purchasereturnService
        .CreatepurchaseReturnitemdetails(formData)
        .pipe(takeUntil(this.destroyed$))
        .subscribe(
          (data: Data) => {
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
          (error) => {
            this.alertifyService.error(
              JSON.stringify(error).replace('"', "").replace('"', "")
            );
            this.router.navigate(["../purchase/purchasereturn"]);
            console.log(error.error);
          },
          () => {
            //  this.alertifyService.success('Saved Successfully.');
            // this.router.navigate(['../purchase/purchaseorderlist'], { queryParams: { orderId: this.saleHeader, customerId: this.customer.epfNo } });
            /*     this.router.navigate(['../purchase/purchasereturnreport'], { queryParams: { purchaseReturnId: this.orderHeader } }); */
            this.router.navigate(["../purchase/purchasereturn"]);
          }
        );
    }
  }
  onFileChange(event) {
    // this.selectedFile = <File>event.target.files[0];
    //  console.log(this.selectedFile);
  }
  /*  changeBranch(e) {
     console.log(e.target.value);
     this.requestlist= this.commonService.getList('Common', 'getallpurchaserequest').subscribe
       ((data) => {
         console.log(data);
          data.filter(p => p.branchLocationId === e.target.value);
       });
   } */
  add() {
    this.productService
      .getProductById(this.selectedCode)
      .pipe(takeUntil(this.destroyed$))
      .subscribe((data) => {
        this.addorderListForm.patchValue({
          productId: data.productId,
          productName: data.productName,
        });
        this.items$ = this.commonService.getListByLocation(
          this.addPOForm.get("branchLocationId").value,
          "Common",
          "getallstockdetail",
          data.barcode
        );
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
    this.products = this.supplierproductService.getProductBySupplierId(
      this.selectedItem,
      this.addPOForm.get("branchLocationId").value
    );
    this.grnService
      .GetgrnbyBranchSupplier(
        this.selectedItem,
        this.addPOForm.get("branchLocationId").value
      )
      .pipe(takeUntil(this.destroyed$))
      .subscribe((data: any) => {
        this.goodReceptNoteHeaderId = data.goodReceptNoteHeaderId;
        console.log(this.goodReceptNoteHeaderId);
      });
    // this.products = this.priceService.getProductPriceBySupplierId(this.selectedItem, this.addPOForm.get('branchLocationId').value);
    if (this.show) {
      this.addPOForm.patchValue({
        branchLocationId: this.addPOForm.get("branchLocationId").value,
        supplierId: this.addPOForm.get("supplierId").value,
        goodReceptNoteHeaderId: this.addPOForm.get("goodReceptNoteHeaderId")
          .value,
      });
      this.show = false;
    }
    console.log(this.products);
  }
  remove(id: any, dSalePrice: number) {
    const formData: any = new FormData();
    formData.append(
      "branchLocationId",
      this.addPOForm.get("branchLocationId").value
    );
    formData.append("dSalePrice", dSalePrice);
    formData.append("barCode", id);
    this.alertifyService.confirm(
      "Please Confirm.",
      "Are you sure, you want to delete this line?",
      () => {
        this.purchasereturnService
          .RemovePurchaseReturnItemDetails(formData)
          .pipe(takeUntil(this.destroyed$))
          .subscribe((data: Data) => {
            this.lines = data.lines;
            console.log(data);
            //  this.count = data.count;
            //  this.totalQuantity = data.totalQuantity;
            this.subtotal = data.computeTotalValue;
            this.discount = data.computeDiscountValue;
            this.total = data.computeGrandTotalValue;
            //  this.count = data.count;
          });
        this.alertifyService.success("Deleted successfully.");
      },
      () => {
        this.initPurchaseOrder();
      }
    );
  }
  /*   getBranches(){
    this.commonService.getList('Common', 'getallbranch').subscribe(data => {
      const mapBranches=this.commonService.getMapUserBranchLocations().length >0 ?  this.commonService.getMapUserBranchLocations() : this.commonService.getUserBranchLocationId();
      const branches = mapBranches.length >1 ?
      data.filter(array => mapBranches.some(filter => filter == array.branchLocationId))
      : data.filter(d => d.branchLocationId==this.commonService.getUserBranchLocationId());
      this.branches =branches;
    });
  } */
}
