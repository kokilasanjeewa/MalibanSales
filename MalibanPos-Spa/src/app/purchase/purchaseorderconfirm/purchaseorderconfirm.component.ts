import { Data } from '../../models/data';
import { catchError } from 'rxjs/operators';
import { PriceService } from '../../_services/price.service';
import { SupplierPrice } from '../../models/supplierprice';
import { PurchaseorderService } from '../../_services/purchaseorder.service';
import { ProductService } from '../../_services/product.service';
import { CommonService } from 'src/app/_services/common.service';
import { formatDate } from '@angular/common';
import { Component, HostListener, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { SupplierproductService } from 'src/app/_services/supplierproduct.service';
import { BranchLocation } from 'src/app/models/branchlocation';
import { PurchaseOrderDetail } from 'src/app/models/purchaseorderdetail';
import { PurchaseOrderHeader } from 'src/app/models/purchaseorderheader';
interface Post {
  startDate: Date;
  endDate: Date;
}
@Component({
  selector: 'app-purchaseorderconfirm',
  templateUrl: './purchaseorderconfirm.component.html',
  styleUrls: ['./purchaseorderconfirm.component.css']
})
export class PurchaseorderconfirmComponent implements OnInit {
  selectedItem: number;
  selectedProductcode: string;
  post: Post = {
    startDate: new Date(Date.now()),
    endDate: new Date(Date.now()),
  };
  addPOForm: FormGroup;
  addorderListForm: FormGroup;
  products: Observable<SupplierPrice[]>;
  supplierPrices: Observable<SupplierPrice[]>;
  branches: Observable<BranchLocation[]>;
  payments: Observable<[]>;
  suppliers: Observable<[]>;
  indents: Observable<[]>;
  productId: any;
  lines: {};
  requestlist: Observable<[]>;
  poItemList: Array<any> = []
  subtotal: number;
  discount: number;
  total: number;
  show: boolean = true;
  count: number;
  orderHeader: number;
  ponumber: number;
  po: PurchaseOrderHeader;
  cApprovalTypes: {};
  // submitted = false;
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private alertifyService: AlertifyService,
    private commonService: CommonService,
    private productService: ProductService,
    private purchaseorderService: PurchaseorderService,
    private priceService: PriceService
  ) {
    // this.products = this.commonService.getList('Common', 'getallproduct');
    this.branches = this.commonService.getListByLocation(
      this.commonService.getUserBranchLocationId(),
      'Common',
      'getbranch'
    );
    this.suppliers = this.commonService.getListByLocation(
      this.commonService.getUserBranchLocationId(),
      'Common',
      'getallsupplier'
    );
    this.payments = this.commonService.getList('Common', 'getallpaymentmethod');
    this.requestlist = this.commonService.getListByLocation(
      this.commonService.getUserBranchLocationId(),
      'Common',
      'getallpurchaserequest'
    );
    this.cApprovalTypes=[{id:1,name:'Approve'},{id:2,name:'Reject'}]
  }
  ngOnInit() {
    const id = this.route.snapshot.params['purchaseOrderHeaderId'];
    this.ponumber =id;
      this.purchaseorderService.GetPurchaseOrderHeder(true,id).subscribe((data: PurchaseOrderHeader) => {
        this.po = data;
        console.log('zaky');
        console.log(data);
        this.addPOForm.patchValue({
        branchLocationId: data.branchLocationId,
        supplierId: data.supplierId,
        paymentMethodId: data.paymentMethodId,
        poDate: formatDate(data.poDate, 'yyyy-MM-dd', 'en'),
        deliveryDate:formatDate(data.deliveryDate, 'yyyy-MM-dd', 'en'),
        description:data.description,
      });
      this.products = this.priceService.getProductPriceBySupplierId(
        data.supplierId,
        data.branchLocationId
      );
      const isEmpty = Object.keys(this.lines).length === 0;

      if(isEmpty && isEmpty !== undefined){
      data?.purchaseOrderDetails.forEach(item => {
        this.poItemList.push({ barCode: item.product.barcode, createdBy: data.createdBy, quantity: item.quantity,approveQuantity:item.approveQuantity,cApprovalType:item.cApprovalType,supplierId:data.supplierId ,supplierPriceId: item.supplierPriceId,branchLocationId:data.branchLocationId,productExpiryDate: item.productExpiryDate })
      });
    }
      this.purchaseorderService
      .CreatepurchaseOrderitemdetaillist(this.poItemList)
      .subscribe((data1: Data) => {
        this.lines = data1.lines;
        console.log('purchase order list');
        console.log(data1);
        //  this.count = data.count;
        //  this.totalQuantity = data.totalQuantity;
        this.subtotal = data1.computeTotalValue;
        this.discount = data1.computeDiscountValue;
        this.total = data1.computeGrandTotalValue;
        //  this.count = data.count;
      });
    });
    console.log(this.poItemList);
    this.createSupplierRegisterForm();
    this.initPurchaseOrder();
    console.log(this.commonService.getUser());
    console.log(this.count);
  }
  initPurchaseOrder() {
    this.purchaseorderService.GetPurchaseOrder().subscribe((data: Data) => {
      this.lines = data.lines;
      console.log(data);
      //  this.count = data.count;
      //  this.totalQuantity = data.totalQuantity;
      this.subtotal = data.computeTotalValue;
      this.discount = data.computeDiscountValue;
      this.total = data.computeGrandTotalValue;
      //  this.count = data.count;
    });
  }
  get f() {
    return this.addPOForm.controls;
  }
  get f1() {
    return this.addorderListForm.controls;
  }
  createSupplierRegisterForm() {
    const now = new Date();
    console.log(now.toISOString());
    this.addPOForm = this.fb.group({
      branchLocationId: ['', [Validators.required]],
      supplierId: ['', [Validators.required]],
      paymentMethodId: ['', [Validators.required]],
      indentId: [''],
      // https://stackblitz.com/edit/angular-kucypd
      poDate: [
        formatDate(this.post.startDate, 'yyyy-MM-dd', 'en'),
        [Validators.required],
      ],
      deliveryDate: ['', [Validators.required]],
      description: [''],
      paymentId: [0],
    });
    this.addorderListForm = this.fb.group({
      productName: ['', [Validators.required]],
      quantity: ['', [Validators.required]],
      barCode: ['', [Validators.required]],
      supplierPriceId: ['', [Validators.required]],
      productExpiryDate:[, [Validators.required]],
    });
  }
  // Choose city using select dropdown
  changeProvince(event: Event) {
    console.log((event.target as HTMLInputElement).value);
    this.addPOForm.controls.provinceId.setValue(
      (event.target as HTMLInputElement).value,
      {
        onlySelf: true,
      }
    );
  }
  poAdd() {
    const formData: any = new FormData();
    formData.append(
      'branchLocationId',
      this.addPOForm.get('branchLocationId').value
    );
    formData.append('supplierId', this.addPOForm.get('supplierId').value);
    formData.append(
      'paymentMethodId',
      this.addPOForm.get('paymentMethodId').value
    );
    formData.append('paymentId', 0);
    formData.append('poDate', this.addPOForm.get('poDate').value);
    formData.append('description', this.addPOForm.get('description').value);
    formData.append('deliveryDate', this.addPOForm.get('deliveryDate').value);
    formData.append('createdBy', this.commonService.getUser());
    formData.append('purchaseOrderHeaderId', this.ponumber);
    console.log(formData);
    if (!this.addPOForm.valid) {
      return false;
    } else {
      //alert(JSON.stringify(this.addPOForm.value));
      this.purchaseorderService.ConfirmPurchaseOrderHeader(formData).subscribe(
        (res: any) => {
          console.log(JSON.stringify(res));
          this.orderHeader = res.purchaseOrderId;
          this.alertifyService.success(
            JSON.stringify(res)
              .replace('"message":"', '')
              .replace('"', '')
              .replace('{', '')
              .replace('}', '')
          );
        },
        (error) => {
          this.alertifyService.error(
            JSON.stringify(error).replace('"', '').replace('"', '')
          );
          this.router.navigate(['../purchase/purchaseorderconfirm',this.ponumber]);
          console.log(error.error);
        },
        () => {
          //  this.alertifyService.success('Saved Successfully.');
          // this.router.navigate(['../purchase/purchaseorderlist'], { queryParams: { orderId: this.saleHeader, customerId: this.customer.epfNo } });
          // this.router.navigate(['../purchase/purchaseorderreport',this.orderHeader], { queryParams: { purchaseOrderId: this.orderHeader } });
          this.router.navigate(['../purchase/purchaseorderlist']);
          // this.router.navigate(['../purchase/purchaseorderlist']);
        }
      );
    }
  }
  itemListAdd() {
    const formData: any = new FormData();
    formData.append('barCode', this.addorderListForm.get('barCode').value);
    formData.append('quantity', this.addorderListForm.get('quantity').value);
    formData.append(
      'branchLocationId',
      this.addPOForm.get('branchLocationId').value
    );
    formData.append(
      'supplierPriceId',
      this.addorderListForm.get('supplierPriceId').value
    );
    formData.append(
      'productExpiryDate',
      this.addorderListForm.get('productExpiryDate').value
    );
    formData.append('supplierId', this.addPOForm.get('supplierId').value);
    formData.append('createdBy', this.commonService.getUser());
    console.log(formData);
    if (!this.addorderListForm.valid) {
      return false;
    } else {
      // alert(JSON.stringify(this.addorderListForm.value));
      this.purchaseorderService
        .CreatepurchaseOrderitemdetails(formData)
        .subscribe((data: Data) => {
          this.lines = data.lines;
          console.log('purchase order list');
          console.log(data);
          //  this.count = data.count;
          //  this.totalQuantity = data.totalQuantity;
          this.subtotal = data.computeTotalValue;
          this.discount = data.computeDiscountValue;
          this.total = data.computeGrandTotalValue;
          //  this.count = data.count;
        });
    }
  }
  updateApprovel(    e: any,
    code: any,
    supplierPriceId: any,
    productExpiryDate: any) {
      console.log(e.target.value);
      const formData: any = new FormData();
      const approveStatus = e.target.value
      formData.append('barCode', code);
      formData.append('approveStatus', approveStatus);
      formData.append(
        'branchLocationId',
        this.addPOForm.get('branchLocationId').value
      );
      formData.append('supplierPriceId', supplierPriceId);
      formData.append('productExpiryDate', productExpiryDate);
      formData.append('supplierId', this.addPOForm.get('supplierId').value);
      formData.append('createdBy', this.commonService.getUser());
      this.purchaseorderService.UpdateApproveStatus(formData).subscribe(
        (data: Data) => {
          console.log('purchase order list');
          console.log(data);
        },
        (err: any) => {
          this.alertifyService.error(JSON.stringify(err));
          console.log(err);
        },
        () => {
          this.initPurchaseOrder();
          this.alertifyService.success('approved status successfully.');
        }
      );
    }
    updateQuantity(
      e: any,
      code: any,
      supplierPriceId: any,
      productExpiryDate: any
    ) {
      const formData: any = new FormData();
      const approveQuantity = e.target.value;
      formData.append('barCode', code);
      formData.append('approveQuantity', approveQuantity);
      formData.append(
        'branchLocationId',
        this.addPOForm.get('branchLocationId').value
      );
      formData.append('supplierPriceId', supplierPriceId);
      formData.append('productExpiryDate', productExpiryDate);
      formData.append('supplierId', this.addPOForm.get('supplierId').value);
      formData.append('createdBy', this.commonService.getUser());
      this.purchaseorderService.UpdateApproveQuantity(formData).subscribe(
        (data: Data) => {
          console.log('purchase order list');
          console.log(data);
        },
        (err: any) => {
          this.alertifyService.error(JSON.stringify(err));
          console.log(err);
        },
        () => {
          this.initPurchaseOrder();
          this.alertifyService.success('approved quantity successfully.');
        }
      );
    }

  onFileChange(event) {
    // this.selectedFile = <File>event.target.files[0];
    //  console.log(this.selectedFile);
  }

  add() {
    //  console.log(e.target.value);
    // this.productId = e.target.value;
    this.productId = this.selectedProductcode;
    let productId: number | undefined;
    this.productService.getProduct(this.selectedProductcode).subscribe(
      (data) => {
        this.addorderListForm.patchValue({
          productName: data.productName,
          barCode: data.barcode,
        });
        productId = data.productId;

        console.log(productId);
      },
      (error) => {},
      () => {
        this.supplierPrices =
          this.priceService.getSupplierPriceBySupplierProductId(
            this.selectedItem,
            this.addPOForm.get('branchLocationId').value,
            productId
          );
        /*  .subscribe((data: any) => {
          this.supplierPrices=data;
          console.log(data)
        }); */
      }
    );
  }
  changeSupplier() {
    const formData: any = new FormData();
    formData.append(
      'branchLocationId',
      this.addPOForm.get('branchLocationId').value
    );
    formData.append('supplierId', this.selectedItem);
    console.log(this.selectedItem);
    this.products = this.priceService.getProductPriceBySupplierId(
      this.selectedItem,
      this.addPOForm.get('branchLocationId').value
    );
    if (this.show) {
      this.addPOForm.patchValue({
        branchLocationId: this.addPOForm.get('branchLocationId').value,
        supplierId: this.addPOForm.get('supplierId').value,
      });
      this.show = false;
    }
    console.log(this.products);
  }
  remove(id: any, supplierPriceId: any,productExpiryDate: any) {
    const formData: any = new FormData();
    formData.append('barCode', id);
    formData.append('supplierPriceId', supplierPriceId);
    formData.append(
      'branchLocationId',
      this.addPOForm.get('branchLocationId').value
    );
    formData.append(
      'productExpiryDate',
      productExpiryDate
    );
    this.alertifyService.confirm(
      'Please Confirm.',
      'Are you sure, you want to delete this line?',
      () => {
        this.purchaseorderService
          .RemovePurchaseOrderItemDetails(formData)
          .subscribe((data: Data) => {
            this.lines = data.lines;
            console.log(data);
            //  this.count = data.count;
            //  this.totalQuantity = data.totalQuantity;
            this.subtotal = data.computeTotalValue;
            this.discount = data.computeDiscountValue;
            this.total = data.computeGrandTotalValue;
            //  this.count = data.count;
          });
        this.alertifyService.success('Deleted successfully.');
      },
      () => {
        this.initPurchaseOrder();
      }
    );
  }
}
