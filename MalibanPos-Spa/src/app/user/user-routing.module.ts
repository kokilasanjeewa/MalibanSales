import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UserManagementListResolver } from '../_resolvers/usermanagementlist.resolver';
import { AddroleComponent } from './addrole/addrole.component';
import { AdduserComponent } from './adduser/adduser.component';
import { ManageuserComponent } from './manageuser/manageuser.component';
import { UsermanagementComponent } from './usermanagement/usermanagement.component';
import { UsermapComponent } from './usermap/usermap.component';


const routes: Routes = [
  {
    path: 'userpermissions/:userId',
    component: UsermanagementComponent,
  },
  {
    path: 'mapuser',
    component: UsermapComponent,
  },
  {
    path: 'manageuser',
    component: ManageuserComponent,
    resolve: { users: UserManagementListResolver }

  }
  ,{
    path: 'adduser',
    component: AdduserComponent
  }
  ,{
    path: 'addrole',
    component: AddroleComponent
  }
  /* ,
 {
    path: '**',
    component:UsermanagementComponent,
    pathMatch: 'full'
  } */
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }
