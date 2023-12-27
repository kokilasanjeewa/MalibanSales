import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AddpriceComponent } from './addprice/addprice.component';
import { EditpriceComponent } from './editprice/editprice.component';
import { PricelistComponent } from './pricelist/pricelist.component';


const routes: Routes = [
  {
    path: 'addprice',
    component: AddpriceComponent,
  },
  {
    path: 'editprice',
    component: EditpriceComponent,
  },
  {
    path: 'pricelist',
    component: PricelistComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PriceRoutingModule { }
