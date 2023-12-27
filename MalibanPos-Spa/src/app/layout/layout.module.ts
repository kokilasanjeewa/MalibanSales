import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FooterComponent } from './footer/footer.component';
import { HeaderComponent } from './header/header.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { IconSpacerModule } from '../iconspacer.module';

@NgModule({
  imports: [CommonModule, RouterModule,  IconSpacerModule, ReactiveFormsModule,FormsModule,],
  exports: [FooterComponent, HeaderComponent, SidebarComponent],
  declarations: [FooterComponent, HeaderComponent, SidebarComponent],
})
export class LayoutModule {}
