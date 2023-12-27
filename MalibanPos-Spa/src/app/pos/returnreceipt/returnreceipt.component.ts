import { CommonService } from 'src/app/_services/common.service';
import { CustomerService } from './../../_services/customer.service';
import { CartService } from 'src/app/_services/cart.service';
import { ReceiptDetails } from './../../models/receiptdetails';
import { AfterViewInit, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { ItemDetail } from 'src/app/models/itemdetail';
import { ToastrService } from 'ngx-toastr';
import { Customer } from 'src/app/models/customer';
declare const test1:any;

interface CustomerDetail {
  number: string;
  fullName: string;
  creditLimit: number;
  creditBalance: number;
  previousCreditBalance: number;
  epfNo: number;
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
  epfNo: number;
  itemDetails : ItemDetail[];
  billDate: Date;
  orderObj: number;
  number: string;
  }


@Component({
  templateUrl: './returnreceipt.component.html',
  styleUrls: ['./returnreceipt.component.css']
})
export class ReturnreceiptComponent implements OnInit,AfterViewInit {
  orderObj: {};
  order: number;
  receiptDetails: ReceiptDetails[];
  itemDetails : ItemDetail[];
  customerId: {};
  customer: CustomerDetail = {
    number: '',
    fullName: '',
    creditLimit: 0,
    creditBalance: 0,
    previousCreditBalance: 0,
    epfNo: 0,
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
    epfNo: 0,
  } ;
  total: number;
  profit: number;
  totalQty: number;
  cashierName: string;
  billDate = new Date(Date.now());
  length = 0;
  constructor(private router: Router,private route: ActivatedRoute, private cartService: CartService, private customerService: CustomerService,private commonService: CommonService,private toastrService: ToastrService
  ) { }

  ngOnInit() {
    this.route.queryParams
      .subscribe((queryParams: Params) => {
        this.orderObj = queryParams['orderId'];
        this.customerId = queryParams['customerId'];
        console.log('works orderObj');
        console.log(this.orderObj);
        console.log(this.customerId);
      });
    this.GetData();
    // this.total=this.receiptDetails.reduce((a,b) => (b.price.salePrice * b.quantity) + a,0);
    // console.log(this.total);
  }
  ngAfterViewInit(){
    setTimeout(() => this.router.navigate(['../../pos/order']), 1000);
   //   setTimeout(() => this.PrintPage(), 1000);
    }
  GetData() {
    this.cartService.GetReceiptDetails(this.orderObj).subscribe((data: any) => {
      console.log(data);
      this.receiptDetails = data.receiptDetails;
      this.length = data.receiptDetails.length;
      this.customer = data.customer;
      console.log(this.customer);
      console.log(this.receiptDetails);
      this.total = this.receiptDetails.reduce((a, b) => (b.price.salePrice * (b.quantity-b.returnQuantity)) + a, 0);
      this.profit = this.receiptDetails.reduce((a, b) => (b.price.maximumRetailPrice * (b.quantity-b.returnQuantity)) + a, 0);
      this.totalQty=this.receiptDetails.reduce((a,b) => a+(b.quantity-b.returnQuantity),0);
      this.cashierName =this.commonService.getUser();
      console.log(this.total);
      this.itemDetails=data.receiptDetails.map((item: ReceiptDetails) => {
        return { productName: item.product.productName,quantity: item.quantity,returnQuantity: item.returnQuantity,salePrice: item.price.salePrice };
    });
    console.log(this.itemDetails);
    this.printbill.itemDetails=this.itemDetails;
     this.printbill.billDate=new Date(Date.now());;
     this.printbill.orderObj=+this.orderObj;
      this.printbill.branchName='Eam Maliban Textiles';
      this.printbill.address='';
      this.printbill.email='';
      this.printbill.contactNumbers='';
      this.printbill.createDate=new Date(Date.now());
      this.printbill.createdBy=this.cashierName;
      this.printbill.profit=this.receiptDetails.reduce((a, b) => (b.price.maximumRetailPrice * (b.quantity - b.returnQuantity)) + a, 0)-this.receiptDetails.reduce((a, b) => (b.price.salePrice * (b.quantity - b.returnQuantity)) + a, 0);
      this.printbill.total=this.receiptDetails.reduce((a, b) => (b.price.salePrice * (b.quantity - b.returnQuantity)) + a, 0);
      this.printbill.length=this.length;
      this.printbill.totalQty=this.receiptDetails.reduce((a, b) => a + (b.quantity - b.returnQuantity), 0);;
      this.printbill.fullName =this.customer?.fullName;
      this.printbill.creditLimit= this.customer?.creditLimit;
      this.printbill.creditBalance= this.customer?.creditBalance;
      this.printbill.previousCreditBalance= this.customer?.previousCreditBalance;
      this.printbill.epfNo= this.customer?.epfNo;
      this.printbill.number=this.customer?.number;
      console.log(this.printbill);
      test1(this.printbill); }
      ,(error) =>{
        this.toastrService.error(error);
       },() => {
        this.toastrService.info('Receipt Printed')
       });
  }
  PrintPage() {
    this.router.navigate(['../../pos/order']);
    window.print();
    window.close();
    window.location.href = "https://192.168.1.72:5001/pos/order";
  }
}
