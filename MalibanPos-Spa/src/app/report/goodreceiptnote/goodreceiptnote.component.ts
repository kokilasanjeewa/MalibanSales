import { PaginatedResult } from './../../models/pagination';
import { PurchaseReturnReportDetail } from './../../models/purchasereturnreportdetail';
import { ReportService } from './../../_services/report.service';
import { GrnService } from './../../_services/grn.service';
import { CommonService } from './../../_services/common.service';
import { GoodReceptNoteHeader } from './../../models/goodreceptnoteheader';
import { BranchLocation } from './../../models/branchlocation';
import { Supplier } from './../../models/supplier';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertifyService } from './../../_services/alertify.service';
import { Observable } from 'rxjs';
import { formatDate } from '@angular/common';
import { ToastrService } from 'ngx-toastr';

@Component({
  templateUrl: './goodreceiptnote.component.html',
  styleUrls: ['./goodreceiptnote.component.css']
})
export class GoodreceiptnoteComponent implements OnInit {
  lines: {};
  supplier: Supplier;
  branchLocation: BranchLocation;
  goodReceptNoteHeader: GoodReceptNoteHeader;
  goodReceptNoteHeaders: GoodReceptNoteHeader[];
  stockReportForm: FormGroup;
  purchasereturns: Observable<[]>;
  goodReceiptHeaders: Observable<[]>;
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private alertifyService: AlertifyService,
    private commonService: CommonService,
    private grnService: GrnService,
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
    const goodReceptNoteHeaderId = this.stockReportForm.get('goodReceptNoteHeaderId').value;
    if (!this.stockReportForm.valid) {
      return false;
    } else {
      this.grnService.GetGoodReceptNoteHeader(true, +goodReceptNoteHeaderId)
        .subscribe((res: GoodReceptNoteHeader) => {
          console.log(res)
          this.lines = res.lines;
          this.supplier = res.supplier;
          this.branchLocation = res.branchLocation;
          this.goodReceptNoteHeader = res;
        });
    }
  }
  searchGrn() {
    const frmDate = this.stockReportForm.get('frmDate').value;
    const toDate = this.stockReportForm.get('toDate').value;
    this.goodReceiptHeaders = this.commonService.getListByLocationFrmToDate(this.commonService.getUserBranchLocationId(), 'Common', 'getallgrn', frmDate, toDate);
  }
  stockexcel() {

  }
  createCustomerForm() {
    this.stockReportForm = this.fb.group({
      frmDate: ['', [Validators.required]],
      toDate: ['', [Validators.required]],
      goodReceptNoteHeaderId: [''],
      active: ['false', [Validators.required]]
    });
  }
}
