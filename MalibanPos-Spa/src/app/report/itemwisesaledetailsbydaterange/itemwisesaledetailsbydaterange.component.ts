import { PaginatedResult } from './../../models/pagination';
import { ReportService } from './../../_services/report.service';
import { CommonService } from './../../_services/common.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import * as fileSaver from 'file-saver';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from 'src/app/_services/product.service';
import { SaleReportItemWiseDetail } from 'src/app/models/salereportitemwisedetail';
@Component({
  selector: 'app-itemwisesaledetailsbydaterange',
  templateUrl: './itemwisesaledetailsbydaterange.component.html',
  styleUrls: ['./itemwisesaledetailsbydaterange.component.css']
})
export class ItemwisesaledetailsbydaterangeComponent implements OnInit {
  total : number;
  quantity: number;
  returnQty: number;
  totalStock : number;
  productId : number;
  saleReportForm: FormGroup;
  saleReportDetails: SaleReportItemWiseDetail[];
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
        this.reportService.getReportsId<SaleReportItemWiseDetail>(status, 1, 5, frmDate, toDate, 'Date Wise Product Sales',this.productId,0).subscribe(
          (res: PaginatedResult<SaleReportItemWiseDetail[]>) => {
            this.saleReportDetails = res.result;
            this.total =this.saleReportDetails.reduce( (x, y) => {
              return x + y.saleValue;
          }, 0);
          this.quantity =this.saleReportDetails.reduce( (x, y) => {
            return x+(y.qty-y.returnQty);
        }, 0);
        this.returnQty =this.saleReportDetails.reduce( (x, y) => {
          return x+y.returnQty;
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
            this.router.navigate(['/report/datewiseproductsales']);
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
      this.reportService.getExcelId(status, 1, 5, frmDate, toDate, 'Date Wise Product Sales',0,this.productId?.toString()).
        subscribe(response => {
          let file = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
          fileSaver.saveAs(file, 'DateWiseProductSales.xlsx');

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
