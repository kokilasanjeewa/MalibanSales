import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { OrderComponent } from './order/order.component';
import { ReceptComponent } from './recept/recept.component';
import { ReprintComponent } from './reprint/reprint.component';
import { ReturnreceiptComponent } from './returnreceipt/returnreceipt.component';
import { SalereturnComponent } from './salereturn/salereturn.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'order',
    pathMatch: 'full',
  },
  {
    path: 'order',
    component: OrderComponent,
  },
  {
    path:'salereturn',
    component: SalereturnComponent,
  },
  {
    path: 'recept',
    component: ReceptComponent,
  },
  {
    path: 'returnreceipt',
    component: ReturnreceiptComponent,
  },
  {
    path: 'reprint',
    component: ReprintComponent,
  }
  ];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PosRoutingModule {}
