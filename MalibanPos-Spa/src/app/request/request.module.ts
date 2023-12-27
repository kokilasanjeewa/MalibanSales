import { MaterialModule } from './../material.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RequestRoutingModule } from './request-routing.module';
import { PurchaseComponent } from './purchase/purchase.component';
import { RequestlistComponent } from './requestlist/requestlist.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { RequestdetailComponent } from './requestdetail/requestdetail.component';


@NgModule({
  declarations: [PurchaseComponent, RequestlistComponent, RequestdetailComponent],
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    FormsModule,
    RequestRoutingModule,
    PaginationModule.forRoot()
  ]
})
export class RequestModule { }
