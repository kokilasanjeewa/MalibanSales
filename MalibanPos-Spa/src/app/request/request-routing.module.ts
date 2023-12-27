import { RequestdetailComponent } from './requestdetail/requestdetail.component';
import { PurchaseRequestListResolver } from './../_resolvers/purchaserequestlist.resolver';
import { RequestlistComponent } from './requestlist/requestlist.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PurchaseComponent } from './purchase/purchase.component';
import { PurchaseRequestDetailResolver } from '../_resolvers/purchaserequestdetail.resolver';


const routes: Routes = [
  {
    path: 'purchase',
    component: PurchaseComponent
  }
  , {
    path: 'requestlist',
    component: RequestlistComponent,
    resolve: { purchaseRequestHeaders: PurchaseRequestListResolver }

  }
  ,{
    path: 'requestdetail/:purchaseRequestHeaderId',
    component: RequestdetailComponent,
    resolve: { purchaseRequestDetails: PurchaseRequestDetailResolver }

  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RequestRoutingModule { }
