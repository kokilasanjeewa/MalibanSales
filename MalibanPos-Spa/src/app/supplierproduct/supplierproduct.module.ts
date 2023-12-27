import { ShortenPipe } from './../_custompipe/shorten.pipe';
import { MaterialModule } from './../material.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SupplierproductRoutingModule } from './supplierproduct-routing.module';
import { AddComponent } from './add/add.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { IconSpacerModule } from '../iconspacer.module';
import { FilterPipe } from '../_custompipe/filter.pipe';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedModule } from "../shared/shared.module";


@NgModule({
    declarations: [
        AddComponent,
        ShortenPipe,
    ],
    imports: [
        CommonModule,
        MaterialModule,
        ReactiveFormsModule,
        IconSpacerModule,
        FormsModule,
        SupplierproductRoutingModule,
        NgSelectModule,
        SharedModule
    ]
})
export class SupplierproductModule { }
