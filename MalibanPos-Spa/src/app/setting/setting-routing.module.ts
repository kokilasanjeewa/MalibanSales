import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CompanyMasterListResolver } from '../_resolvers/companymasterlist.resolver';
import { CustomerMasterListResolver } from '../_resolvers/customermasterlist.resolver';
import { CompanymastersettingComponent } from './companymastersetting/companymastersetting.component';
import { CompanymastersettinglistComponent } from './companymastersettinglist/companymastersettinglist.component';
import { CustomermastersettingComponent } from './customermastersetting/customermastersetting.component';
import { CustomermastersettinglistComponent } from './customermastersettinglist/customermastersettinglist.component';
import { EditcompanymastersettingComponent } from './editcompanymastersetting/editcompanymastersetting.component';
import { EditcustomermastersettingComponent } from './editcustomermastersetting/editcustomermastersetting.component';
const routes: Routes = [
  { path: 'customermaster', component: CustomermastersettingComponent },
  { path: 'customermasterlist', component: CustomermastersettinglistComponent, resolve: { customerMasterlist: CustomerMasterListResolver } },
  { path: 'editcustomermaster/:calculationMasterId', component: EditcustomermastersettingComponent },
  { path: 'companymaster', component: CompanymastersettingComponent },
  { path: 'editcompanymaster/:calculationMasterId', component: EditcompanymastersettingComponent },
  { path: 'companymasterlist', component: CompanymastersettinglistComponent,    resolve: { companyMasterlist: CompanyMasterListResolver },
},

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SettingRoutingModule { }
