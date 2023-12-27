import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { IconSpacerModule } from '../iconspacer.module';
import { BreadcrumbComponent } from './breadcrumb/breadcrumb.component';
import { BranchlocationComponent } from './branchlocation/branchlocation.component';


@NgModule({
  declarations: [BreadcrumbComponent, BranchlocationComponent],
  exports: [BreadcrumbComponent],
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    IconSpacerModule,
  ]
})
export class SharedModule { }
