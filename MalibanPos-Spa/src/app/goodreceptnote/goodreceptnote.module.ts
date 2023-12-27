import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GoodreceptnoteRoutingModule } from './goodreceptnote-routing.module';
import { CreategrnComponent } from './creategrn/creategrn.component';
import { MaterialModule } from '../material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { GrnlistComponent } from './grnlist/grnlist.component';
import { GrndetailComponent } from './grndetail/grndetail.component';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgSelectModule } from '@ng-select/ng-select';
import { GoodreceptnotereportComponent } from './goodreceptnotereport/goodreceptnotereport.component';
import { StockdetailComponent } from './stockdetail/stockdetail.component';
import { IconSpacerModule } from '../iconspacer.module';
import { EditgrnComponent } from './editgrn/editgrn.component';

@NgModule({
  declarations: [CreategrnComponent, GrnlistComponent, GrndetailComponent, GoodreceptnotereportComponent, StockdetailComponent, EditgrnComponent],
  imports: [
    CommonModule,
    GoodreceptnoteRoutingModule,
    MaterialModule,
    IconSpacerModule,
    ReactiveFormsModule,
    PaginationModule.forRoot(),
    NgSelectModule,
    FormsModule
  ]
})
export class GoodreceptnoteModule { }
