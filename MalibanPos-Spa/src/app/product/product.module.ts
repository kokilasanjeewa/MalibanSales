import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductRoutingModule } from './product-routing.module';
import { AddproductComponent } from './addproduct/addproduct.component';
import { ProductlistComponent } from './productlist/productlist.component';
import { MaterialModule } from '../material.module';
import { EditproductComponent } from './editproduct/editproduct.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { IconSpacerModule } from '../iconspacer.module';


@NgModule({
  declarations: [AddproductComponent, ProductlistComponent, EditproductComponent],
  imports: [
    CommonModule,
    ProductRoutingModule,
    IconSpacerModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    PaginationModule.forRoot()
  ]
})
export class ProductModule { }
