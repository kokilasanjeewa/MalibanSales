//#region import

import { PricemodalComponent } from "./../pricemodal/pricemodal.component";
import { TransactionDetail } from "./../../models/transactiondetail";
import { OrderData } from "./../../models/orderdata";
import { CommonService } from "./../../_services/common.service";
import { CustomerService } from "./../../_services/customer.service";
import { environment } from "src/environments/environment";
import { formatDate } from "@angular/common";
import { HttpClient } from "@angular/common/http";

import {
  Component,
  ElementRef,
  OnInit,
  ViewChild,
  HostListener,
  AfterViewInit,
  Renderer2,
  OnDestroy,
} from "@angular/core";
import { Router } from "@angular/router";
import { CartService } from "src/app/_services/cart.service";
import { AlertifyService } from "./../../_services/alertify.service";
import { ToastrService } from "ngx-toastr";
import { Observable, Subject, timer } from "rxjs";
import { BsModalRef, BsModalService } from "ngx-bootstrap/modal";
import * as $ from "jquery";
import { THIS_EXPR } from "@angular/compiler/src/output/output_ast";
import { MessagemodelComponent } from "../messagemodel/messagemodel.component";
import { CheckoutcashmodalComponent } from "../checkoutcashmodal/checkoutcashmodal.component";
import { ConnectionService } from "src/app/_services/connection.service";
import { takeUntil } from "rxjs/operators";
//#endregion import
//#region initial data
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
  epfNo: string;
  cashLimit?: number;
  cashBalance?: number;
}
//#endregion initial data

@Component({
  templateUrl: "./order.component.html",
  styleUrls: ["./order.component.css"],
})
export class OrderComponent implements OnInit, AfterViewInit, OnDestroy {
  //#region variable
  //for default ngx-bootstrap modal
  public modalRef: BsModalRef;
  selectedItem: string;
  paymentReferenceId: any;
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
  public priceVersion: String = "";
  baseUrl = environment.apiUrl + "Sale/";
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
    number: "",
    fullName: "",
    creditLimit: 0,
    creditBalance: 0,
    epfNo: "",
    cashLimit: 0,
    cashBalance: 0,
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
  paymentType: number = 1;
  public connected: boolean = true;
  private destroyed$ = new Subject();
  //#endregion variable

