import { BranchLocation } from "./../../models/branchlocation";
import { Supplier } from "./../../models/supplier";
import { PurchaseOrderHeader } from "./../../models/purchaseorderheader";
import { PurchaseorderService } from "src/app/_services/purchaseorder.service";
import { AlertifyService } from "./../../_services/alertify.service";
import { CommonService } from "./../../_services/common.service";
import { Component, OnDestroy, OnInit } from "@angular/core";
import { FormBuilder } from "@angular/forms";
import { ActivatedRoute, Params, Router } from "@angular/router";
import { Subject } from "rxjs";
import { takeUntil } from "rxjs/operators";

@Component({
  templateUrl: "./purchaseorderreport.component.html",
  styleUrls: ["./purchaseorderreport.component.css"],
})
export class PurchaseorderreportComponent implements OnInit, OnDestroy {
  orderObj: number;
  lines: {};
  supplier: Supplier;
  branchLocation: BranchLocation;
  purchaseOrderHeader: PurchaseOrderHeader;
  private destroyed$ = new Subject();

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private alertifyService: AlertifyService,
    private commonService: CommonService,
    private purchaseorderService: PurchaseorderService
  ) {}

  ngOnInit() {
    this.route.queryParams.pipe(takeUntil(this.destroyed$)).subscribe({
      next: (queryParams: Params) => {
        this.orderObj = queryParams["purchaseOrderId"];
        console.log("works orderObj");
        console.log(this.orderObj);
      },
      error: () => {},
      complete: () => {},
    });
    this.orderObj = +this.route.snapshot.params.purchaseOrderId;
    this.purchaseorderService
      .GetPurchaseOrderHeder(true, +this.orderObj)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: (res: PurchaseOrderHeader) => {
          console.log(res);
          this.lines = res.purchaseOrderDetails;
          this.supplier = res.supplier;
          this.branchLocation = res.branchLocation;
          this.purchaseOrderHeader = res;
        },
        error: () => {},
        complete: () => {},
      });
  }
  ngOnDestroy() {
    this.destroyed$.next();
    this.destroyed$.complete();
  }
}
