import { CreateGrnItemDetailDto } from './../../models/creategrnitemdetaildto';
import { filter } from 'rxjs/operators';
import { PurchaseOrderHeader } from './../../models/purchaseorderheader';
import { DataEdit } from './../../models/dataedit';
import { PurchaseOrderDetail } from './../../models/purchaseorderdetail';
import { GrnService } from './../../_services/grn.service';
import { PurchaseorderService } from './../../_services/purchaseorder.service';
import { SupplierService } from 'src/app/_services/supplier.service';
import { CommonService } from 'src/app/_services/common.service';
import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { formatDate } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
//import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { StockdetailComponent } from '../stockdetail/stockdetail.component';
import { BranchLocation } from 'src/app/models/branchlocation';
interface PostData {
  branchLocationId: number;
  purchaseOrderHeaderId: number;
  supplierId: number;
}
interface Post {
  startDate: Date;
  endDate: Date;
}
@Component({
  templateUrl: './editgrn.component.html',
  styleUrls: ['./editgrn.component.css']
})
export class EditgrnComponent implements OnInit {
  selectedItem: number;
//  selectedSupplier: number;
  selectedProduct: string;
  post: Post = {
    startDate: new Date(Date.now()),
    endDate: new Date(Date.now())
  };
  postData: PostData={
    branchLocationId:0,
    purchaseOrderHeaderId:0,
    supplierId:0
  }
  addGrnForm: FormGroup;
  addGrnListForm: FormGroup;
  products: PurchaseOrderDetail[];
  branches: Observable<BranchLocation[]>;
  suppliers: Observable<[]>;
  sections: Observable<[]>;
  purchaseorderList: Observable<[]>;
  lines: {};
  linesp: {};
  subtotal: number;
  discount: number;
  total: number;
  count: number;
  grnItemList: Array<any> = []
  baseUrl = environment.apiUrl + 'Grn/';
  theCheckbox = false;
  public checkdisable: boolean = false;
  bsModalRef: BsModalRef;
  goodReceptNoteHeaderId: number;
  // submitted = false;
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private toastrService: ToastrService,
    private commonService: CommonService,
    private purchaseorderService: PurchaseorderService,
    private supplierService: SupplierService,
    private grnService: GrnService,
    private modalService: BsModalService,
    //private http: HttpClient,
  ) {
      // this.getBranches();
    // this.branches = this.commonService.getList('Common', 'getallbranch');
    this.branches = this.commonService.getListByLocation(this.commonService.getUserBranchLocationId(), 'Common', 'getbranch');
   this.suppliers = this.commonService.getListByLocation(this.commonService.getUserBranchLocationId(), 'Common', 'getallsupplier');
    this.purchaseorderList = this.commonService.getListByLocation(this.commonService.getUserBranchLocationId(), 'Common', 'getallpo');
  }
 /*  changeBranch(){
    this.purchaseorderList = this.commonService.getListByLocation(this.addGrnForm.get('branchLocationId').value, 'Common', 'getallpo');
    this.suppliers = this.commonService.getListByLocation(this.addGrnForm.get('branchLocationId').value, 'Common', 'getallsupplier');

  } */
  ngOnInit() {
    this.goodReceptNoteHeaderId = +this.route.snapshot.params.receiptOrderId;
    this.createSupplierRegisterForm();
    const postData = localStorage.getItem('postData');
    if (postData!=null){
      this.postData = JSON.parse(postData);
      this.selectedItem=this.postData?.purchaseOrderHeaderId;
      this.addGrnForm.patchValue({
       branchLocationId: this.postData?.branchLocationId,
       purchaseOrderHeaderId: this.postData?.purchaseOrderHeaderId,
       supplierId: this.postData?.supplierId,
     });
    }
    this.initGoodReceiptNote();
  }
  initGoodReceiptNote() {
    this.grnService.GetGoodReceptNoteHeader(true,this.goodReceptNoteHeaderId).subscribe((data: any) => {
      console.log('check subtotal and discount')
      console.log(data);
      this.addGrnForm.patchValue({
        branchLocationId: data.branchLocationId,
        purchaseOrderHeaderId: data.purchaseOrderHeader.purchaseOrderHeaderId,
        supplierId: data.supplier.supplierId,
        invoiceId:data.supplierInvoiceId,
        invoiceDate:formatDate(data.supplierInvoiceDate, 'yyyy-MM-dd', 'en'),
        totalDiscountAmount:data.totalDiscountAmount,
      });
      this.lines = data?.lines;
      this.count = data?.count;
      this.subtotal = data?.totalBillAmount;
      this.total = data?.totalCost;
      this.discount = data?.totalDiscountAmount;

    });

  }
  clearCart() {
    localStorage.removeItem('postData');
    this.grnService.ClearGoodReceipt().subscribe((response: any) => {
      this.lines = response.lines;
      console.log(response)
    }, (error) => {
      //  this.alertifyService.error(JSON.stringify(error));
      this.toastrService.error(JSON.stringify(error));
      console.log(error);
    },
      () => {
        this.addGrnForm.patchValue({
          branchLocationId: '',
          purchaseOrderHeaderId: '',
          invoiceId:'',
          invoiceDate:'',
          supplierId:'',
          totalDiscountAmount:0,
        });
/*        this.lines = [];
 */        this.total = 0.00;
        this.count = 0;
        this.discount = 0.00;
        this.subtotal = 0.00;
        window.location.reload();
        this.toastrService.success('Clear Successfully.');
        //  this.alertifyService.success('Clear Successfully.');
        //this.router.navigate(['../../pos/']);
      }
    );
  }

  get f() {
    return this.addGrnForm.controls;
  }
  get fo() {
    return this.addGrnListForm.controls;
  }
  createSupplierRegisterForm() {
    const now = new Date();
    console.log(now.toISOString());
    this.addGrnForm = this.fb.group({
      purchaseOrderHeaderId: ['', [Validators.required]],
      invoiceId: ['', [Validators.required]],
      invoiceDate: ['', [Validators.required]],
      // https://stackblitz.com/edit/angular-kucypd
      grnDate: [formatDate(this.post.startDate, 'yyyy-MM-dd', 'en'), [Validators.required]],
      branchLocationId: ['', [Validators.required]],
      supplierId: ['', [Validators.required]],
      totalDiscountAmount: [0, [Validators.required]],
       /*     totalBillAmount: [0, [Validators.required]], */
    });
    this.addGrnListForm = this.fb.group({
      productId: [''],
      quantity: ['', [Validators.required]],
      retailPrice: ['', [Validators.required]],
      productName: ['', [Validators.required]],
      freequantity: [''],
      supplierPriceId: ['', [Validators.required]],
      productExpiryDate: ['', [Validators.required]],
      purchaseOrderHeaderId: ['', [Validators.required]],

    });
  }
  grnAdd() {
    this.checkdisable=true;
    const formData: any = new FormData();
    formData.append('purchaseOrderHeaderId', this.addGrnForm.get('purchaseOrderHeaderId').value);
    formData.append('supplierInvoiceId', this.addGrnForm.get('invoiceId').value);
    formData.append('supplierInvoiceDate', this.addGrnForm.get('invoiceDate').value);
    formData.append('branchLocationId', this.addGrnForm.get('branchLocationId').value);
    formData.append('supplierId', this.addGrnForm.get('supplierId').value);
    formData.append('grnDate', this.addGrnForm.get('grnDate').value);
    formData.append('createdBy', this.commonService.getUser());
    formData.append('totalDiscountAmount', this.addGrnForm.get('totalDiscountAmount').value);
   /*     formData.append('totalBillAmount', this.addGrnForm.get('totalBillAmount').value); */
    formData.append('applicationUserId', this.commonService.getUserId());
    if (!this.addGrnForm.valid) {
      return false;
    } else {
      //alert(JSON.stringify(this.addGrnForm.value));
      this.grnService.CreateGoodReceptNoteHeader(formData).subscribe(
        (data: any) => {
          console.log(data);
          this.goodReceptNoteHeaderId=data.goodReceptNoteHeaderId;
          console.log(data.goodReceptNoteHeaderId);
          //     this.alertifyService.success("Saved successfully.");
        },
        (error) => {
          this.toastrService.error(
            JSON.stringify(error.text).replace('"', '').replace('"', '')
          );
        }, () => {
          localStorage.removeItem('postData');
          this.toastrService.success("Saved successfully.");
          this.router.navigate(['/grn/goodreceptnotereport/',this.goodReceptNoteHeaderId]);
          //this.router.navigate(['/grn/grnlist/']);
          this.initGoodReceiptNote();

        }
      );
    }

  }
  onFileChange(event) {
    // this.selectedFile = <File>event.target.files[0];
    //  console.log(this.selectedFile);
  }
  changePO() {
   // this.clearCart();
    this.lines = [];
    this.total = 0.00;
    this.count = 0;
    this.discount = 0.00;
    this.subtotal = 0.00;
    console.log(this.selectedItem);
    this.purchaseorderService.GetPurchaseOrderDetails(this.selectedItem).subscribe((data: PurchaseOrderDetail[]) => {
      //  this.lines = data;
      console.log('change po in grn');
      console.log(data);
      this.postData.branchLocationId =data[0].purchaseOrderHeaders?.branchLocationId;
      this.postData.supplierId=data[0].purchaseOrderHeaders?.supplierId;
      this.postData.purchaseOrderHeaderId=data[0].purchaseOrderHeaders.purchaseOrderHeaderId;
      localStorage.setItem('postData', JSON.stringify(this.postData));
      this.products = data;
      //  this.lines = data;
      console.log(this.lines);
      //    this.count = data.count;
      this.addGrnForm.patchValue({
        branchLocationId: data[0].purchaseOrderHeaders?.branchLocationId,
        supplierId: data[0].purchaseOrderHeaders?.supplierId,
        purchaseOrderHeaderId: this.selectedItem
      });
      //this.selectedSupplier = data[0].purchaseOrderHeaders?.supplierId;
      console.log("changepo");
      console.log(data);
        data.forEach(item => {
          this.grnItemList.push({ barCode: item.product.barcode, createdBy: this.commonService.getUser(), quantity: item.quantity, priceId: item.supplierPriceId, purchaseOrderHeaderId: item.purchaseOrderHeaderId, dNetQuantity: item.currentQuantity })
        });

      this.grnService.Updategrnitemdetaillist(this.grnItemList).subscribe(
        (data: DataEdit) => {
          this.lines = data?.lines;
          this.count = data?.count;
          this.subtotal = data?.totalBillAmount;
          this.total = data?.totalCost;
          this.discount = data?.totalDiscountAmount;
        }
      );
    });

  }
  changeP(e) {
    console.log(e.target.value);
    console.log("changep");
    console.log(this.products);
    this.linesp = this.products.filter(p => p.product.productId == e.target.value);
    console.log(this.linesp);

    this.addGrnListForm.patchValue({
      quantity: this.linesp[0].quantity,
      productName: this.linesp[0].product.productName,
      supplierPriceId: this.linesp[0].supplierPriceId,
      purchaseOrderHeaderId: this.linesp[0].purchaseOrderHeaders.purchaseOrderHeaderId,
      branchLocationId: this.linesp[0].purchaseOrderHeaders.branchLocationId,
      supplierId: this.linesp[0].purchaseOrderHeaders.supplierId,
      retailPrice: this.linesp[0].supplierPrice?.maximumRetailPrice,
    });

  }
  grnAddList() {
    const formData: any = new FormData();
    formData.append('productId', this.addGrnListForm.get('productId').value);
    formData.append('quantity', this.addGrnListForm.get('quantity').value);
    formData.append('freequantity', this.addGrnListForm.get('freequantity').value);
    formData.append('supplierPriceId', this.addGrnListForm.get('supplierPriceId').value);
    formData.append('productExpiryDate', this.addGrnListForm.get('productExpiryDate').value);
    //formData.append('batchnumber', this.addGrnListForm.get('batchnumber').value);
    //formData.append('barcode', this.addGrnListForm.get('productId').value);
    formData.append('purchaseOrderHeaderId', this.addGrnListForm.get('purchaseOrderHeaderId').value);
    formData.append('retailPrice', this.addGrnListForm.get('retailPrice').value);
    formData.append('createdBy', this.commonService.getUser());
    if (!this.addGrnListForm.valid) {
      return false;
    } else {
      // alert(JSON.stringify(this.addGrnListForm.value));
      this.grnService.Creategrnitemdetails(formData).subscribe(
        (data: DataEdit) => {
          this.lines = data?.lines;
          this.count = data?.count;
          this.subtotal = data?.totalBillAmount;
          this.total = data?.totalCost;
          this.discount = data?.totalDiscountAmount;
        }
      );
    }

  }
  remove(code: any, poId: any,supplierPriceId: any) {
    console.log(code + "and" + poId);
    const formData: any = new FormData();
    formData.append('barCode', code);
    formData.append('purchaseOrderHeaderId', poId);
    formData.append('supplierPriceId', supplierPriceId);
    this.grnService.RemoveGoodReceptNoteItemDetails(formData).subscribe((data) => {
      console.log('remove grn item line');
      console.log(data);
      this.initGoodReceiptNote();
      this.toastrService.success("Deleted successfully.");
    });


  }
  quantityUpdate(e: any, code: any, poId: any,supplierPriceId: any) {
    console.log(e.target.value);
    const quantity = e.target.value
    console.log(code + "and" + quantity);
    const formData: any = new FormData();
    formData.append('barCode', code);
    formData.append('quantity', quantity);
    formData.append('purchaseOrderHeaderId', poId);
    formData.append('supplierPriceId', supplierPriceId);
    this.grnService.UpdateGrnItemQuantity(formData).subscribe((data) => {
      console.log('remove grn item line');
      console.log(data);
      this.initGoodReceiptNote();
      this.toastrService.success("Quantity added successfully.");
    });
  }
  freeQuantityUpdate(e: any, code: any, poId: any,supplierPriceId: any) {
    console.log(e.target.value);
    const quantity = e.target.value
    console.log(code + "and" + quantity);
    const formData: any = new FormData();
    formData.append('barCode', code);
    formData.append('quantity', quantity);
    formData.append('purchaseOrderHeaderId', poId);
    formData.append('supplierPriceId', supplierPriceId);
    this.grnService.UpdateGoodReceptNoteItemDetails(formData).subscribe((data) => {
      console.log('remove grn item line');
      console.log(data);
      this.initGoodReceiptNote();
      this.toastrService.success("free quantity added successfully.");
    });
  }
  updateExpiryDate(e: any, code: any, poId: any,supplierPriceId: any) {
    console.log(e.target.value);
    const productExpiryDate = e.target.value
    const formData: any = new FormData();
    formData.append('barCode', code);
    formData.append('purchaseOrderHeaderId', poId);
    formData.append('supplierPriceId', supplierPriceId);
    formData.append('productExpiryDate', productExpiryDate);
    this.grnService.DateUpdateGoodReceptNoteItemDetails(formData).subscribe((data) => {
      console.log('remove grn item line');
      console.log(data);
      this.initGoodReceiptNote();
      this.toastrService.success("ExpiryDate added successfully.");
    });

  }
  updateGroupDiscount(e: any, code: any, poId: any,supplierPriceId: any) {
    console.log(e.target.value);
    const groupDiscount = e.target.value
    const formData: any = new FormData();
    formData.append('barCode', code);
    formData.append('purchaseOrderHeaderId', poId);
    formData.append('supplierPriceId', supplierPriceId);
    formData.append('groupDiscountAmount', groupDiscount);
    this.grnService.GroupDiscountUpdateGoodReceptNoteItemDetails(formData).subscribe((data) => {
      console.log('remove grn item line');
      console.log(data);
      this.initGoodReceiptNote();
      this.toastrService.success("Discount Price added successfully.");
    });
  }
  updateGroupDiscountAmount(e: any, code: any, poId: any,supplierPriceId: any) {
    console.log(e.target.value);
    const groupDiscount = e.target.value
    const formData: any = new FormData();
    formData.append('barCode', code);
    formData.append('purchaseOrderHeaderId', poId);
    formData.append('supplierPriceId', supplierPriceId);
    formData.append('groupDiscountAmountAmount', groupDiscount);
    this.grnService.GroupDiscountAmountUpdateGoodReceptNoteItemDetails(formData).subscribe((data) => {
      console.log('remove grn item line');
      console.log(data);
      this.initGoodReceiptNote();
      this.toastrService.success("Discount Price added successfully.");
    });
  }

  updateDiscountPrice(e: any, code: any, poId: any,supplierPriceId: any) {
    console.log(e.target.value);
    const PricePrecentage = e.target.value
    const formData: any = new FormData();
    formData.append('barCode', code);
    formData.append('purchaseOrderHeaderId', poId);
    formData.append('supplierPriceId', supplierPriceId);
    formData.append('pricePrecentage', PricePrecentage);
    this.grnService.DiscountUpdateGoodReceptNoteItemDetails(formData).subscribe((data) => {
      console.log('remove grn item line');
      console.log(data);
      this.initGoodReceiptNote();
      this.toastrService.success("Discount Price added successfully.");
    });

  }
  updateVatPrice(e: any, code: any, poId: any,supplierPriceId: any) {
    console.log(e.target.value);
    const VatPrecentage = e.target.value
    const formData: any = new FormData();
    formData.append('barCode', code);
    formData.append('purchaseOrderHeaderId', poId);
    formData.append('supplierPriceId', supplierPriceId);
    formData.append('vatPrecentage', VatPrecentage);
    this.grnService.VatUpdateGoodReceptNoteItemDetails(formData).subscribe((data) => {
      console.log('remove grn item line');
      console.log(data);
      this.initGoodReceiptNote();
      this.toastrService.success("Discount Price added successfully.");
    });

  }
  updateNbtPrice(e: any, code: any, poId: any,supplierPriceId: any) {
    console.log(e.target.value);
    const nbtPrecentage = e.target.value
    const formData: any = new FormData();
    formData.append('barCode', code);
    formData.append('purchaseOrderHeaderId', poId);
    formData.append('supplierPriceId', supplierPriceId);
    formData.append('nbtPrecentage', nbtPrecentage);
    this.grnService.DiscountUpdateGoodReceptNoteItemDetails(formData).subscribe((data) => {
      console.log('remove grn item line');
      console.log(data);
      this.initGoodReceiptNote();
      this.toastrService.success("Discount Price added successfully.");
    });

  }
  overwriteStockPrice(e: any, code: any, poId: any,supplierPriceId: any) {
    console.log(e.target.checked);
    const productExpiryDate = e.target.checked
    const formData: any = new FormData();
    formData.append('barCode', code);
    formData.append('purchaseOrderHeaderId', poId);
    formData.append('supplierPriceId', supplierPriceId);
    formData.append('overWriteStockPrice', this.theCheckbox);
    this.grnService.Overwritestockpricegoodreceptnoteitemdetails(formData).subscribe((data) => {
      console.log('overwriteStockPrice grn item line');
      console.log(data);
      this.initGoodReceiptNote();
      this.toastrService.success("Overwrite Stock Price successfully.");
    });
  }
  openModalWithComponent(barcode: string) {
    const initialState = {
      barcode,
      title: 'kokila'
    };
    this.bsModalRef = this.modalService.show(StockdetailComponent, { initialState, class: 'gray modal-xl' });

  }
  /* getBranches(){
    this.commonService.getList('Common', 'getallbranch').subscribe(data => {
      const mapBranches=this.commonService.getMapUserBranchLocations().length >0 ?  this.commonService.getMapUserBranchLocations() : this.commonService.getUserBranchLocationId();
      const branches = mapBranches.length >1 ?
      data.filter(array => mapBranches.some(filter => filter == array.branchLocationId))
      : data.filter(d => d.branchLocationId==this.commonService.getUserBranchLocationId());
      this.branches =branches;
    });
  } */
}

