import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CustomerListResolver } from '../_resolvers/customerlist.resolver';
import { CustomerlistComponent } from './customerlist/customerlist.component';
import { EditcustomerComponent } from './editcustomer/editcustomer.component';
import { MapcustomerComponent } from './mapcustomer/mapcustomer.component';
import { UploadcustomerComponent } from './uploadcustomer/uploadcustomer.component';

const routes: Routes = [
  { path: 'editcustomer/:customerId', component: EditcustomerComponent },
  {
    path: 'customerlist',
    component: CustomerlistComponent,
    resolve: { customers: CustomerListResolver },
  },
  {
    path: 'uploadcustomer',
    component: UploadcustomerComponent,
  },
  {
    path: 'mapcustomer',
    component: MapcustomerComponent,
  },
  {
    path: '**',
    redirectTo: './customerlist',
    pathMatch: 'full',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CustomerRoutingModule {}
