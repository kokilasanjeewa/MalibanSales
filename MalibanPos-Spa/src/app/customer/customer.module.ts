import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CustomerRoutingModule } from './customer-routing.module';
import { CustomerlistComponent } from './customerlist/customerlist.component';
import { IconSpacerModule } from '../iconspacer.module';
import { MaterialModule } from '../material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { UploadcustomerComponent } from './uploadcustomer/uploadcustomer.component';
import { EditcustomerComponent } from './editcustomer/editcustomer.component';
import { MapcustomerComponent } from './mapcustomer/mapcustomer.component';


@NgModule({
  declarations: [CustomerlistComponent, UploadcustomerComponent, EditcustomerComponent, MapcustomerComponent],
  imports: [
    CommonModule,
    CustomerRoutingModule,
    IconSpacerModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    PaginationModule.forRoot()
  ]
})
export class CustomerModule { }
