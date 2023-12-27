import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { CommonService } from "src/app/_services/common.service";
import { ProductService } from "src/app/_services/product.service";
import { ReportService } from "src/app/_services/report.service";
import * as fileSaver from "file-saver";
import { PaginatedResult } from "src/app/models/pagination";
import { formatDate } from "@angular/common";
import { BranchLocation } from "src/app/models/branchlocation";
import { PurchaseOrderListReport } from "src/app/models/purchaseorderfilterlist";
import { Observable, Subject } from "rxjs";
import { takeUntil } from "rxjs/operators";
import { Supplier } from "src/app/models/supplier";
import { Product } from "src/app/models/product";
import { PriceService } from "src/app/_services/price.service";
import { SupplierPrice } from "src/app/models/supplierprice";

@Component({
  selector: "app-purchaseorderfilterlist",
  templateUrl: "./purchaseorderfilterlist.component.html",
  styleUrls: ["./purchaseorderfilterlist.component.css"],
})
export class PurchaseorderfilterlistComponent implements OnInit {
  total: number;
  quantity: number;
  totalStock: number;
  productId: number;
  supplierId: number;
  poListReportForm: FormGroup;
  poListDetails: PurchaseOrderListReport[];
  active: boolean = false;
  branches: BranchLocation[];
  suppliers: Observable<Supplier[]>;
  products: Observable<SupplierPrice[]>;
  filters: object[] = [
    { Id: 1, Name: "Supplier" },
    { Id: 2, Name: "Product" },
    { Id: 3, Name: "PO" },
    { Id: 4, Name: "Delivery" },
  ];

  filterId: number;
  private destroyed$ = new Subject();
  constructor(
    private commonService: CommonService,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private toastrService: ToastrService,
    private productService: ProductService,
    private reportService: ReportService,
  ) {
    this.getBranches();
  }

  ngOnInit() {
    this.createCustomerForm();
  }
  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }
  /*  */
  returnBlob(res): any {
    return new Blob([JSON.parse(res.headers.get("Content-Length"))], {
      type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
    });
  }
  bincardexcel() {
    const frmDate = this.poListReportForm.get("frmDate").value;
    const toDate = this.poListReportForm.get("toDate").value;
    const itemCode = this.poListReportForm.get("itemCode").value;
    const status = this.poListReportForm.get("active").value;
    this.productService.getProduct(itemCode).subscribe(
      (data) => {
        this.productId = data.productId;
      },
      (error) => {
        console.log(error);
      },
      () => {}
    );
    if (!this.poListReportForm.valid) {
      return false;
    } else {
      this.reportService
        .getExcelId(
          status,
          1,
          5,
          frmDate,
          toDate,
          "Purchase Order List",
          0,
          this.productId?.toString()
        )
        .subscribe(
          (response) => {
            let file = new Blob([response], {
              type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            });
            fileSaver.saveAs(
              file,
              "ItemWiseBinCard" +
                formatDate(new Date(), "yyyy-MM-dd hh:mm:ss", "en").toString() +
                ".xlsx"
            );
          },
          (error) => {
            console.error(`Error: ${error.message}`);
          }
        );
    }
  }
  get f() {
    return this.poListReportForm.controls;
  }
  createCustomerForm() {
    this.poListReportForm = this.fb.group({
      pofrmDate: [formatDate(new Date(Date.now()), "yyyy-MM-dd", "en")],
      potoDate: [formatDate(new Date(Date.now()), "yyyy-MM-dd", "en")],
      supplierId: [0],
      productId: [0],
      branchLocationId: ["", [Validators.required]],
      deliveryfrmDate: [formatDate(new Date(Date.now()), "yyyy-MM-dd", "en")],
      deliverytoDate: [formatDate(new Date(Date.now()), "yyyy-MM-dd", "en")],
      filterby: ["", [Validators.required]],
    });
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
  changeFilterBY() {
    this.filterId = this.poListReportForm.get("filterby").value;
    this.poListDetails=[];
    if (this.filterId == 1) {
      this.suppliers = this.commonService.getListByLocation(
        this.poListReportForm.get("branchLocationId").value,
        "Common",
        "getallsupplier"
      );
    }
    if (this.filterId == 2) {
      this.products = this.commonService.getListByLocation(
        this.poListReportForm.get("branchLocationId").value,
        "Common",
        "getallproduct"
      );
    }
  }
  submitForm() {
    this.loadReportData();
  }
  loadReportData() {
    var pOListCriteria = {
      branchLocationId:this.poListReportForm.get("branchLocationId").value,
      pOCreateFromDate: this.poListReportForm.get("pofrmDate")?.value,
      pOCreateToDate: this.poListReportForm.get("potoDate")?.value,
      productId: this.poListReportForm.get("productId")?.value,
      supplierId: this.poListReportForm.get("supplierId")?.value,
      deliveryFromDate: this.poListReportForm.get("deliveryfrmDate")?.value,
      deliveryToDate: this.poListReportForm.get("deliverytoDate")?.value,
      filterby: String(this.filterId),
    };
    this.reportService
      .getReportsId<PurchaseOrderListReport>(
        true,
        1,
        5,
        pOListCriteria.pOCreateFromDate,
        pOListCriteria.pOCreateToDate,
        "Purchase Order List",
        0,
        0,
        JSON.stringify(pOListCriteria)
      )
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: (res: PaginatedResult<PurchaseOrderListReport[]>) => {
          this.poListDetails = res.result;
          console.log(res.result);
          /* this.total =this.saleReportDetails.reduce( (x, y) => {
            return x + y.dNetValue;
        }, 0);
        this.quantity = this.saleReportDetails.reduce<number>((sum:number, t) => {return sum+t.dQuantity-t.dIssueQuantity}, 0);
        this.totalStock =this.saleReportDetails.reduce( (x, y) => {
          return x + y.dValue;
      }, 0); */
        },
        error: (error) => {
          this.toastrService.error(
            JSON.stringify(error).replace('"', "").replace('"', "")
          );
          console.log(error.error);
        },
        complete: () => {
          this.toastrService.success("Data Loading Successfully.");
          this.router.navigate(["/report/purchaseorderfilterlist"]);
        },
      });
  }
}
