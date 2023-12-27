import { PurchaseorderlistComponent } from './purchaseorderlist/purchaseorderlist.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PurchaselistComponent } from './purchaselist/purchaselist.component';
import { PurchaseorderComponent } from './purchaseorder/purchaseorder.component';
import { PurchaseOrderListResolver } from '../_resolvers/purchaseorderlist.resolver';
import { PurchaseorderdetailComponent } from './purchaseorderdetail/purchaseorderdetail.component';
import { PurchaseOrderDetailResolver } from '../_resolvers/purchaseorderdetail.resolver';
import { PurchaseorderreportComponent } from './purchaseorderreport/purchaseorderreport.component';
import { PurchasereturnComponent } from './purchasereturn/purchasereturn.component';
import { PurchasereturnreportComponent } from './purchasereturnreport/purchasereturnreport.component';
import { PurchaseReturnListResolver } from '../_resolvers/purchasereturnlist.resolver';
import { PurchasereturnlistComponent } from './purchasereturnlist/purchasereturnlist.component';
import { PurchaseordereditComponent } from './purchaseorderedit/purchaseorderedit.component';



const routes: Routes = [
  {
    path: 'purchaselist',
    component: PurchaselistComponent
  },
  {
    path: 'purchaseorderlist',
    component: PurchaseorderlistComponent,
    resolve: { purchaseOrderHeaders: PurchaseOrderListResolver }
  },
  {
    path: 'purchaseorderdetail/:purchaseOrderHeaderId',
    component: PurchaseorderdetailComponent,
    resolve: { purchaseOrderDetails: PurchaseOrderDetailResolver }
  },
  {
    path: 'purchaseorder',
    component: PurchaseorderComponent
  },
  {
    path: 'purchaseorderreport/:purchaseOrderId',
    component: PurchaseorderreportComponent
  },
  {
    path: 'purchasereturn',
    component: PurchasereturnComponent
  },
  {
    path: 'purchasereturnlist',
    component: PurchasereturnlistComponent,
    resolve: { purchaseReturnHeaders: PurchaseReturnListResolver }
  },
  {
    path: 'purchasereturnreport/:purchaseReturnId',
    component: PurchasereturnreportComponent
  },
  {
    path: 'purchaseorderedit/:purchaseOrderHeaderId',
    component: PurchaseordereditComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PurchaseRoutingModule { }

/* ,
  {
    path: 'purchaseorderapprove/:purchaseOrderHeaderId',
    component: PurchaseorderapproveComponent
  },
  {
    path: 'purchaseorderconfirm/:purchaseOrderHeaderId',
    component: PurchaseorderconfirmComponent
  }, */
