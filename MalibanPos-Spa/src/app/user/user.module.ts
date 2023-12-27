import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserRoutingModule } from './user-routing.module';
import { UsermanagementComponent } from './usermanagement/usermanagement.component';
import { SharedModule } from '../shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ManageuserComponent } from './manageuser/manageuser.component';
import { UserpermissionmodalComponent } from './userpermissionmodal/userpermissionmodal.component';
import { MaterialModule } from '../material.module';
import { NgSelectModule } from '@ng-select/ng-select';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { IconSpacerModule } from '../iconspacer.module';
import { AdduserComponent } from './adduser/adduser.component';
import { EdituserComponent } from './edituser/edituser.component';
import { AddroleComponent } from './addrole/addrole.component';
import { FilterPipe } from '../_custompipe/filter.pipe';
import { UsermapComponent } from './usermap/usermap.component';


@NgModule({
  declarations: [UsermanagementComponent, ManageuserComponent,UserpermissionmodalComponent, AdduserComponent, EdituserComponent, AddroleComponent, UsermapComponent],
  imports: [
    CommonModule,
    UserRoutingModule,
    MaterialModule,
    ReactiveFormsModule,
    SharedModule,
    IconSpacerModule,
    FormsModule,
    PaginationModule.forRoot(),
    NgSelectModule,

  ]
})
export class UserModule { }
