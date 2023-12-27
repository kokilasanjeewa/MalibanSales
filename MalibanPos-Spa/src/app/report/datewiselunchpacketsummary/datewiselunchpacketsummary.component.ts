import { SaleReportDetailByCustomer } from './../../models/salereportdetailbycustomer';
import { PaginatedResult } from './../../models/pagination';
import { ReportService } from './../../_services/report.service';
import { CommonService } from './../../_services/common.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import * as fileSaver from 'file-saver';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { SaleSummaryReportDetail } from 'src/app/models/salesummaryreportdetail';
import { ProductService } from 'src/app/_services/product.service';
import { LunchPackSalesSummary } from 'src/app/models/lunchpacksalessummary';
@Component({
  selector: 'app-datewiselunchpacketsummary',
  templateUrl: './datewiselunchpacketsummary.component.html',
  styleUrls: ['./datewiselunchpacketsummary.component.css']
})
export class DatewiselunchpacketsummaryComponent implements OnInit {
  total : number;
  quantity: number;
  returnQty: number;
  totalStock : number;
  productId : number;
  saleReportForm: FormGroup;
  saleReportDetails: LunchPackSalesSummary[];
  active:boolean=false;
  constructor(
    private commonService: CommonService,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private toastrService: ToastrService,
    private productService: ProductService,
    private reportService: ReportService
  ) { }

  ngOnInit() {
    this.createCustomerForm();
  }
  saleAdd() {
    const frmDate = this.saleReportForm.get('frmDate').value;
    const toDate = this.saleReportForm.get('toDate').value;
    const itemCode =this.saleReportForm.get('itemCode').value;
    const status = this.saleReportForm.get('active').value;

    if (!this.saleReportForm.valid) {
      return false;
    } else {
      // alert(JSON.stringify(this.saleReportForm.value));
      this.productService.getProduct(itemCode).subscribe((data) => {
        this.productId=data.productId;
        this.reportService.getReportsId<LunchPackSalesSummary>(status, 1, 5, frmDate, toDate, 'Date Wise Lunch Packet Summary',this.productId,0).subscribe(
          (res: PaginatedResult<LunchPackSalesSummary[]>) => {
            this.saleReportDetails = res.result;
            this.total =this.saleReportDetails.reduce( (x, y) => {
              return x + (y.total * y.salePrice);
          }, 0);
          this.quantity =this.saleReportDetails.reduce( (x, y) => {
            return x+(y.total);
        }, 0);
            },
          (error) => {
            this.toastrService.error(
              JSON.stringify(error).replace('"', '').replace('"', '')
            );
            console.log(error.error);
          },
          () => {
            this.toastrService.success('Data Loading Successfully.');
            this.router.navigate(['/report/datewiselunchpacketsummary']);
          }
        );
      },(error) => {},() =>{

      });

    }
  }
  returnBlob(res): any {
    return new Blob([JSON.parse(res.headers.get('Content-Length'))], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
  }
  bincardexcel() {
    const frmDate = this.saleReportForm.get('frmDate').value;
    const toDate = this.saleReportForm.get('toDate').value;
    const itemCode =this.saleReportForm.get('itemCode').value;
    const status = this.saleReportForm.get('active').value;
    this.productService.getProduct(itemCode).subscribe((data) => {this.productId=data.productId},(error) => {},() =>{});
    if (!this.saleReportForm.valid) {
      return false;
    } else {
      // alert(JSON.stringify(this.saleReportForm.value));
      this.reportService.getExcelId(status, 1, 5, frmDate, toDate, 'Date Wise Lunch Packet Summary',0,this.productId?.toString()).
        subscribe(response => {
          let file = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
          fileSaver.saveAs(file, 'DateWiseLunchPacketSummary.xlsx');

        },
          error => {
            console.error(`Error: ${error.message}`);
          });
    }
  }
  get f() {
    return this.saleReportForm.controls;
  }
  createCustomerForm() {
    this.saleReportForm = this.fb.group({
      frmDate: ['', [Validators.required]],
      toDate: ['', [Validators.required]],
      itemCode: ['',[Validators.required]],
      active: [this.active, [Validators.required]]
    });
  }
}
