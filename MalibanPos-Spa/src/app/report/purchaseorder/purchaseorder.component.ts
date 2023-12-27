import { PurchaseorderService } from 'src/app/_services/purchaseorder.service';
import { PurchaseOrderHeader } from './../../models/purchaseorderheader';
import { Component, OnInit } from '@angular/core';
import { PaginatedResult } from './../../models/pagination';
import { PurchaseReturnReportDetail } from './../../models/purchasereturnreportdetail';
import { ReportService } from './../../_services/report.service';
import { CommonService } from './../../_services/common.service';
import { GoodReceptNoteHeader } from './../../models/goodreceptnoteheader';
import { BranchLocation } from './../../models/branchlocation';
import { Supplier } from './../../models/supplier';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertifyService } from './../../_services/alertify.service';
import { Observable } from 'rxjs';
import { formatDate } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
@Component({
  templateUrl: './purchaseorder.component.html',
  styleUrls: ['./purchaseorder.component.css']
})
export class PurchaseorderComponent implements OnInit {
  lines: {};
  supplier: Supplier;
  branchLocation: BranchLocation;
  purchaseOrderHeader: PurchaseOrderHeader;
  stockReportForm: FormGroup;
  purchaseOrderHeaders: Observable<[]>;
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private commonService: CommonService,
    private purchaseorderService: PurchaseorderService,
    private toastrService: ToastrService,
    private reportService: ReportService

  ) { }

  ngOnInit() {
    this.createCustomerForm();
  }
  get f() {
    return this.stockReportForm.controls;
  }
  stockAdd() {
    const purchaseOrderHeaderId = this.stockReportForm.get('purchaseOrderHeaderId').value;
    if (!this.stockReportForm.valid) {
      return false;
    } else {
        this.purchaseorderService.GetPurchaseOrderHeder(true, +purchaseOrderHeaderId)
        .subscribe((res: PurchaseOrderHeader) => {
          console.log(res)
          this.lines = res.purchaseOrderDetails;
          this.supplier = res.supplier;
          this.branchLocation = res.branchLocation;
          this.purchaseOrderHeader = res;
        });
    }
  }
  searchGrn() {
    const frmDate = this.stockReportForm.get('frmDate').value;
    const toDate = this.stockReportForm.get('toDate').value;
    this.purchaseOrderHeaders = this.commonService.getListByLocationFrmToDate(this.commonService.getUserBranchLocationId(), 'Common', 'getallpo', frmDate, toDate);
  }
  stockexcel() {

  }
  createCustomerForm() {
    this.stockReportForm = this.fb.group({
      frmDate: ['', [Validators.required]],
      toDate: ['', [Validators.required]],
      purchaseOrderHeaderId: [''],
      active: ['false', [Validators.required]]
    });
  }
}
