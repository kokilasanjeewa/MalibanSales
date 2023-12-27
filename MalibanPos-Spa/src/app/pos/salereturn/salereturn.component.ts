import { PricemodalComponent } from './../pricemodal/pricemodal.component';
import { TransactionDetail } from './../../models/transactiondetail';
import { CartReturnData, OrderData } from './../../models/orderdata';
import { CommonService } from './../../_services/common.service';
import { CustomerService } from './../../_services/customer.service';
import { environment } from 'src/environments/environment';
import { formatDate } from '@angular/common';
import { HttpClient } from '@angular/common/http';

import {
  Component,
  ElementRef,
  OnInit,
  ViewChild,
  HostListener,
  Renderer2,
} from '@angular/core';
import { Router } from '@angular/router';
import { CartService } from 'src/app/_services/cart.service';
import { AlertifyService } from './../../_services/alertify.service';
import { ToastrService } from 'ngx-toastr';
import { Observable, Subject, timer } from 'rxjs';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import * as $ from 'jquery';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { MessagemodelComponent } from '../messagemodel/messagemodel.component';
import { takeUntil } from 'rxjs/operators';
interface PostData {
  returnCustomer: Customer;
  billNumber: number;
  isReadOnly: boolean;
}
interface Post {
  startDate: Date;
  endDate: Date;
}
interface Customer {
  number: string;
  fullName: string;
  creditLimit: number;
  creditBalance: number;
  cashLimit: number;
  cashBalance: number;
  epfNo: string;
}
@Component({
  selector: 'app-salereturn',
  templateUrl: './salereturn.component.html',
  styleUrls: ['./salereturn.component.css'],
})
export class SalereturnComponent implements OnInit {
  //for default ngx-bootstrap modal
  public modalRef: BsModalRef;
  selectedItem: string;
  itemQuantity: number;
  selectedItemIndex: number;
  items$: TransactionDetail[];
  items$$: Observable<TransactionDetail[]>;
  item: TransactionDetail;
  arrayitem: Customer[];
  [x: string]: any;
  public customerNumber: String;
  public billNumber: number;
  public billActive: boolean = false;
  public checkdisable: boolean = false;
  public scanCode: String;
  public priceVersion: String = '';
  baseUrl = environment.apiUrl + 'Sale/';
  post: Post = {
    startDate: new Date(Date.now()),
    endDate: new Date(Date.now()),
  };
  billDate = new Date(Date.now());
  subtotal: number;
  returntotal: number;
  total: number;
  discount: number;
  totalQuantity: number;
  count: number;
  lines: {};
  customer: Customer = {
    number: '',
    fullName: '',
    creditLimit: 0,
    creditBalance: 0,
    cashLimit: 0,
    cashBalance: 0,
    epfNo: '',
  };
  postData: PostData = {
    returnCustomer: this.customer,
    billNumber: 0,
    isReadOnly: false,
  };
  orderdata: OrderData;
  saleHeader: {};
  isReadonly = false;
  isReturnReadonly = false;
  barcodeCount: number;
  bsModalRef: BsModalRef;
  private countClick = 0;
  paymentType: number;
  private destroyed$ = new Subject();

  constructor(
    private router: Router,
    private alertifyService: AlertifyService,
    private cartService: CartService,
    private http: HttpClient,
    private renderer: Renderer2,
    private customerService: CustomerService,
    private toastrService: ToastrService,
    private commonService: CommonService,
    private modalService: BsModalService
  ) { }
  // https://stackoverflow.com/questions/37362488/how-can-i-listen-for-keypress-event-on-the-whole-page

