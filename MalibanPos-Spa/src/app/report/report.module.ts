import { NgModule} from '@angular/core';
import { CommonModule } from '@angular/common';

import { ReportRoutingModule } from './report-routing.module';
import { ViewreportComponent } from './viewreport/viewreport.component';
import { MaterialModule } from '../material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { CustomerComponent } from './customer/customer.component';
import { StockComponent } from './stock/stock.component';
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
import { IconSpacerModule } from '../iconspacer.module';
import { BincardComponent } from './bincard/bincard.component';
import { CustomerunpaidComponent } from './customerunpaid/customerunpaid.component';
import { ReceiptdetailsbydaterangeComponent } from './receiptdetailsbydaterange/receiptdetailsbydaterange.component';
import { ItemwisesaledetailsbydaterangeComponent } from './itemwisesaledetailsbydaterange/itemwisesaledetailsbydaterange.component';
import { DatewiselunchpacketsummaryComponent } from './datewiselunchpacketsummary/datewiselunchpacketsummary.component';
import { StockdatewiseComponent } from './stockdatewise/stockdatewise.component';
import { DatewisesaledetailsummaryComponent } from './datewisesaledetailsummary/datewisesaledetailsummary.component';
import { ItemwisemovingnonmovingstockComponent } from './itemwisemovingnonmovingstock/itemwisemovingnonmovingstock.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { ExpirationstockComponent } from './expirationstock/expirationstock.component';
import { PurchaseorderfilterlistComponent } from './purchaseorderfilterlist/purchaseorderfilterlist.component';
import { StocktransferComponent } from './stocktransfer/stocktransfer.component';

@NgModule({
  declarations: [ViewreportComponent, CustomerComponent, StockComponent, SaleComponent, PurchasereturnComponent, PurchaseorderComponent, GoodreceiptnoteComponent, StockadjestmentComponent, SalereturnComponent, SalesummaryComponent, SupplierlistComponent, SalesummarybycustomerComponent, SalesdetailsbycustomerComponent, BincardComponent, CustomerunpaidComponent, ReceiptdetailsbydaterangeComponent, ItemwisesaledetailsbydaterangeComponent, DatewiselunchpacketsummaryComponent,StockdatewiseComponent, DatewisesaledetailsummaryComponent, ItemwisemovingnonmovingstockComponent, ExpirationstockComponent, PurchaseorderfilterlistComponent, StocktransferComponent],
  imports: [
    CommonModule,
    ReportRoutingModule,
    IconSpacerModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    PaginationModule.forRoot(),
    NgSelectModule
  ],

})
export class ReportModule { }
