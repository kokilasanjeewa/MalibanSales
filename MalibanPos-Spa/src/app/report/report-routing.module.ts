import { StockComponent } from './stock/stock.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CustomerComponent } from './customer/customer.component';
import { ViewreportComponent } from './viewreport/viewreport.component';
import { SaleComponent } from './sale/sale.component';
import { PurchasereturnComponent } from './purchasereturn/purchasereturn.component';
import { PurchaseorderComponent } from './purchaseorder/purchaseorder.component';
import { GoodreceiptnoteComponent } from './goodreceiptnote/goodreceiptnote.component';
import { StockadjestmentComponent } from './stockadjestment/stockadjestment.component';
import { SalereturnComponent } from './salereturn/salereturn.component';
import { SalesummaryComponent } from './salesummary/salesummary.component';
import { SupplierlistComponent } from './supplierlist/supplierlist.component';
import { SalesummarybycustomerComponent } from './salesummarybycustomer/salesummarybycustomer.component';
import { SalesdetailsbycustomerComponent } from './salesdetailsbycustomer/salesdetailsbycustomer.component';
import { BincardComponent } from './bincard/bincard.component';
import { CustomerunpaidComponent } from './customerunpaid/customerunpaid.component';
import { ReceiptdetailsbydaterangeComponent } from './receiptdetailsbydaterange/receiptdetailsbydaterange.component';
import { ItemwisesaledetailsbydaterangeComponent } from './itemwisesaledetailsbydaterange/itemwisesaledetailsbydaterange.component';
import { DatewiselunchpacketsummaryComponent } from './datewiselunchpacketsummary/datewiselunchpacketsummary.component';
import { StockdatewiseComponent } from './stockdatewise/stockdatewise.component';
import { DatewisesaledetailsummaryComponent } from './datewisesaledetailsummary/datewisesaledetailsummary.component';
import { ItemwisemovingnonmovingstockComponent } from './itemwisemovingnonmovingstock/itemwisemovingnonmovingstock.component';
import { ExpirationstockComponent } from './expirationstock/expirationstock.component';
import { PurchaseorderfilterlistComponent } from './purchaseorderfilterlist/purchaseorderfilterlist.component';
import { StocktransferComponent } from './stocktransfer/stocktransfer.component';


const routes: Routes = [
  {
    path: 'viewreport',
    component: ViewreportComponent,
  },
  {
    path: 'customer',
    component: CustomerComponent,
  },
  {
    path: 'unpaidcustomer',
    component: CustomerunpaidComponent,
  },
  {
    path: 'stock',
    component: StockComponent,
  },
  {
    path: 'expirationstock',
    component: ExpirationstockComponent,
  },
  {
    path: 'sale',
    component: SaleComponent,
  },
  {
    path: 'salesummary',
    component: SalesummaryComponent,
  },
  {
    path: 'salesummarybycustomer',
    component: SalesummarybycustomerComponent,
  },
  {
    path: 'salereturn',
    component: SalereturnComponent,
  },
  {
    path: 'purchasereturn',
    component: PurchasereturnComponent,
  },
  {
    path: 'purchaseorder',
    component: PurchaseorderComponent,
  },
  {
    path: 'goodreceiptnote',
    component: GoodreceiptnoteComponent,
  },
  {
    path: 'stockadjustment',
    component: StockadjestmentComponent,
  },
  {
    path: 'supplierlist',
    component: SupplierlistComponent,
  },
  {
    path: 'bincard',
    component: BincardComponent,
  },
  {
    path: 'datewiseproductsales',
    component: ItemwisesaledetailsbydaterangeComponent,
  },
  {
    path: 'datewiselunchpacketsummary',
    component: DatewiselunchpacketsummaryComponent,
  },
  {
    path: 'datewisereceiptsummary',
    component: ReceiptdetailsbydaterangeComponent,
  },
  {
    path: 'salesdetailsbycustomer',
    component: SalesdetailsbycustomerComponent,
  },
  {
    path: 'datewisestockdetails',
    component: StockdatewiseComponent,
  },
  {
    path: 'datewisesaledetailsummary',
    component: DatewisesaledetailsummaryComponent,
  },
  {
    path: 'itemwisemovingnonmovingstock',
    component: ItemwisemovingnonmovingstockComponent,
  },
  {
    path:'purchaseorderfilterlist',
    component: PurchaseorderfilterlistComponent,
  },
  {
    path:'stocktransfer',
    component: StocktransferComponent,

  },
  {
    path: '**',
    redirectTo: './viewreport',
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReportRoutingModule { }
