import { Component, OnInit } from "@angular/core";
import { Data } from "./../../models/data";
import { SupplierPrice } from "./../../models/supplierprice";
import { ProductService } from "./../../_services/product.service";
import { CommonService } from "src/app/_services/common.service";
import { formatDate } from "@angular/common";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { AlertifyService } from "src/app/_services/alertify.service";
import { ActivatedRoute, Router } from "@angular/router";
import { Observable, Subject } from "rxjs";
import { StockadjustmentService } from "./../../_services/stockadjustment.service";
import { TransactionDetail } from "src/app/models/transactiondetail";
import { BranchLocation } from "src/app/models/branchlocation";
import { StockAdjustmentHeader } from "src/app/models/stockadjustmentheader";
import { takeUntil } from "rxjs/operators";
interface Post {
  startDate: Date;
}
@Component({
  templateUrl: "./stockadjustmentconfirm.component.html",
  styleUrls: ["./stockadjustmentconfirm.component.css"],
})
export class StockadjustmentconfirmComponent implements OnInit {
  selectedItem: number;
  stockAdjustmentHeaderId: number;
  post: Post = {
    startDate: new Date(Date.now()),
  };
  selectedCostPrice: number;
  items$: Observable<TransactionDetail[]>;
  addSAForm: FormGroup;
  addsaListForm: FormGroup;
  products: Observable<SupplierPrice[]>;
  branches: BranchLocation[];
  adjustmenttypes: Observable<[]>;
  cApprovalTypes: {};
  //dcostprices: any;
  productId: any;
  lines: {};
  subtotal: number;
  discount: number;
  total: number;
  show: boolean = true;
  orderHeader: number;
  stockAdjustmentHeader: StockAdjustmentHeader;
  saItemList: Array<any> = [];
  private destroyed$ = new Subject();
  // submitted = false;
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private alertifyService: AlertifyService,
    private commonService: CommonService,
    private productService: ProductService,
    private stockadjustmentService: StockadjustmentService,
  ) {
    this.products = this.commonService.getList("Common", "getallproduct");
    //this.branches = this.commonService.getListByLocation(this.commonService.getUserBranchLocationId(), 'Common', 'getbranch');
    this.getBranches();
    this.adjustmenttypes = this.commonService.getList(
      "Common",
      "getalladjustmenttype"
    );
    this.cApprovalTypes = [
      { id: 1, name: "Approve" },
      { id: 2, name: "Reject" },
    ];
  }
  ngOnInit() {
    this.stockAdjustmentHeaderId =
      +this.route.snapshot.params.stockadjustmentHeaderId;
    this.stockadjustmentService
      .GetStockaAdjustmentHeaderDetails(this.stockAdjustmentHeaderId)
      .pipe(takeUntil(this.destroyed$))
      .subscribe((res: any) => {
        this.stockAdjustmentHeader = res.stockAdjustmentHeader;
        this.addSAForm.patchValue({
          branchLocationId: res.stockAdjustmentHeader.branchLocationId,
          adjustmentTypeId: res.stockAdjustmentHeader.adjustmentTypeId,
          stockAdjustmentDate: formatDate(
            res.stockAdjustmentHeader.stockAdjustmentDate,
            "yyyy-MM-dd",
            "en"
          ),
          stockTakenDate: formatDate(
            res.stockAdjustmentHeader.stockTakenDate,
            "yyyy-MM-dd",
            "en"
          ),
        });
        console.log(res);
        //  this.count = data.count;
        //  this.totalQuantity = data.totalQuantity;
        this.subtotal = res.computeGrandTotalValue;
        this.discount = 0;
        this.total = res.computeGrandTotalValue;
        //  this.count = data.count;
        const isEmpty = Object.keys(this.lines).length === 0;
        if (isEmpty) {
          this.lines = res.lines;
        }
      });
    this.createStockAdjustmentForm();
    this.initStockAdjustment();
    console.log(this.commonService.getUser());
  }
  initStockAdjustment() {
    this.stockadjustmentService
      .GetStockaAdjustment()
      .pipe(takeUntil(this.destroyed$))
      .subscribe((data: Data) => {
        this.lines = data.lines;
        console.log(data);
        //  this.count = data.count;
        //  this.totalQuantity = data.totalQuantity;
        this.subtotal = data.computeGrandTotalValue;
        this.discount = 0;
        this.total = data.computeGrandTotalValue;
        //  this.count = data.count;
      });
  }
  get f() {
    return this.addSAForm.controls;
  }
  get f1() {
    return this.addsaListForm.controls;
  }
  createStockAdjustmentForm() {
    const now = new Date();
    console.log(now.toISOString());
    this.addSAForm = this.fb.group({
      branchLocationId: ["", [Validators.required]],
      adjustmentTypeId: ["", [Validators.required]],
      // https://stackblitz.com/edit/angular-kucypd
      stockAdjustmentDate: [
        formatDate(this.post.startDate, "yyyy-MM-dd", "en"),
        [Validators.required],
      ],
      stockTakenDate: ["", [Validators.required]],
    });
    this.addsaListForm = this.fb.group({
      productName: ["", [Validators.required]],
      quantity: ["", [Validators.required]],
      barCode: ["", [Validators.required]],
      // dCostPrice: ['', [Validators.required]],
    });
  }
  saAdd() {
    const formData: any = new FormData();
    formData.append(
      "branchLocationId",
      this.addSAForm.get("branchLocationId").value
    );
    formData.append(
      "stockAdjustmentDate",
      this.addSAForm.get("stockAdjustmentDate").value
    );
    formData.append(
      "stockTakenDate",
      this.addSAForm.get("stockTakenDate").value
    );
    formData.append(
      "adjustmentTypeId",
      this.addSAForm.get("adjustmentTypeId").value
    );
    formData.append("id", this.commonService.getUserId());
    formData.append("modifiedBy", this.commonService.getUser());
    formData.append("stockAdjustmentHeaderId", this.stockAdjustmentHeaderId);
    console.log(formData);
    if (!this.addSAForm.valid) {
      return false;
    } else {
      // alert(JSON.stringify(this.addPOForm.value));

      this.stockadjustmentService
        .ConfirmStockaAdjustmentHeader(formData)
        .pipe(takeUntil(this.destroyed$))
        .subscribe(
          (res: any) => {
            console.log(JSON.stringify(res));
            this.orderHeader = res.purchaseOrderId;
            this.lines = res.lines;
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
            this.router.navigate([
              "../stock/stockadjustmentconfirm",
              this.stockAdjustmentHeaderId,
            ]);
            console.log(error.error);
          },
          () => {
            //  this.alertifyService.success('Saved Successfully.');
            // this.router.navigate(['../purchase/purchaseorderlist'], { queryParams: { orderId: this.saleHeader, customerId: this.customer.epfNo } });
            /*           this.router.navigate(['../purchase/purchaseorderreport'], { queryParams: { purchaseOrderId: this.orderHeader } }); */
            this.stockadjustmentService.ClearCart().subscribe((data: any) => {
              this.lines = [];
            });
            this.router.navigate(["../../stock/stockadjustmentlist"]);
          }
        );
    }
  }
  itemListAdd() {
    const formData: any = new FormData();
    formData.append("barCode", this.addsaListForm.get("barCode").value);
    // formData.append('costPrice', this.addsaListForm.get('salePrice').value);
    formData.append(
      "stockAdjustmentQuantity",
      this.addsaListForm.get("quantity").value
    );
    formData.append(
      "branchLocationId",
      this.addSAForm.get("branchLocationId").value
    );
    // formData.append('dCostPrice', this.addsaListForm.get('dCostPrice').value);
    formData.append("createdBy", this.commonService.getUser());
    console.log(formData);
    if (!this.addsaListForm.valid) {
      return false;
    } else {
      //  alert(JSON.stringify(this.addsaListForm.value));
      this.stockadjustmentService
        .CreateStockaAdjustmentItemDetails(formData)
        .pipe(takeUntil(this.destroyed$))
        .subscribe(
          (data: Data) => {
            this.lines = data.lines;
            console.log("purchase order list");
            console.log(data);
            //  this.count = data.count;
            //  this.totalQuantity = data.totalQuantity;
            this.subtotal = data.computeGrandTotalValue;
            this.discount = 0;
            this.total = data.computeGrandTotalValue;
            this.addsaListForm.patchValue({
              productName: null,
              barCode: null,
              quantity: null,
            });
            //  this.count = data.count;
          },
          (error) => {
            this.alertifyService.error(
              JSON.stringify(error).replace('"', "").replace('"', "")
            );
            this.router.navigate([
              "../stock/stockadjustmentapprove",
              this.stockAdjustmentHeaderId,
            ]);
            console.log(error.error);
          }
        );
    }
  }
  add(e) {
    console.log(e.target.value);
    this.productId = e.target.value;
    this.productService.getProduct(e.target.value).subscribe((data) => {
      this.addsaListForm.patchValue({
        productName: data.productName,
        barCode: data.barcode,
      });
      this.items$ = this.commonService.getListByLocation(
        this.addSAForm.get("branchLocationId").value,
        "Common",
        "getallstockdetail",
        data.barcode
      );
    });
  }
  remove(id: any, dSalePrice: number) {
    const formData: any = new FormData();
    formData.append("barCode", id);
    formData.append("dSalePrice", dSalePrice);
    formData.append(
      "branchLocationId",
      this.addSAForm.get("branchLocationId").value
    );
    this.alertifyService.confirm(
      "Please Confirm.",
      "Are you sure, you want to delete this line?",
      () => {
        this.stockadjustmentService
          .RemoveStockaAdjustmentItemDetails(formData)
          .subscribe((data: Data) => {
            this.lines = data.lines;
            console.log(data);
            //  this.count = data.count;
            //  this.totalQuantity = data.totalQuantity;
            this.subtotal = data.computeGrandTotalValue;
            this.discount = 0;
            this.total = data.computeGrandTotalValue;
            //  this.count = data.count;
          });
        this.alertifyService.success("Deleted successfully.");
      },
      () => {
        this.initStockAdjustment();
      }
    );
  }
  getBranches() {
    this.commonService.getList("Common", "getallbranch").subscribe((data) => {
      const mapBranches =
        this.commonService.getMapUserBranchLocations().length > 0
          ? this.commonService.getMapUserBranchLocations()
          : this.commonService.getUserBranchLocationId();
      const branches =
        mapBranches.length > 1
          ? data.filter((array) =>
              mapBranches.some((filter) => filter == array.branchLocationId)
            )
          : data.filter(
              (d) =>
                d.branchLocationId ==
                this.commonService.getUserBranchLocationId()
            );
      this.branches = branches;
    });
  }
  clear() {
    this.stockadjustmentService.ClearCart().subscribe((data: any) => {
      this.lines = [];
    });
  }
}
