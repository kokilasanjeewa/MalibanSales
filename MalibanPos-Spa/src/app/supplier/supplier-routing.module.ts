import { SupplierListResolver } from './../_resolvers/supplierlist.resolver';
import { EditsupplierComponent } from './editsupplier/editsupplier.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AddsupplierComponent } from './addsupplier/addsupplier.component';
import { SupplierlistComponent } from './supplierlist/supplierlist.component';
import { DetailsupplierComponent } from './detailsupplier/detailsupplier.component';
import { AuthGuard } from '../_guards/auth.guard';

const routes: Routes = [
  {
    path: 'addsupplier',
    component: AddsupplierComponent,
  },
  {
    path: 'editsupplier/:supplierId',
    component: EditsupplierComponent,
  },
  {
    path: 'detailsupplier/:supplierId',
    component: DetailsupplierComponent,
  }
  , {
    path: 'supplierlist',
    component: SupplierlistComponent,
    resolve: { suppliers: SupplierListResolver }
  }, {
    path: '**',
    redirectTo: './supplierlist',
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SupplierRoutingModule { }