  openModalWithMessageComponent(message: string) {
    this.bsModalRef = this.modalService.show(MessagemodelComponent, {
      initialState: message,
      class: 'gray modal-md modal-dialog-centered',
      backdrop: true,
      ignoreBackdropClick: false,
      animated: false,
    });
    this.bsModalRef.content.updateErrorAdd.subscribe((data) => {
      //  console.log(data);
    });
  }
  ngOnInit() {
    let branchLocationId = this.commonService.getUserBranchLocationId();
    //  this.selectedItem = null;
    var Data = localStorage.getItem('returnData');
    if (Data != null) {
      this.postData = JSON.parse(Data);
      this.customer = this.postData.returnCustomer;
      this.billNumber = this.postData.billNumber;
      this.isReturnReadonly = this.postData.isReadOnly;
    }
    this.items$$ = this.commonService
      .getListByLocation(
        this.commonService.getUserBranchLocationId(),
        'Common',
        'getallstockdetail',
        ''
      )
      .pipe(takeUntil(this.destroyed$));
    // this.arrayitem = JSON.parse(localStorage.getItem('ListaPermessi'));
    this.toastrService.info('initital load Successfully.');
    //  this.alertifyService.success('initital load Successfully.');
    this.initCart();
    // this.ngAfterViewInit();
  }
  initCart() {
    this.cartService
      .GetCart()
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: (data: OrderData) => {
          this.lines = data.lines;
          this.totalQuantity = data.totalQuantity;
          this.returntotal = data.computeReturnTotal;
          this.subtotal = data.computeTotal;
          this.discount = data.computeDiscountValue;
          this.total = data.computeGrandTotalValue;
          this.count = data.count;
          this.billActive = data.billActive;
          this.selectedItem = null;
          //  console.log(data);
          //  console.log('works');
        },
        error: (error) => {
          this.toastrService.error(error);
          //   this.alertifyService.error(error);
        },
        complete: () => {
          this.selectedItem = null;
          /*     const postData = localStorage.getItem('returnData');
      this.returnData = JSON.parse(postData);
      this.customer = this.returnData.customer;
      this.isReadonly =this.returnData.isReadonly;
      this.billNumber =this.returnData.billNumber; */
          this.toastrService.success('Item add successfully.');
          //  this.alertifyService.success('Item add successfully.');
        },
      });
  }
  Billadd(event: Event) {
    const Value = (event.target as HTMLInputElement).value;
    const billNumber = Value.trim().toLowerCase();
    //   console.log(billNumber);
    this.cartService
      .GetReturnDetails(
        Number(billNumber),
        Number(this.commonService.getUserBranchLocationId())
      )
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: (data: CartReturnData) => {
          console.log('return data testing');
          console.log(data);
          this.lines = data.cart1.lines;
          this.totalQuantity = data.cart1.totalQuantity;
          this.subtotal = data.cart1.computeTotal;
          this.returntotal = data.cart1.computeReturnTotal;
          this.discount = data.cart1.computeDiscountValue;
          this.total = data.cart1.computeGrandTotalValue;
          this.count = data.cart1.count;
          this.billActive = data.cart1.billActive;
          this.isReadonly = true;
          this.isReturnReadonly = data.cart1.billActive;
          this.paymentType = data.paymentType;
          /*       console.log(this.receiptDetails);
         this.total = this.receiptDetails.reduce((a, b) => (b.price.salePrice * b.quantity) + a, 0);
         this.profit = this.receiptDetails.reduce((a, b) => (b.price.maximumRetailPrice * b.quantity) + a, 0);
         console.log(this.total); */
        },
        error: (error) => {
          this.toastrService.error(error);
        },
        complete: () => {
          this.toastrService.success('Bill successfully loading.');
        },
      });
    this.customerService
      .GetCustomerBySaleHeaderId(
        Number(billNumber),
        Number(this.commonService.getUserBranchLocationId())
      )
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: (data: Customer) => {
          //   console.log(data);
          this.customer = data;
          this.postData.returnCustomer = data;
          this.postData.billNumber = Number(billNumber);
          this.postData.isReadOnly = true;
          localStorage.setItem('returnData', JSON.stringify(this.postData));
        },
        error: (err) => { },
        complete: () => { },
      });
    /*       this.returnData1.billNumber =Number(billNumber);
    this.returnData1.customer =this.customer;
    this.returnData.isReadonly = true; */
  }
  onChangeEvent(
    event: any,
    barcode: string,
    dSalePrice: number,
    dCostPrice: number,
    transactionDetailId: number
  ) {
    var qty = Number(event.target.value);
    //  console.log(barcode);
    if (qty >= 0) {
      const formData: any = new FormData();
      formData.append('paymentId', this.paymentType);
      formData.append('barCode', barcode);
      formData.append('quantity', qty);
      formData.append('dSalePrice', dSalePrice);
      formData.append('dCostPrice', dCostPrice);
      formData.append('transactionDetailId', transactionDetailId);
      formData.append(
        'billDate',
        formatDate(this.billDate, 'yyyy-MM-dd', 'en')
      );
      formData.append('number', this.customer.number);
      formData.append(
        'branchLocationId',
        Number(this.commonService.getUserBranchLocationId())
      );
      // console.log('Orderadd');
      // console.log(this.commonService.getUserBranchLocationId());
      this.cartService
        .CreateSaleItemQuantity(formData)
        .pipe(takeUntil(this.destroyed$))
        .subscribe({
          next: (data: OrderData) => {
            this.lines = data.lines;
            this.totalQuantity = data.totalQuantity;
            this.subtotal = data.computeTotal;
            this.returntotal = data.computeReturnTotal;
            this.discount = data.computeDiscountValue;
            this.total = data.computeGrandTotalValue;
            this.count = data.count;
            //   console.log(data);
            //   console.log('works');
          },
          error: (error) => {
            if (error.message === 'Your credit limit is over.') {
              this.openModalWithMessageComponent(error.message);
            }
            this.toastrService.error(error.message);
            this.lines = error.cart.lines;
            this.totalQuantity = error.cart.totalQuantity;
            this.subtotal = error.cart.computeTotal;
            this.discount = error.cart.computeDiscountValue;
            this.total = error.cart.computeGrandTotalValue;
            this.count = error.cart.count;
            //  console.log(error);
            //  this.alertifyService.error(error);
          },
          complete: () => {
            this.toastrService.success('Item add successfully.');
            this.initCart();
          },
        });
      this.scanCode = '';
    }
  }
  clearCart() {
    return this.http
      .get(this.baseUrl + 'clear')
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: (response) => {
          this.selectedItem = null;
          this.total = 0.0;
          //  console.log(response);
        },
        error: (error) => {
          //  this.alertifyService.error(JSON.stringify(error));
          this.toastrService.error(JSON.stringify(error));
          //     console.log(error);
        },
        complete: () => {
          this.lines = null;
          this.lines = [];
          this.total = 0.0;
          this.count = 0;
          this.discount = 0.0;
          this.subtotal = 0.0;
          this.totalQuantity = 0;
          this.customer.number = '';
          this.customer.fullName = '';
          this.customer.creditLimit = 0.0;
          this.customer.creditBalance = 0.0;
          this.customer.epfNo = '';
          this.isReadonly = false;
          this.isReturnReadonly = false;
          this.customerNumber = '';
          this.selectedItem = null;
          this.billNumber = null;
          localStorage.removeItem('returnData');
          this.ngOnInit();
          this.toastrService.success('Clear Successfully.');
          //  this.alertifyService.success('Clear Successfully.');
          // this.router.navigate(['../../pos/order']);
        },
      });
  }

  salereturn() {
    this.checkdisable = true;
    const formData: any = new FormData();
    formData.append('employeeCode', this.customer.number);
    formData.append(
      'branchLocationId',
      this.commonService.getUserBranchLocationId()
    );
    formData.append('saleHeaderId', this.billNumber);
    formData.append('createdBy', this.commonService.getUser());
    formData.append('id', this.commonService.getUserId());
    if (this.count == 0 && this.customer.epfNo == '') {
      this.toastrService.error('Please check with order details');
    } else {
      this.cartService
        .CreateSaleReturnHeader(formData)
        .pipe(takeUntil(this.destroyed$))
        .subscribe({
          next: (data: OrderData) => {
            this.lines = data.lines;
            this.saleHeader = data.saleHeader;
            this.totalQuantity = data.totalQuantity;
            this.subtotal = data.computeTotal;
            this.discount = data.computeDiscountValue;
            this.total = data.computeGrandTotalValue;
            this.count = data.count;
            this.customer.number = '';
            this.customer.fullName = '';
            this.customer.creditLimit = 0.0;
            // this.customer.epfNo = 0;
            //   console.log('CreateSaleHeader Works');
            //    console.log(data);
            this.clearCart();
          },
          error: (error) => {
            this.toastrService.error(error);
            //   this.alertifyService.error(error);
          },
          complete: () => {
            localStorage.removeItem('returnData');
            this.clearCart();
            this.toastrService.success('Checkout Successfully.');
            this.router.navigate(['../../pos/returnreceipt'], {
              queryParams: {
                orderId: this.saleHeader,
                customerId: this.customer.epfNo,
              },
            });
            //https://www.digitalocean.com/community/tutorials/angular-query-parameters
          },
        });
    }
  }

  @ViewChild('myinputqty', { static: false }) myInputFieldqty?: ElementRef;

  ngAfterViewInit(): void {
    timer(0).subscribe(() => this.myInputFieldqty?.nativeElement.focus());
  }
  reprint() {
    this.router.navigate(['../../pos/reprint'], {
      queryParams: {
        orderId: this.billNumber,
      },
    });
  }
  remove(id: string, dSalePrice: number) {
    const formData: any = new FormData();
    formData.append('dSalePrice', dSalePrice);
    formData.append(
      'branchLocationId',
      Number(this.commonService.getUserBranchLocationId())
    );
    formData.append('barCode', id);
    this.cartService
      .RemoveSaleItemDetails(formData)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: (data: OrderData) => {
          this.lines = data.lines;
          this.totalQuantity = data.totalQuantity;
          this.subtotal = data.computeTotal;
          this.discount = data.computeDiscountValue;
          this.total = data.computeGrandTotalValue;
          this.count = data.count;
          //  console.log(data);
          //  console.log('works');
        },
        error: (error) => {
          //        this.toastrService.error(error);
          this.toastrService.error(error);
        },
        complete: () => {
          this.toastrService.success('Item removed successfully.');
        },
      });
  }
}
