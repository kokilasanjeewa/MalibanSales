import { ProductListResolver } from './../_resolvers/productlistresolver';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AddproductComponent } from './addproduct/addproduct.component';
import { ProductlistComponent } from './productlist/productlist.component';
import { EditproductComponent } from './editproduct/editproduct.component';
import { AuthGuard } from '../_guards/auth.guard';

const routes: Routes = [
  {
    path: 'addproduct',
    component: AddproductComponent,
    //canActivate: [AuthGuard],
  },
  {
    path: 'editproduct/:productId',
    component: EditproductComponent,
  },
  {
    path: 'productlist',
    component: ProductlistComponent,
    resolve: { products: ProductListResolver },
    canActivate: [AuthGuard],
  },
  {
    path: '**',
    redirectTo: './productlist',
    pathMatch: 'full',
    //canActivate: [AuthGuard],
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ProductRoutingModule { }
