import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PaginatedResult } from 'src/app/models/pagination';
import { CommonService } from 'src/app/_services/common.service';
import { ReportService } from 'src/app/_services/report.service';
import { ToastrService } from 'ngx-toastr';
import * as fileSaver from 'file-saver';
import { ProductService } from 'src/app/_services/product.service';
import { GetBinByItemCodeDto } from 'src/app/models/gettransactionbyItemcodedto';
import { formatDate } from '@angular/common';
import { takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-bincard',
  templateUrl: './bincard.component.html',
  styleUrls: ['./bincard.component.css']
})
export class BincardComponent implements OnInit {
  total : number;
  quantity: number;
  totalStock : number;
  productId : number;
  saleReportForm: FormGroup;
  saleReportDetails: GetBinByItemCodeDto[];
  active:boolean=false;
  private destroyed$ = new Subject();
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
        this.reportService.getReportsId<GetBinByItemCodeDto>(status, 1, 5, frmDate, toDate, 'Item Wise Bin Card',this.productId,0)
        .pipe(takeUntil(this.destroyed$))
        .subscribe(
          (res: PaginatedResult<GetBinByItemCodeDto[]>) => {
            this.saleReportDetails = res.result;
            /* this.total =this.saleReportDetails.reduce( (x, y) => {
              return x + y.dNetValue;
          }, 0);
          this.quantity = this.saleReportDetails.reduce<number>((sum:number, t) => {return sum+t.dQuantity-t.dIssueQuantity}, 0);
          this.totalStock =this.saleReportDetails.reduce( (x, y) => {
            return x + y.dValue;
        }, 0); */
          },
          (error) => {
            this.toastrService.error(
              JSON.stringify(error).replace('"', '').replace('"', '')
            );
            console.log(error.error);
          },
          () => {
            this.toastrService.success('Data Loading Successfully.');
            this.router.navigate(['/report/bincard']);
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
    this.productService.getProduct(itemCode)
    .pipe(takeUntil(this.destroyed$))
    .subscribe((data) => {this.productId=data.productId},(error) => {},() =>{});
    if (!this.saleReportForm.valid) {
      return false;
    } else {
      // alert(JSON.stringify(this.saleReportForm.value));
      this.reportService.getExcelId(status, 1, 5, frmDate, toDate, 'Item Wise Bin Card',0,this.productId?.toString())
      .pipe(takeUntil(this.destroyed$))
      .subscribe(response => {
          let file = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
          fileSaver.saveAs(file, 'ItemWiseBinCard'+  formatDate(new Date(), 'yyyy-MM-dd hh:mm:ss', 'en').toString()+'.xlsx');

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
