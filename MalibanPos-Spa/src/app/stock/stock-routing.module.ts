import { ShopStockListResolver } from './../_resolvers/shopstocklist.resolver.';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AddstockComponent } from './addstock/addstock.component';
import { StocklistComponent } from './stocklist/stocklist.component';
import { StockadjustmentComponent } from './stockadjustment/stockadjustment.component';
import { StockadjustmentlistComponent } from './stockadjustmentlist/stockadjustmentlist.component';
import { StockAdjustmentListResolver } from '../_resolvers/stockadjustmentlist.resolver';
import { StockadjustmentdetailComponent } from './stockadjustmentdetail/stockadjustmentdetail.component';
import { StockAdjustmentDetailResolver } from '../_resolvers/stockadjustmentdetail.resolver';
import { StockadjustmentapproveComponent } from './stockadjustmentapprove/stockadjustmentapprove.component';


const routes: Routes = [
  {
    path: 'addstock',
    component: AddstockComponent,
  },
  {
    path: 'stockadjustment',
    component: StockadjustmentComponent,
  },
  {
    path: 'stockadjustmentapprove/:stockadjustmentHeaderId',
    component: StockadjustmentapproveComponent,
  },
  {
    path: 'stockadjustmentlist',
    component: StockadjustmentlistComponent,
    resolve: { stockadjustments: StockAdjustmentListResolver }
  },
  {
    path: 'stockadjustmentdetail/:stockadjustmentHeaderId',
    component: StockadjustmentdetailComponent,
    resolve: { stockadjustmentDetails: StockAdjustmentDetailResolver }
  },
  {
    path: 'stocklist',
    component: StocklistComponent,
    resolve: { shopstocks: ShopStockListResolver }

  }, {
    path: '**',
    redirectTo: './stocklist',
    pathMatch: 'full'
  }

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StockRoutingModule { }
