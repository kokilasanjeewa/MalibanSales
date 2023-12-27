import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PurchaseRoutingModule } from './purchase-routing.module';
import { CreatepurchaseComponent } from './createpurchase/createpurchase.component';
import { PurchaselistComponent } from './purchaselist/purchaselist.component';
import { PurchaserequestComponent } from './purchaserequest/purchaserequest.component';
import { PurchaseorderComponent } from './purchaseorder/purchaseorder.component';
import { PurchaseorderlistComponent } from './purchaseorderlist/purchaseorderlist.component';
import { MaterialModule } from '../material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { PurchaseorderdetailComponent } from './purchaseorderdetail/purchaseorderdetail.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { PurchaseorderreportComponent } from './purchaseorderreport/purchaseorderreport.component';
import { PurchasereturnComponent } from './purchasereturn/purchasereturn.component';
import { PurchasereturnreportComponent } from './purchasereturnreport/purchasereturnreport.component';
import { PurchasereturnlistComponent } from './purchasereturnlist/purchasereturnlist.component';
import { IconSpacerModule } from '../iconspacer.module';
import { PurchaseordereditComponent } from './purchaseorderedit/purchaseorderedit.component';
import { PurchaseorderapprovemodelComponent } from './purchaseorderapprovemodel/purchaseorderapprovemodel.component';

@NgModule({
  declarations: [
    CreatepurchaseComponent,
    PurchaselistComponent,
    PurchaserequestComponent,
    PurchaseorderComponent,
    PurchaseordereditComponent,
    PurchaseorderlistComponent,
    PurchaseorderdetailComponent,
    PurchaseorderreportComponent,
    PurchasereturnComponent,
    PurchasereturnreportComponent,
    PurchasereturnlistComponent,
    PurchaseorderapprovemodelComponent,
  ],
  imports: [
    CommonModule,
    PurchaseRoutingModule,
    IconSpacerModule,
    MaterialModule,
    ReactiveFormsModule,
    NgSelectModule,
    FormsModule,
    PaginationModule.forRoot()

  ],
})
export class PurchaseModule { }
