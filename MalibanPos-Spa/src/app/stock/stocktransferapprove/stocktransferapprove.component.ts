import { takeUntil } from 'rxjs/operators';
import { CommonService } from 'src/app/_services/common.service';
import { formatDate } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, Subject } from 'rxjs';
import { BranchLocation } from 'src/app/models/branchlocation';
import {StockTransferDetail,StockTransferHeader,StocktransferService} from 'src/app/_services/stocktransfer.service';

@Component({
  selector: 'app-stocktransferapprove',
  templateUrl: './stocktransferapprove.component.html',
  styleUrls: ['./stocktransferapprove.component.css']
})
export class StocktransferapproveComponent implements OnInit {
  addPOForm: FormGroup;
  branches: Observable<BranchLocation[]>;
  transferBranches: Observable<BranchLocation[]>;
  payments: Observable<[]>;
  lines: StockTransferDetail[];
  subtotal: number;
  discount: number;
  total: number;
  show: boolean = true;
  stockTransferHeaderId: number;
  stockTransferHeader: StockTransferHeader;

  private destroyed$ = new Subject();
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private alertifyService: AlertifyService,
    private commonService: CommonService,
    private stocktransferService: StocktransferService,
  )
  {
    this.branches = this.commonService.getListByLocation( this.commonService.getUserBranchLocationId(), 'Common','getbranch');
    this.payments = this.commonService.getList('Common', 'getallpaymentmethod');
    this.transferBranches=this.commonService.getList('Common', 'getallbranch');
  }
  ngOnInit() {
    this.stockTransferFormGroup();
    this.initStockTransferList();
    localStorage.removeItem("postData");
    this.stockTransferHeaderId = +this.route.snapshot.params.stockTransferHeaderId;
    this.stocktransferService.getStockTransferHeader(true, +this.stockTransferHeaderId).pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: (res: StockTransferHeader) => {
          this.populateFormValues(res);
        },
        error: (error) => {
          // Handle the error appropriately (e.g., display an error message)
        },
        complete: () => {
          // Perform any necessary cleanup or additional logic
        },
      });
  }
  private populateFormValues(res: StockTransferHeader) {
    this.stockTransferHeader = res;
    this.addPOForm.patchValue({
      branchLocationId: this.stockTransferHeader?.branchLocationId,
      transferbranchLocationId: this.stockTransferHeader?.transferBranchLocationId,
      paymentMethodId: this.stockTransferHeader?.paymentMethodId,
      stockTransferDate: formatDate(new Date(this.stockTransferHeader?.createDate), 'yyyy-MM-dd', 'en')
    });
  }
  initStockTransferList() {
    this.stocktransferService
      .getcachestocktransferdetails(
        this.commonService.getUserBranchLocationId()
      )
      .subscribe({
        next: (res: any) => {
          this.lines = res.getCacheStockTransferDetail;
          this.total=res.computeGrandTotalValue;
          this.subtotal=res.computeTotalValue;
          this.discount=res.computeDiscountValue;
          console.log('list');
          console.log(res);
        },
        error: (error) => {},
        complete: () => {},
      });
  }
  get f() {
    return this.addPOForm.controls;
  }
  stockTransferFormGroup() {
     this.addPOForm = this.fb.group({
      branchLocationId: ['', [Validators.required],],
      transferbranchLocationId: ['', [Validators.required]],
      paymentMethodId: ['', [Validators.required]],
      stockTransferDate: [formatDate(new Date(Date.now()), 'yyyy-MM-dd', 'en'),[Validators.required],],
    });
  }
  async stockTransferAdd() {
    const formData: any = new FormData();
    formData.append('branchLocationId',this.addPOForm.get('branchLocationId').value);
    formData.append('transferBranchLocationId',this.addPOForm.get('transferbranchLocationId').value);
    formData.append('paymentMethodId',this.addPOForm.get('paymentMethodId').value);
    formData.append('stockTransferDate',this.addPOForm.get('stockTransferDate').value);
    formData.append('createdBy', this.commonService.getUser());
    formData.append('id', this.commonService.getUserId());
    // console.log(formData);
    if (!this.addPOForm.valid) {
      return false;
    } else {
      this.stocktransferService
        .createStockTransferHeader(formData)
        .pipe(takeUntil(this.destroyed$))
        .subscribe({
          next: (res: any) => {
            this.stockTransferHeaderId = res.stockTransferHeaderId;
            this.alertifyService.success(res.message);
          },
          error: (error) => {
            this.alertifyService.error(JSON.stringify(error));
            this.router.navigate(['../stock/stocktransfer']);
          },
          complete: () => {
            this.router.navigate(['../stock/stocktransferreport',this.stockTransferHeaderId]);
          },
        });
    }
  }
