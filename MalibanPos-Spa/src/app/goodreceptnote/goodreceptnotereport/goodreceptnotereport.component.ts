import { GoodReceptNoteHeader } from "./../../models/goodreceptnoteheader";
import { GrnService } from "./../../_services/grn.service";
import { AlertifyService } from "./../../_services/alertify.service";
import { CommonService } from "./../../_services/common.service";
import { BranchLocation } from "./../../models/branchlocation";
import { Supplier } from "./../../models/supplier";
import { Component, OnInit } from "@angular/core";
import { FormBuilder } from "@angular/forms";
import { ActivatedRoute, Router, Params } from "@angular/router";
import { PurchasereturnService } from "src/app/_services/purchasereturn.service";
import { PurchaseReturnHeader } from "src/app/models/purchasereturnheader";
import { Subject } from "rxjs";
import { takeUntil } from "rxjs/operators";

@Component({
  templateUrl: "./goodreceptnotereport.component.html",
  styleUrls: ["./goodreceptnotereport.component.css"],
})
export class GoodreceptnotereportComponent implements OnInit {
  orderObj: number;
  lines: {};
  prnlines: any[] = [];
  supplier: Supplier;
  branchLocation: BranchLocation;
  goodReceptNoteHeader: GoodReceptNoteHeader;
  purchaseReturnHeader: PurchaseReturnHeader[];
  total: number;
  private destroyed$ = new Subject();

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private alertifyService: AlertifyService,
    private commonService: CommonService,
    private purchasereturnService: PurchasereturnService,
    private grnService: GrnService
  ) {}
  ngOnInit() {
    localStorage.removeItem("postData");
    this.orderObj = +this.route.snapshot.params.receiptOrderId;
    this.grnService
      .GetGoodReceptNoteHeader(true, +this.orderObj)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: (res: GoodReceptNoteHeader) => {
          console.log(res);
          this.lines = res.lines;
          this.supplier = res.supplier;
          this.branchLocation = res.branchLocation;
          this.goodReceptNoteHeader = res;
        },
        error: () => {},
        complete: () => {},
      });
    this.purchasereturnService
      .GetPurchaseReturnHederByGRNId(true, +this.orderObj)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: (res: PurchaseReturnHeader[]) => {
          console.log(res);
          res.forEach((x: any) => {
            this.prnlines.push(...x?.lines);
          });
          this.purchaseReturnHeader = res;
          this.total = res.reduce((a, b) => b.purchaseReturnAmount + a, 0);
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
