import { MaterialModule } from './../material.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SupplierpriceRoutingModule } from './supplierprice-routing.module';
import { AddsupplierpriceComponent } from './addsupplierprice/addsupplierprice.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SupplierpricelistComponent } from './supplierpricelist/supplierpricelist.component';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { IconSpacerModule } from '../iconspacer.module';
import { NgSelectModule } from '@ng-select/ng-select';

@NgModule({
  declarations: [AddsupplierpriceComponent, SupplierpricelistComponent],
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    IconSpacerModule,
    SupplierpriceRoutingModule,
    NgSelectModule,
    PaginationModule.forRoot()
  ]
})
export class SupplierpriceModule { }
