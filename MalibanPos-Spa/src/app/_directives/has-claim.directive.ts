import { Directive, Input, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { Router } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';

@Directive({
  selector: '[appHasClaim]'
})
export class HasClaimDirective implements OnInit {
  @Input() appHasClaim: string;
  isVisible =false
  constructor(private viewContainerRef: ViewContainerRef,
    private templateRef: TemplateRef<any>,
    private authService: AuthService,
    )
    {

  }
  ngOnInit() {
    const userRoles = this.authService.decodedToken.role as Array<string>;
    console.log(userRoles);
    // if no roles clear the ViewContainerRef
    if (!userRoles) {
      this.viewContainerRef.clear();
    }
    console.log("running has claim")
    console.log(this.appHasClaim);
    // if user has role need then render the  element
    if (this.authService.rolePermission(this.appHasClaim)) {
      if (!this.isVisible) {
        this.isVisible = true;
        this.viewContainerRef.createEmbeddedView(this.templateRef);
      }
      else {
        this.isVisible = false;
        this.viewContainerRef.clear();
      }
    }
   }
}