  constructor(
    private router: Router,
    private alertifyService: AlertifyService,
    private cartService: CartService,
    private http: HttpClient,
    private renderer: Renderer2,
    private customerService: CustomerService,
    private toastrService: ToastrService,
    private commonService: CommonService,
    private modalService: BsModalService,
    private connection: ConnectionService
  ) {
    this.connected = this.connection.connected;
    connection.Changes.subscribe((state) => (this.connected = state));
  }
  // https://stackoverflow.com/questions/37362488/how-can-i-listen-for-keypress-event-on-the-whole-page
  @HostListener("document:keyup", ["$event"])
  handleKeyboardEvent(event: KeyboardEvent) {
    // event.altKey && event.key === '.'
    event.stopPropagation();
    if (event.key === "+") {
      this.checkout();
      //  console.log('checkout');
      // remove something...
    }
  }
  openModalWithMessageComponent(message: string) {
    this.bsModalRef = this.modalService.show(MessagemodelComponent, {
      initialState: message,
      class: "gray modal-md modal-dialog-centered",
      backdrop: true,
      ignoreBackdropClick: false,
      animated: false,
    });
    this.bsModalRef.content.updateErrorAdd.subscribe((data) => {});
  }
  openModalWithCashComponent() {
    this.bsModalRef = this.modalService.show(CheckoutcashmodalComponent, {
      initialState: null,
      class: "gray modal-md modal-dialog-centered",
      backdrop: true,
      ignoreBackdropClick: false,
      animated: false,
    });
    this.bsModalRef.content.updateErrorAdd
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: (data) => {
          console.log(data.paymentReferenceId);
          this.paymentReferenceId = data.paymentReferenceId;
          this.checkoutCash();
        },
        error: () => {},
        complete: () => {},
      });
  }
  openModalWithComponent(list: TransactionDetail[]) {
    this.bsModalRef = this.modalService.show(PricemodalComponent, {
      initialState: list,
      class: "gray modal-lg modal-dialog-centered",
      backdrop: "static",
      animated: false,
    });
    this.bsModalRef.content.updateItemAdd.subscribe((data) => {
      // console.log(data);
      this.selectedItemIndex = data;
      this.itemChange();
    });
    //this.bsModalRef.content.closeBtnName = 'Close';
  }
  ngOnInit() {
    let branchLocationId = this.commonService.getUserBranchLocationId();
    //  this.selectedItem = null;
    var Data = localStorage.getItem("returnData");
    if (Data != null) {
      this.postData = JSON.parse(Data);
      this.customer = this.postData.returnCustomer;
      this.billNumber = this.postData.billNumber;
      this.isReturnReadonly = this.postData.isReadOnly;
    }

    // this.arrayitem = JSON.parse(localStorage.getItem('ListaPermessi'));
    this.toastrService.info("initital load Successfully.");
    //  this.alertifyService.success('initital load Successfully.');
    this.initCart();
    // this.ngAfterViewInit();
  }
  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
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
        },
        error: (error) => {
          this.toastrService.error(error);
        },
        complete: () => {
          this.selectedItem = null;
          this.toastrService.success("Item add successfully.");
        },
      });
  }
  // not functioning any more
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
        next: (data: OrderData) => {
          this.lines = data.lines;
          this.totalQuantity = data.totalQuantity;
          this.subtotal = data.computeTotal;
          this.returntotal = data.computeReturnTotal;
          this.discount = data.computeDiscountValue;
          this.total = data.computeGrandTotalValue;
          this.count = data.count;
          this.billActive = data.billActive;
          this.isReadonly = true;
          this.isReturnReadonly = data.billActive;
        },
        error: (error) => {
          this.toastrService.error(error);
        },
        complete: () => {
          this.toastrService.success("Bill successfully loading.");
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
          localStorage.setItem("returnData", JSON.stringify(this.postData));
        },
        error(err) {
          console.log(err);
        },
        complete() {},
      });
  }
  Customeradd(event: Event) {
    const Value = (event.target as HTMLInputElement).value;
    const empCode = Value.trim().toLowerCase();
    // this.myInputField1.nativeElement.focus();
    // this.customer=this.arrayitem.find(obj => obj.epfNo===empCode.toString());
    if (empCode.length > 0)
      this.customerService
        .GetCustomer(
          empCode,
          Number(this.commonService.getUserBranchLocationId())
        )
        .pipe(takeUntil(this.destroyed$))
        .subscribe({
          next: (data) => {
            this.customer = data;
            timer(0).subscribe(() => this.myInputField1?.nativeElement.focus());
            // this.myInputField1?.nativeElement.focus();
            //  console.log(data);
          },
          error: (error) => {
            // this.alertifyService.error(error);
            this.toastrService.error(error);
          },
          complete: () => {
            // this.alertifyService.success('Customer add successfully.');
            // this.myInputField1.nativeElement.focus();
            this.toastrService.success("Customer add successfully.");
          },
        });
  }
  onChangeEvent(
    event: any,
    barcode: string,
    dSalePrice: number,
    dCostPrice: number,
    transactionDetailId: number
  ) {
    var qty = Number(event.target.value);
    if (qty >= 0) {
      const formData: any = new FormData();
      formData.append("paymentId", this.paymentType);
      formData.append("barCode", barcode);
      formData.append("quantity", qty);
      formData.append("dSalePrice", dSalePrice);
      formData.append("dCostPrice", dCostPrice);
      formData.append("transactionDetailId", transactionDetailId);
      formData.append(
        "billDate",
        formatDate(this.billDate, "yyyy-MM-dd", "en")
      );
      formData.append("number", this.customer.number);
      formData.append(
        "branchLocationId",
        Number(this.commonService.getUserBranchLocationId())
      );
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
          },
          error: (error) => {
            if (error.message === "Your credit limit is over.") {
              this.openModalWithMessageComponent(error.message);
            }
            this.toastrService.error(error.message);
            this.lines = error.cart.lines;
            this.totalQuantity = error.cart.totalQuantity;
            this.subtotal = error.cart.computeTotal;
            this.discount = error.cart.computeDiscountValue;
            this.total = error.cart.computeGrandTotalValue;
            this.count = error.cart.count;
          },
          complete: () => {
            timer(0).subscribe(() => this.myInputField1?.nativeElement.focus());
            this.toastrService.success("Item add successfully.");
            this.initCart();
          },
        });
      this.scanCode = "";
      this.myInputField1?.nativeElement.focus();
    }
  }
  onChangeFifoEvent(
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
      formData.append("paymentId", this.paymentType);
      formData.append("barCode", barcode);
      formData.append("quantity", qty);
      formData.append("dSalePrice", dSalePrice);
      formData.append("dCostPrice", dCostPrice);
      formData.append("transactionDetailId", transactionDetailId);
      formData.append(
        "billDate",
        formatDate(this.billDate, "yyyy-MM-dd", "en")
      );
      formData.append("number", this.customer.number);
      formData.append(
        "branchLocationId",
        Number(this.commonService.getUserBranchLocationId())
      );
      this.cartService
        .FifoCreateSaleItemQuantity(formData)
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
            if (error.message === "Your credit limit is over.") {
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
            //   this.alertifyService.success('Item add successfully.');
            timer(0).subscribe(() => this.myInputField1?.nativeElement.focus());
            this.toastrService.success("Item add successfully.");
            this.initCart();
          },
        });
      this.scanCode = "";
      this.myInputField1?.nativeElement.focus();
    }
  }
  Orderadd(event: Event) {
    const Value = (event.target as HTMLInputElement).value;
    const code = Value.trim().toLowerCase();
    const formData: any = new FormData();
    formData.append("barCode", code);
    formData.append("quantity", 1);
    formData.append("billDate", formatDate(this.billDate, "yyyy-MM-dd", "en"));
    formData.append(
      "branchLocationId",
      Number(this.commonService.getUserBranchLocationId())
    );
    formData.append("priceVersion", this.priceVersion);
    this.cartService
      .CreateSaleItemDetails(formData)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: (data: OrderData) => {
          this.lines = data.lines;
          this.totalQuantity = data.totalQuantity;
          this.subtotal = data.computeTotal;
          this.discount = data.computeDiscountValue;
          this.total = data.computeGrandTotalValue;
          this.count = data.count;
        },
        error: (error) => {
          this.toastrService.error(error.message);
          this.lines = error.cart.lines;
          this.totalQuantity = error.cart.totalQuantity;
          this.subtotal = error.cart.computeTotal;
          this.discount = error.cart.computeDiscountValue;
          this.total = error.cart.computeGrandTotalValue;
          this.count = error.cart.count;
        },
        complete: () => {
          this.toastrService.success("Item add successfully.");
          this.initCart();
        },
      });
    this.scanCode = "";
    this.priceVersion = "";
  }
  clearCart() {
    return this.http
      .get(this.baseUrl + "clear")
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: (response) => {
          this.selectedItem = null;
          this.total = 0.0;
        },
        error: (error) => {
          this.toastrService.error(JSON.stringify(error));
        },
        complete: () => {
          this.lines = null;
          this.lines = [];
          this.total = 0.0;
          this.count = 0;
          this.discount = 0.0;
          this.subtotal = 0.0;
          this.totalQuantity = 0;
          this.customer.number = "";
          this.customer.fullName = "";
          this.customer.creditLimit = 0.0;
          this.customer.creditBalance = 0.0;
          this.customer.epfNo = "";
          this.isReadonly = false;
          this.isReturnReadonly = false;
          this.customerNumber = "";
          this.selectedItem = null;
          this.billNumber = null;
          localStorage.removeItem("returnData");
          this.ngOnInit();
          this.toastrService.success("Clear Successfully.");
          //  this.alertifyService.success('Clear Successfully.');
          // this.router.navigate(['../../pos/order']);
        },
      });
  }
  // not functioning any more
  salereturn() {
    this.checkdisable = true;
    const formData: any = new FormData();
    formData.append("employeeCode", this.customer.number);
    formData.append(
      "branchLocationId",
      this.commonService.getUserBranchLocationId()
    );
    formData.append("saleHeaderId", this.billNumber);
    formData.append("createdBy", this.commonService.getUser());
    formData.append("id", this.commonService.getUserId());
    if (this.count == 0 && this.customer.epfNo == "") {
      this.toastrService.error("Please check with order details");
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
            this.customer.number = "";
            this.customer.fullName = "";
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
            localStorage.removeItem("returnData");
            this.clearCart();
            this.toastrService.success("Checkout Successfully.");
            this.router.navigate(["../../pos/returnreceipt"], {
              queryParams: {
                orderId: this.saleHeader,
                customerId: this.customer.epfNo,
              },
            });
            //  this.router.navigate(['/products'], { queryParams: { order: 'popular', 'price-range': 'not-cheap' } });
            //http://localhost:4200/products?order=popular&price-range=not-cheap
            //https://www.digitalocean.com/community/tutorials/angular-query-parameters
          },
        });
    }
  }

  checkout() {
    this.countClick++;
    //  console.log('CLICK COUNT '+this.countClick);
    this.checkdisable = true;
    const formData: any = new FormData();
    formData.append("employeeCode", this.customer.number);
    formData.append(
      "branchLocationId",
      this.commonService.getUserBranchLocationId()
    );
    formData.append("createdBy", this.commonService.getUser());
    formData.append("id", this.commonService.getUserId());
    formData.append("paymentId", 1);
    if (this.count == 0 && this.customer.epfNo == "") {
      this.toastrService.error("Please check with order details");
    } else {
      this.isReadonly = true;
      if (this.countClick == 1) {
        this.cartService
          .CreateSaleHeader(formData)
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
              this.customer.number = "";
              this.customer.fullName = "";
              this.customer.creditLimit = 0.0;
              this.customer.creditBalance = 0.0;
              this.customer.cashLimit = 0.0;
              this.customer.cashBalance = 0.0;
              this.customerNumber = "";
             },
            error: (error) => {
              this.toastrService.error(error);
              console.log(error);
            },
            complete: () => {
              this.clearCart();
              this.toastrService.success("Checkout Successfully.");
              this.initCart();
              //  this.router.navigate(['../../pos/order']);
              // disable to preview print receipt
              this.router.navigate(["../../pos/recept"], {
                queryParams: {
                  orderId: this.saleHeader,
                  customerId: this.customer.epfNo,
                },
              });
            },
          });
      }
      if (this.countClick > 1) {
        //  alert('DUPLICATE');
        this.alertifyService.confirm(
          "Duplicate Checkout.",
          "Are you sure you want to cancel?",
          () => {
            this.countClick = 0;
            this.clearCart();
          },
          () => {}
        );
      }
    }
  }
  checkoutCash() {
    this.countClick++;
    this.checkdisable = true;
    const formData: any = new FormData();
    formData.append("employeeCode", this.customer.number);
    formData.append(
      "branchLocationId",
      this.commonService.getUserBranchLocationId()
    );
    formData.append("createdBy", this.commonService.getUser());
    formData.append("id", this.commonService.getUserId());
    formData.append("paymentId", 2);
    formData.append("paymentReferenceId", this.paymentReferenceId);

    if (this.count == 0 && this.customer.epfNo == "") {
      this.toastrService.error("Please check with order details");
    } else {
      this.isReadonly = true;
      if (this.countClick == 1) {
        this.cartService
          .CreateSaleHeader(formData)
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
              this.customer.number = "";
              this.customer.fullName = "";
              this.customer.creditLimit = 0.0;
              this.customer.creditBalance = 0.0;
              this.customer.cashLimit = 0.0;
              this.customer.cashBalance = 0.0;
              this.customerNumber = "";
            },
            error: (error) => {
              this.toastrService.error(error);
              console.log(error);
            },
            complete: () => {
              this.clearCart();
              this.toastrService.success("Checkout Successfully.");
              this.initCart();
              //  this.router.navigate(['../../pos/order']);
              // disable to preview print receipt
              this.router.navigate(["../../pos/recept"], {
                queryParams: {
                  orderId: this.saleHeader,
                  customerId: this.customer.epfNo,
                },
              });
            },
          });
      }
      if (this.countClick > 1) {
        //  alert('DUPLICATE');
        this.alertifyService.confirm(
          "Duplicate Checkout.",
          "Are you sure you want to cancel?",
          () => {
            this.countClick = 0;
            this.clearCart();
          },
          () => {}
        );
      }
    }
  }
  checkoutwp() {
    const formData: any = new FormData();
    formData.append("employeeCode", this.customer.number);
    formData.append(
      "branchLocationId",
      this.commonService.getUserBranchLocationId()
    );
    formData.append("createdBy", this.commonService.getUser());
    formData.append("id", this.commonService.getUserId());
    if (this.count == 0 && this.customer.epfNo == "") {
      this.toastrService.error("Please check with order details");
    } else {
      this.isReadonly = true;
      this.cartService
        .CreateSaleHeader(formData)
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
            this.customer.number = "";
            this.customer.fullName = "";
            this.customer.creditLimit = 0.0;
            this.customer.creditBalance = 0.0;
            this.customerNumber = "";
            this.selectedItem = null;
          },
          error: (error) => {
            this.toastrService.error(error);
          },
          complete: () => {
            this.clearCart();
            this.customerNumber = "";
            this.selectedItem = null;
            this.customer.creditLimit = 0.0;
            this.total = 0.0;
            this.toastrService.success("Checkout Successfully.");
            this.router.navigate(["../../pos/order"]);
            this.ngAfterViewInit();
            this.initCart();
          },
        });
    }
  }
  @ViewChild("myinput", { static: false }) myInputField?: ElementRef;
  @ViewChild("myinput1", { static: false }) myInputField1?: ElementRef;
  @ViewChild("myinputqty", { static: false }) myInputFieldqty?: ElementRef;
  @ViewChild("myinput2", { static: false }) myInputFieldqty1?: ElementRef;

  ngAfterViewInit(): void {
    timer(0).subscribe(() => this.myInputField?.nativeElement.focus());
  }
  // not functioning any more
  reprint() {
    this.router.navigate(["../../pos/reprint"], {
      queryParams: {
        orderId: this.billNumber,
      },
    });
  }
  // not functioning any more
  add(id: any) {
    const formData: any = new FormData();
    formData.append("barCode", id);
    formData.append("quantity", 1);
    formData.append("billDate", formatDate(this.billDate, "yyyy-MM-dd", "en"));
    formData.append("number", this.customer.number);
    formData.append(
      "branchLocationId",
      Number(this.commonService.getUserBranchLocationId())
    );
    this.cartService
      .CreateSaleItemDetails(formData)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: (data: OrderData) => {
          this.lines = data.lines;
          this.totalQuantity = data.totalQuantity;
          this.subtotal = data.computeTotal;
          this.discount = data.computeDiscountValue;
          this.total = data.computeGrandTotalValue;
          this.count = data.count;
        },
        error: (error) => {
          this.toastrService.error(error.message);
          this.lines = error.cart.lines;
          this.totalQuantity = error.cart.totalQuantity;
          this.subtotal = error.cart.computeTotal;
          this.discount = error.cart.computeDiscountValue;
          this.total = error.cart.computeGrandTotalValue;
          this.count = error.cart.count;
        },
        complete: () => {
          this.toastrService.success("Item add successfully.");
        },
      });
  }
  // not functioning any more
  removeadd(id: any) {
    const formData: any = new FormData();
    formData.append("barCode", id);
    formData.append("quantity", 1);
    formData.append(
      "branchLocationId",
      Number(this.commonService.getUserBranchLocationId())
    );
    this.cartService
      .RemoveSaleItemQuantity(formData)
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: (data: OrderData) => {
          this.lines = data.lines;
          this.totalQuantity = data.totalQuantity;
          this.subtotal = data.computeTotal;
          this.discount = data.computeDiscountValue;
          this.total = data.computeGrandTotalValue;
          this.count = data.count;
        },
        error: (error) => {
          this.toastrService.error(error);
        },
        complete: () => {
          this.toastrService.success("Item add successfully.");
        },
      });
  }
  remove(id: string, dSalePrice: number) {
    const formData: any = new FormData();
    formData.append("dSalePrice", dSalePrice);
    formData.append(
      "branchLocationId",
      Number(this.commonService.getUserBranchLocationId())
    );
    formData.append("barCode", id);
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
        },
        error: (error) => {
          this.toastrService.error(error);
        },
        complete: () => {
          this.toastrService.success("Item removed successfully.");
        },
      });
  }
  // not functioning any more
  itemChange() {
    const formData1: any = new FormData();
    formData1.append("itemcount", this.items$?.length);
    formData1.append("barCode", this.items$[this.selectedItemIndex]?.barcode);
    formData1.append("index", this.selectedItemIndex);
    formData1.append("quantity", 1);
    formData1.append("billDate", formatDate(this.billDate, "yyyy-MM-dd", "en"));
    formData1.append("number", this.customer.number);
    formData1.append(
      "branchLocationId",
      Number(this.commonService.getUserBranchLocationId())
    );
    if (this.selectedItemIndex != null) {
      this.cartService
        .CreateSaleItemDetails(formData1)
        .pipe(takeUntil(this.destroyed$))
        .subscribe({
          next: (data: OrderData) => {
            this.lines = data.lines;
            this.totalQuantity = data.totalQuantity;
            this.subtotal = data.computeTotal;
            this.discount = data.computeDiscountValue;
            this.total = data.computeGrandTotalValue;
            this.count = data.count;
            this.billActive = data.billActive;
          },
          error: (error) => {
            if (error.message === "Your credit limit is over.") {
              this.openModalWithMessageComponent(error.message);
            }
            this.toastrService.error(error.message);
            this.lines = error.cart.lines;
            this.totalQuantity = error.cart.totalQuantity;
            this.subtotal = error.cart.computeTotal;
            this.discount = error.cart.computeDiscountValue;
            this.total = error.cart.computeGrandTotalValue;
            this.count = error.cart.count;
          },
          complete: () => {
            this.selectedItem = null;
            this.items$ = null;
            this.selectedItemIndex = null;
            timer(0).subscribe(() =>
              this.myInputFieldqty?.nativeElement.focus()
            );
            this.myInputFieldqty?.nativeElement.focus();
            this.toastrService.success("Item add successfully.");
          },
        });
    }
    this.scanCode = "";
    this.priceVersion = "";
  }
  changeBarcode(event: Event) {
    const Value = (event.target as HTMLInputElement).value;
    const code = Value.trim();
    this.selectedItem = code;
    this.commonService
      .getListByLocation(
        this.commonService.getUserBranchLocationId(),
        "Common",
        "getallstockdetail",
        code
      )
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: (res) => {
          this.itemQuantity = 1;
          timer(0).subscribe(() =>
            this.myInputFieldqty1?.nativeElement.focus()
          );
          this.myInputFieldqty1?.nativeElement.focus();
        },
        error: (error) => {
          this.toastrService.error(error);
        },
        complete: () => {
          this.toastrService.success("Item found successfully.");
        },
      });
  }
  itemChange1(event: Event) {
    const Value = (event.target as HTMLInputElement).value;
    const itemQuantity = Value.trim();
    this.itemQuantity = Number(itemQuantity);
    if (
      this.selectedItem !== undefined &&
      this.selectedItem !== null &&
      this.itemQuantity !== null
    ) {
      const formData: any = new FormData();
      formData.append("paymentId", this.paymentType);
      formData.append("barCode", this.selectedItem);
      formData.append("itemcount", 0);
      formData.append("number", this.customer.number);
      formData.append("quantity", this.itemQuantity);
      formData.append(
        "billDate",
        formatDate(this.billDate, "yyyy-MM-dd", "en")
      );
      formData.append(
        "branchLocationId",
        Number(this.commonService.getUserBranchLocationId())
      );

      this.cartService
        .CreateSaleItemDetails(formData)
        .pipe(takeUntil(this.destroyed$))
        .subscribe({
         next: (data: OrderData) => {
            this.lines = data.lines;
            this.totalQuantity = data.totalQuantity;
            this.subtotal = data.computeTotal;
            this.discount = data.computeDiscountValue;
            this.total = data.computeGrandTotalValue;
            this.count = data.count;
            this.billActive = data.billActive;
          },
         error: (error) => {
            if (error.message === "Your credit limit is over.") {
              this.openModalWithMessageComponent(error.message);
            }
            this.toastrService.error(error.message);
            this.lines = error.cart.lines;
            this.totalQuantity = error.cart.totalQuantity;
            this.subtotal = error.cart.computeTotal;
            this.discount = error.cart.computeDiscountValue;
            this.total = error.cart.computeGrandTotalValue;
            this.count = error.cart.count;
          },
         complete: () => {
            this.selectedItem = null;
            timer(0).subscribe(() => this.myInputField1?.nativeElement.focus());
            this.myInputField1?.nativeElement.focus();
            this.toastrService.success("Item add successfully.");
          }
    });

      this.selectedItem = null;
      this.itemQuantity = null;
      this.selectedItemIndex = null;
      this.items$ = null;
      this.scanCode = "";
      this.priceVersion = "";
    }
  }
  changePayment() {
    console.log(this.paymentType);
  }
}
