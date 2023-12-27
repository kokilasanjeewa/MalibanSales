import { NgSelectModule } from '@ng-select/ng-select';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PosRoutingModule } from './pos-routing.module';
import { OrderComponent } from './order/order.component';
import { MaterialModule } from '../material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ReceptComponent } from './recept/recept.component';
import { ReturnreceiptComponent } from './returnreceipt/returnreceipt.component';
import { ThermalPrintModule } from 'ng-thermal-print';
import { ReversePipe } from '../_custompipe/reverse.pipe';
import { StopPropagationDirective } from '../_directives/stop-propagation.directive';
import { PricemodalComponent } from './pricemodal/pricemodal.component';
import { IconSpacerModule } from '../iconspacer.module';
import { ReprintComponent } from './reprint/reprint.component';
import { MessagemodelComponent } from './messagemodel/messagemodel.component';
import { SalereturnComponent } from './salereturn/salereturn.component';
import { CheckoutcashmodalComponent } from './checkoutcashmodal/checkoutcashmodal.component';
@NgModule({
  declarations: [ OrderComponent,ReceptComponent,ReturnreceiptComponent,ReversePipe,StopPropagationDirective, PricemodalComponent, ReprintComponent, MessagemodelComponent, SalereturnComponent, CheckoutcashmodalComponent],
  imports: [
    CommonModule,
    PosRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    IconSpacerModule,
    FormsModule,
    NgSelectModule,
    ]
})
export class PosModule { }
