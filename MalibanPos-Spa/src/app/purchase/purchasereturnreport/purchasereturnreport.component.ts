import { PurchaseReturnHeader } from './../../models/purchasereturnheader';
import { BranchLocation } from './../../models/branchlocation';
import { Supplier } from './../../models/supplier';
import { PurchaseOrderHeader } from './../../models/purchaseorderheader';
import { PurchaseorderService } from 'src/app/_services/purchaseorder.service';
import { AlertifyService } from './../../_services/alertify.service';
import { CommonService } from './../../_services/common.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { PurchasereturnService } from 'src/app/_services/purchasereturn.service';
@Component({
  templateUrl: './purchasereturnreport.component.html',
  styleUrls: ['./purchasereturnreport.component.css']
})
export class PurchasereturnreportComponent implements OnInit {
  orderObj: number;
  lines: {};
  supplier: Supplier;
  branchLocation: BranchLocation;
  purchaseReturnHeader: PurchaseReturnHeader;
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private alertifyService: AlertifyService,
    private commonService: CommonService,
    private purchasereturnService: PurchasereturnService,
  ) { }

  ngOnInit() {
    this.route.queryParams
      .subscribe((queryParams: Params) => {
        this.orderObj = queryParams['purchaseReturnId'];
        console.log('works orderObj');
        console.log(this.orderObj);
      });
    this.orderObj = +this.route.snapshot.params.purchaseReturnId;
    console.log('works purchaseReturnId');
    console.log(this.orderObj);
    this.purchasereturnService.GetPurchaseReturnHeder(true, +this.orderObj)
      .subscribe((res: PurchaseReturnHeader) => {
        console.log(res)
        this.lines = res.lines;
        this.supplier = res.supplier;
        this.branchLocation = res.branchLocation;
        this.purchaseReturnHeader = res;
      });
  }
}
