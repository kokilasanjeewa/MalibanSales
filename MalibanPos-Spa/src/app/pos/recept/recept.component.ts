import { Customer } from './../../models/customer';
import { CommonService } from 'src/app/_services/common.service';
import { ReceiptDetails } from './../../models/receiptdetails';
import { CartService } from 'src/app/_services/cart.service';
import {  AfterViewInit, Component, DoCheck, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import * as $ from 'jquery';
import { CustomerService } from './../../_services/customer.service';
import { PrintService, UsbDriver, WebPrintDriver } from 'ng-thermal-print';
import { PrintDriver } from 'ng-thermal-print/lib/drivers/PrintDriver';
import { ReceiptPrint } from 'src/app/models/receiptprint';
import { ItemDetail } from 'src/app/models/itemdetail';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { ToastrService } from 'ngx-toastr';
import { Console } from 'console';
import { BranchlocationService } from 'src/app/_services/branchlocation.service';
import { BranchLocation } from 'src/app/models/branchlocation';
//import { test1 } from '../../../assets/resource/js/demo';
declare const test1:any;
interface CustomerDetail {
   number: string;
  fullName: string;
  creditLimit: number;
  creditBalance: number;
  previousCreditBalance: number;
  epfNo: Number;
  paymentId: number;
  paymentReferenceId: string;
  cashLimit: number;
  cashBalance: number;
  previousCashBalance: number;
}

interface PrintBill {
branchName: string;
address: string;
email: string;
contactNumbers: string;
createDate:Date;
createdBy: string;
profit: number;
total: number;
length: number;
totalQty: number;
fullName: string;
creditLimit: number;
creditBalance: number;
previousCreditBalance: number;
cashLimit: number;
cashBalance: number;
previousCashBalance: number;
epfNo: Number;
itemDetails : ItemDetail[];
billDate: Date;
orderObj: number;
number: string;
paymentId:number;
paymentReferenceId: string;
}

@Component({
  selector: 'app-recept',
  templateUrl: './recept.component.html',
  styleUrls: ['./recept.component.css']
})
export class ReceptComponent implements OnInit,AfterViewInit {
  orderObj: {};
  order: number;
  receiptDetails: ReceiptDetails[];
  itemDetails : ItemDetail[];
  customerId: {};
  receiptPrint: ReceiptPrint;
  customer: CustomerDetail = {
    number: '',
    fullName: '',
    creditLimit: 0,
    creditBalance: 0,
    previousCreditBalance: 0,
    epfNo: 0,
    paymentId:0,
    paymentReferenceId: '',
    cashLimit: 0,
    cashBalance: 0,
    previousCashBalance: 0,
  };
  printbill: PrintBill = {
    billDate: new Date(Date.now()),
    orderObj: 0,
    number: '',
    itemDetails : [],
    branchName: '',
    address: '',
    email: '',
    contactNumbers: '',
    createDate: new Date(Date.now()),
    createdBy: '',
    profit: 0,
    total: 0,
    length: 0,
    totalQty: 0,
    fullName: '',
    creditLimit: 0,
    creditBalance: 0,
    previousCreditBalance: 0,
    cashLimit: 0,
    cashBalance: 0,
    previousCashBalance: 0,
    epfNo: 0,
    paymentId:0,
    paymentReferenceId: '',
  } ;
  total: number;
  profit: number;
  totalQty: number;
  cashierName: string;
  billDate = new Date(Date.now());
  length = 0;
  //thermal printer setting
  status: boolean = false;
  received = [];
  sent : ReceiptDetails[];
  branch: BranchLocation;
  constructor(private router: Router,
    private route: ActivatedRoute,
    private cartService: CartService,
    private customerService: CustomerService,
    private commonService: CommonService,
    private printService: PrintService,
    private toastrService: ToastrService,
    private branchService: BranchlocationService,
  ) {

  }
  ngAfterViewInit(){
    setTimeout(() => this.router.navigate(['../../pos/order']), 1000);
 //   setTimeout(() => this.PrintPage(), 1000);
  }
   autoPrint(){
    window.print();
   this.router.navigate(['../../pos/order']);
  }
  ngOnInit() {
    this.route.queryParams
      .subscribe((queryParams: Params) => {
        this.orderObj = queryParams['orderId'];
        this.customerId = queryParams['customerId'];
        console.log('works orderObj');
        console.log(this.orderObj);
        console.log('printtest')
        console.log(JSON.stringify(this.customerId));
      },(error) => {
        console.log(error);
      }
      ,() => {
      });
      this.branchService.getBranchLocatio(this.commonService.getUserBranchLocationId())
      .subscribe((data:BranchLocation) => {
        console.log('sale header details branch data');
        this.branch=data;
        console.log(data);
      });
      this.GetData();
    //this.PrintPage();

  }
  GetData() {
     this.cartService.GetReceiptDetails(this.orderObj).subscribe((data: any) => {
      console.log('sale header details data');
      console.log(data);
      this.receiptDetails = data.receiptDetails;
      this.length = data.length;
      this.customer = data.customer;
      console.log(data.customer)
      console.log(this.receiptDetails);
      this.total = this.receiptDetails.reduce((a, b) => (b.price.salePrice * (b.quantity - b.returnQuantity)) + a, 0);
      this.profit = this.receiptDetails.reduce((a, b) => (b.price.maximumRetailPrice * (b.quantity - b.returnQuantity)) + a, 0);
      this.totalQty = this.receiptDetails.reduce((a, b) => a + (b.quantity - b.returnQuantity), 0);
      this.cashierName = this.commonService.getUser();
      console.log(this.total);
     // this.receiptPrint.receiptDetails=this.receiptDetails;
     this.itemDetails=data.receiptDetails.map((item: ReceiptDetails) => {
      return { productName: item.product.productName,quantity: item.quantity,returnQuantity: item.returnQuantity,salePrice: item.price.salePrice };
  });
      console.log(this.itemDetails);
      this.printbill.itemDetails=this.itemDetails;
       this.printbill.billDate=new Date(Date.now());;
       this.printbill.orderObj=+this.orderObj;
        this.printbill.branchName=this.branch?.branchName;
        this.printbill.address=this.branch?.address;
        this.printbill.email='';
        this.printbill.contactNumbers=this.branch?.contactNumbers;
        this.printbill.createDate=new Date(Date.now());
        this.printbill.createdBy=this.cashierName;
        this.printbill.profit=this.receiptDetails.reduce((a, b) => (b.price.maximumRetailPrice * (b.quantity - b.returnQuantity)) + a, 0)-this.receiptDetails.reduce((a, b) => (b.price.salePrice * (b.quantity - b.returnQuantity)) + a, 0);
        this.printbill.total=this.receiptDetails.reduce((a, b) => (b.price.salePrice * (b.quantity - b.returnQuantity)) + a, 0);
        this.printbill.length=this.length;
        this.printbill.totalQty=this.receiptDetails.reduce((a, b) => a + (b.quantity - b.returnQuantity), 0);;
        this.printbill.fullName =this.customer.fullName;
        this.printbill.creditLimit= this.customer.creditLimit;
        this.printbill.creditBalance= this.customer.creditBalance;
        this.printbill.previousCreditBalance= this.customer.previousCreditBalance;
        this.printbill.cashLimit= this.customer.cashLimit;
        this.printbill.cashBalance= this.customer.cashBalance;
        this.printbill.previousCashBalance= this.customer.previousCashBalance;
        this.printbill.epfNo=this.customer.epfNo;
       // this.printbill.epfNo= this.customer.epfNo;
        this.printbill.number=this.customer.number;
        this.printbill.paymentId=this.customer.paymentId;
        this.printbill.paymentReferenceId=this.customer.paymentReferenceId;
        console.log('last edge');
        console.log(this.printbill);
        test1(this.printbill);
       },(error) =>{
        this.toastrService.error(error);
       },() => {
        this.toastrService.info('Receipt Printed')
       });

  }
  PrintPage() {
      this.cartService.GetReceiptDetails(this.orderObj).subscribe((data: ReceiptDetails[]) => {
      console.log(data);
      this.receiptDetails = data;
      this.length = data.length;
      console.log(this.receiptDetails);
      this.total = this.receiptDetails.reduce((a, b) => (b.price.salePrice * (b.quantity - b.returnQuantity)) + a, 0);
      this.profit = this.receiptDetails.reduce((a, b) => (b.price.maximumRetailPrice * (b.quantity - b.returnQuantity)) + a, 0);
      this.totalQty = this.receiptDetails.reduce((a, b) => a + (b.quantity - b.returnQuantity), 0);
      this.cashierName = this.commonService.getUser();
      console.log(this.total);
    });
    window.print();
    window.close();
    window.location.href = "../pos/order";
  //this.router.navigate(['../../pos/order']);
  }
}
