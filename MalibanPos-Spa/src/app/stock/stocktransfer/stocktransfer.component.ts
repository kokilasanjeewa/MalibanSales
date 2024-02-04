import { takeUntil } from 'rxjs/operators';
import { ProductService } from './../../_services/product.service';
import { CommonService } from 'src/app/_services/common.service';
import { formatDate } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, Subject } from 'rxjs';
import { BranchLocation } from 'src/app/models/branchlocation';
import {PriceDetail,ProductDetail,StockTransferDetail,StocktransferService} from 'src/app/_services/stocktransfer.service';
@Component({
  selector: 'app-stocktransfer',
  templateUrl: './stocktransfer.component.html',
  styleUrls: ['./stocktransfer.component.css'],
})
export class StocktransferComponent implements OnInit {
  selectedItem: number;
  selectedProductcode: string;
  addPOForm: FormGroup;
  addorderListForm: FormGroup;
  ProductDetails: Observable<ProductDetail[]>;
  PriceDetails: Observable<PriceDetail[]>;
  branches: Observable<BranchLocation[]>;
  transferBranches: BranchLocation[];
  payments: Observable<[]>;
  productId: any;
  lines: StockTransferDetail[];
  subtotal: number;
  discount: number;
  total: number;
  show: boolean = true;
  stockTransferHeaderId: number;
  private destroyed$ = new Subject();
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private alertifyService: AlertifyService,
    private commonService: CommonService,
    private productService: ProductService,
    private stocktransferService: StocktransferService,
  )
  {
    this.branches = this.commonService.getListByLocation( this.commonService.getUserBranchLocationId(), 'Common','getbranch');
    this.payments = this.commonService.getList('Common', 'getallpaymentmethod');
    this.getBranches();
  }
  ngOnInit() {
    this.stockTransferFormGroup();
    this.initStockTransferList();
    console.log(this.commonService.getUser());
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
  get f1() {
    return this.addorderListForm.controls;
  }
  stockTransferFormGroup() {
     this.addPOForm = this.fb.group({
      branchLocationId: ['', [Validators.required]],
      transferbranchLocationId: ['', [Validators.required]],
      paymentMethodId: ['', [Validators.required]],
      stockTransferDate: [formatDate(new Date(Date.now()), 'yyyy-MM-dd', 'en'),[Validators.required],],
    });
    this.addorderListForm = this.fb.group({
      productName: ['', [Validators.required]],
      quantity: ['', [Validators.required]],
      barCode: ['', [Validators.required]],
      priceId: ['', [Validators.required]],
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
  itemListAdd() {
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
  }
  onFileChange(event) {
    // this.selectedFile = <File>event.target.files[0];
    //  console.log(this.selectedFile);
  }
  add() {
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
  }
  changeBranch() {
    const formData: any = new FormData();
    formData.append(
      'branchLocationId',
      this.addPOForm.get('branchLocationId').value
    );

    this.ProductDetails = this.stocktransferService.getProducts(
      Number(this.addPOForm.get('branchLocationId').value),
      true
    );

    if (this.show) {
      this.addPOForm.patchValue({
        branchLocationId: this.addPOForm.get('branchLocationId').value,
      });
      this.show = false;
    }
  }
  changePrice(stock:number){

  }
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
  getBranches(){
    this.commonService.getList('Common', 'getallbranch').subscribe(data => {
      const mapBranches=this.commonService.getMapUserBranchLocations().length >0 ?  this.commonService.getMapUserBranchLocations() : [];
      const branches = mapBranches.length >1 ?
      data.filter(array => mapBranches.some(filter => filter == array.branchLocationId))
      : 0
      this.transferBranches =branches.filter(a => a.branchLocationId!=this.commonService.getUserBranchLocationId());
    });
  }
}
