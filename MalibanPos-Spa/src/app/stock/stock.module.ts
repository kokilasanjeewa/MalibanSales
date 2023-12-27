import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgSelectModule } from '@ng-select/ng-select';
import { StockRoutingModule } from './stock-routing.module';
import { AddstockComponent } from './addstock/addstock.component';
import { StocklistComponent } from './stocklist/stocklist.component';
import { StockadjustmentComponent } from './stockadjustment/stockadjustment.component';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from '../material.module';
import { StockadjustmentlistComponent } from './stockadjustmentlist/stockadjustmentlist.component';
import { StockadjustmentdetailComponent } from './stockadjustmentdetail/stockadjustmentdetail.component';
import { IconSpacerModule } from '../iconspacer.module';
import { StockadjustmentapproveComponent } from './stockadjustmentapprove/stockadjustmentapprove.component';
import { StockadjustmentconfirmComponent } from './stockadjustmentconfirm/stockadjustmentconfirm.component';
import { StocktransferComponent } from './stocktransfer/stocktransfer.component';
@NgModule({
  declarations: [AddstockComponent, StocklistComponent, StockadjustmentComponent, StockadjustmentlistComponent, StockadjustmentdetailComponent, StockadjustmentapproveComponent, StockadjustmentconfirmComponent, StocktransferComponent],
  imports: [
    CommonModule,
    StockRoutingModule,
    MaterialModule,
    IconSpacerModule,
    ReactiveFormsModule,
    FormsModule,
    NgSelectModule,
    PaginationModule.forRoot()
  ]
})
export class StockModule { }
