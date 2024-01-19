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
import { StocktransferComponent } from './stocktransfer/stocktransfer.component';
import { StocktransferlistComponent } from './stocktransferlist/stocktransferlist.component';
import { StockTransferListResolver } from '../_resolvers/stocktransferlist.resolver';
import { StocktransferreportComponent } from './stocktransferreport/stocktransferreport.component';
import { StocktransferapproveComponent } from './stocktransferapprove/stocktransferapprove.component';
import { StocktransfereditComponent } from './stocktransferedit/stocktransferedit.component';


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
    path: 'stocktransfer',
    component: StocktransferComponent,
  },
  {
    path: 'stocktransferreport/:stockTransferHeaderId',
    component: StocktransferreportComponent,
  },
  {
    path: 'stocktransferedit/:stockTransferHeaderId',
    component: StocktransfereditComponent,
  },
  {
    path: 'stocktransferapprove/:stockTransferHeaderId',
    component: StocktransferapproveComponent,
  },
  {
    path: 'stocktransferlist',
    component: StocktransferlistComponent,
    resolve: { stocktransferlist: StockTransferListResolver }
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
