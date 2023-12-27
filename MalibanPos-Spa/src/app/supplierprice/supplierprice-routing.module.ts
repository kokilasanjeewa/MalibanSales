import { SupplierPriceListResolver } from './../_resolvers/supplierpricelist.resolver';
import { SupplierpricelistComponent } from './supplierpricelist/supplierpricelist.component';
import { Component, NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AddsupplierpriceComponent } from './addsupplierprice/addsupplierprice.component';
import { HasClaimDirective } from '../_directives/has-claim.directive';
import { AuthGuard } from '../_guards/auth.guard';


const routes: Routes = [
  {
    path: 'addsupplierprice',
    component: AddsupplierpriceComponent,
    canActivate: [AuthGuard],
    //data: {claims: ['CanAccessAddSupplierPrice']}
  },
  {
    path: 'supplierpricelist',
    component: SupplierpricelistComponent,
    resolve: { supplierPrices: SupplierPriceListResolver },
    canActivate: [AuthGuard],
    //canLoad: [AuthGuard],
    // data: {claim: ['CanAccessAddSupplierPrice', 'CanAccessAddSupplier']}
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SupplierpriceRoutingModule { }
