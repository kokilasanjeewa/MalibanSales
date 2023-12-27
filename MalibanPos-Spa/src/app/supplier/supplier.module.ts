import { LayoutModule } from './../layout/layout.module';
import { NavigationService } from './../_services/navigation.service';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SupplierRoutingModule } from './supplier-routing.module';
import { AddsupplierComponent } from './addsupplier/addsupplier.component';
import { MaterialModule } from '../material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { EditsupplierComponent } from './editsupplier/editsupplier.component';
import { SupplierlistComponent } from './supplierlist/supplierlist.component';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { DetailsupplierComponent } from './detailsupplier/detailsupplier.component';
import { IconSpacerModule } from '../iconspacer.module';


@NgModule({
  declarations: [AddsupplierComponent, EditsupplierComponent, SupplierlistComponent, DetailsupplierComponent],
  imports: [
    CommonModule,
    SupplierRoutingModule,
    IconSpacerModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    PaginationModule.forRoot(),
    ],
  providers:[NavigationService],
})
export class SupplierModule { }
