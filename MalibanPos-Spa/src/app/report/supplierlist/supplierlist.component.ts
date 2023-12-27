import { Supplier } from 'src/app/models/supplier';
import { PaginatedResult } from './../../models/pagination';
import { ReportService } from './../../_services/report.service';
import { CommonService } from './../../_services/common.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { formatDate } from '@angular/common';
import * as fileSaver from 'file-saver';

interface Post {
  startDate: Date;
  endDate: Date;
}
@Component({
  templateUrl: './supplierlist.component.html',
  styleUrls: ['./supplierlist.component.css']
})
export class SupplierlistComponent implements OnInit {
  post: Post = {
    startDate: new Date(Date.now()),
    endDate: new Date(Date.now())
  };
  supplierlistReportForm: FormGroup;
  branches: Observable<[]>;
  suppliers: Supplier[];
  constructor(
    private commonService: CommonService,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private toastrService: ToastrService,
    private reportService: ReportService
  ) {
    this.branches = this.commonService.getList('Common', 'getallbranch', 'false');
  }

  ngOnInit() {
    this.createCustomerForm();
  }
  getSuppliers() {
    const frmDate = this.supplierlistReportForm.get('frmDate').value;
    const toDate = this.supplierlistReportForm.get('toDate').value;
    const status = this.supplierlistReportForm.get('active').value;
    const id = this.supplierlistReportForm.get('branchLocationId').value;

    if (!this.supplierlistReportForm.valid) {
      return false;
    } else {
      this.reportService.getReportsId<Supplier>(status, 1, 5, frmDate, toDate, 'Supplier List', id).subscribe(
        (res: PaginatedResult<Supplier[]>) => {
          this.suppliers = res.result;
        },
        (error) => {
          this.toastrService.error(
            JSON.stringify(error).replace('"', '').replace('"', '')
          );
          console.log(error.error);
        },
        () => {
          this.toastrService.success('Data Loading Successfully.');
          this.router.navigate(['/report/supplierlist']);
        }
      );
    }
  }
  returnBlob(res): any {
    return new Blob([JSON.parse(res.headers.get('Content-Length'))], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
  }
  stockexcel() {
    const frmDate = this.supplierlistReportForm.get('frmDate').value;
    const toDate = this.supplierlistReportForm.get('toDate').value;
    const status = this.supplierlistReportForm.get('active').value;
    const id = this.supplierlistReportForm.get('branchLocationId').value;

    if (!this.supplierlistReportForm.valid) {
      return false;
    } else {
      this.reportService.getExcelId(status, 1, 5, frmDate, toDate, 'Supplier List', id).
        subscribe(response => {
          let file = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
          fileSaver.saveAs(file, 'SupplierList.xlsx');
        },
          error => {
            console.error(`Error: ${error.message}`);
          });
    }
  }
  get f() {
    return this.supplierlistReportForm.controls;
  }
  createCustomerForm() {
    this.supplierlistReportForm = this.fb.group({
      branchLocationId: ['', [Validators.required]],
      frmDate: [formatDate(this.post.startDate, 'yyyy-MM-dd', 'en'), [Validators.required]],
      toDate: [formatDate(this.post.startDate, 'yyyy-MM-dd', 'en'), [Validators.required]],
      active: ['false', [Validators.required]]
    });
  }

}
