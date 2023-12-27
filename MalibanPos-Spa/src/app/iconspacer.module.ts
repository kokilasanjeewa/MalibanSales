import { NgModule } from '@angular/core';
import { FilterPipe } from './_custompipe/filter.pipe';
import { HasClaimDirective } from './_directives/has-claim.directive';
import { HasRoleDirective } from './_directives/has-role.directive';

@NgModule({
  declarations: [HasRoleDirective,HasClaimDirective,FilterPipe],
  exports: [
    HasRoleDirective,
    HasClaimDirective,
    FilterPipe,

  ],
})
export class IconSpacerModule  {}
