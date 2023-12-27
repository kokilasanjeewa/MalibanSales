import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SettingRoutingModule } from './setting-routing.module';
import { CustomermastersettingComponent } from './customermastersetting/customermastersetting.component';
import { CompanymastersettingComponent } from './companymastersetting/companymastersetting.component';
import { MaterialModule } from '../material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { IconSpacerModule } from '../iconspacer.module';
import { NgSelectModule } from '@ng-select/ng-select';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { CompanymastersettinglistComponent } from './companymastersettinglist/companymastersettinglist.component';
import { CustomermastersettinglistComponent } from './customermastersettinglist/customermastersettinglist.component';
import { EditcompanymastersettingComponent } from './editcompanymastersetting/editcompanymastersetting.component';
import { EditcustomermastersettingComponent } from './editcustomermastersetting/editcustomermastersetting.component';


@NgModule({
  declarations: [CustomermastersettingComponent, CompanymastersettingComponent, CompanymastersettinglistComponent, CustomermastersettinglistComponent, EditcompanymastersettingComponent, EditcustomermastersettingComponent,],
  imports: [
    CommonModule,
    SettingRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    IconSpacerModule,
    NgSelectModule,
    PaginationModule.forRoot()
  ]
})
export class SettingModule { }
