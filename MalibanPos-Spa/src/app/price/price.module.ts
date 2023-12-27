import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PriceRoutingModule } from './price-routing.module';
import { AddpriceComponent } from './addprice/addprice.component';
import { EditpriceComponent } from './editprice/editprice.component';
import { PricelistComponent } from './pricelist/pricelist.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [AddpriceComponent, EditpriceComponent, PricelistComponent],
  imports: [
    CommonModule,
    PriceRoutingModule,
    ReactiveFormsModule,
    FormsModule
  ]
})
export class PriceModule { }