/*   itemListAdd() {
    const formData: any = new FormData();
    formData.append(
      'branchLocationId',
      this.addPOForm.get('branchLocationId').value
    );
    formData.append('barCode', this.addorderListForm.get('barCode').value);
    formData.append('priceId', this.addorderListForm.get('priceId').value);
    formData.append('quantity', this.addorderListForm.get('quantity').value);
    formData.append('createdBy', this.commonService.getUser());
    console.log(formData);
    if (!this.addorderListForm.valid) {
      return false;
    } else {
      // alert(JSON.stringify(this.addorderListForm.value));
      this.stocktransferService
        .addStockTransferItem(formData)
        .pipe(takeUntil(this.destroyed$))
        .subscribe({
          next: (res: any) => {
            this.lines = res.stockTransferItems;
            this.total=res.computeGrandTotalValue;
            this.subtotal=res.computeTotalValue;
            this.discount=res.computeDiscountValue;
            console.log('purchase order list');
            console.log(res);
            //  this.count = data.count;
            //  this.totalQuantity = data.totalQuantity;
            //this.subtotal = data.computeTotalValue;
            //this.discount = data.computeDiscountValue;
            //this.total = data.computeGrandTotalValue;
          },
          error: (error) => {},
          complete: () => {},
        });
    }
  } */

/*   add() {
    this.productId = this.selectedProductcode;
    let productId: number | undefined;

    const handleProductData = ({ productName, barcode, productId }: any) => {
      this.addorderListForm.patchValue({
        productName,
        barCode: barcode,
      });
      this.PriceDetails = this.stocktransferService.getProductPrices(
        this.addPOForm.get('branchLocationId').value,
        productId
      );
    };

    this.productService
      .getProduct(this.selectedProductcode)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: handleProductData,
        error: (error) => {},
        complete: () => {},
      });
  } */
  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }
  confirmDeleteLine(id: any, priceId: any) {
    const formData: any = new FormData();
    formData.append('priceId', priceId);
    formData.append(
      'branchLocationId',
      this.addPOForm.get('branchLocationId').value
    );

    this.showConfirmationDialog(() => {
      this.deleteLine(id, formData);
    });
  }
  private showConfirmationDialog(confirmCallback: () => void) {
    this.alertifyService.confirm(
      'Please Confirm.',
      'Are you sure, you want to delete this line?',
      confirmCallback,
      () => {
        this.initStockTransferList();
      }
    );
  }
  private deleteLine(id: any, formData: any) {
    this.stocktransferService.removeline(id).subscribe({
      next: (res: any) => {
        console.log(res);
      },
      error: (error) => {
        this.initStockTransferList();
      },
      complete: () => {
          this.initStockTransferList();
      }
    });
  }
  approve(line:any,event:any){
    const isApprove=event.target.checked;
    this.stocktransferService.ApproveStockTransferDetail(line,isApprove)
    .pipe(takeUntil(this.destroyed$))
    .subscribe({
      next: (res: any) => {
        this.alertifyService.success(res.message);
      },
      error: (error) => {
        this.alertifyService.error(JSON.stringify(error));
        this.router.navigate(['../stock/stocktransferapprove',this.stockTransferHeaderId]);
      },
      complete: () => {
        this.router.navigate(['../stock/stocktransferapprove',this.stockTransferHeaderId]);
      },
    });
  }
}
