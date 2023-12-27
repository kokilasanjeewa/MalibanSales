import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, CanActivateChild, CanLoad, Route, Router, RouterStateSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate,CanActivateChild,CanLoad {
  claims: Array<string>;
  active: boolean ;
  urls: Array<string>;
  r:string;
  constructor(
    private authService: AuthService,
    private alertifyService: AlertifyService,
    private router: Router
  ) { }
  canActivate(next: ActivatedRouteSnapshot,state: RouterStateSnapshot): boolean {
    console.log('state data here');
    console.log(state);
     // use of String split() Method
     this.urls = state.url.split('/');
     console.log(this.urls);
    const roles = next.firstChild?.data['roles'] as Array<string>;
    this.claims = next?.data['claims'] as Array<string>;
    //this.claims = claims;
   console.log('here is a can activate claimns test');
    console.log(this.claims);
    if (roles) {
      const match = this.authService.roleMatch(roles);
      if (match) {
        return true;
      } else {
        this.router.navigate(['/']);
        this.alertifyService.error('you are not authorized to access this area.')
      }
    }
    this.claims?.forEach(element => {
      this.r =element.toLowerCase().replace('canaccess','')
      var result=this.urls.filter(url => url===this.r);
      console.log(result);
      if(result[0]){
        console.log(result);
        const match = this.authService.rolePermission(element);
        console.log(element)
        if (match) {
          return this.active=true;
        }
        if (!match) {
          this.router.navigate(['/']);
          this.alertifyService.error('you are not authorized to access this area.')
          return this.active=false;;
        }
      }
    });
    if (this.authService.loggedIn()) {
      return true;
    }
    this.alertifyService.error('You shall not pass!!!');
    this.router.navigate(['']);
    return false;
  }
  canActivateChild(next: ActivatedRouteSnapshot): boolean{
  const claims = next.data['claims'] as Array<string>;
    console.log(claims);
    console.log('here is a claimns test');
    //const match = this.authService.rolePermission(this.claims)
    this.claims.forEach(element => {
      const match = this.authService.rolePermission(element);
      console.log(element)
      if (match) {
        return this.active=true;
      }
      if (!match) {
        this.router.navigate(['/']);
        this.alertifyService.error('you are not authorized to access this area.')
        return this.active=false;;
      }
    });
    console.log(this.claims);
   //this.alertifyService.error('You shall not pass!!!');
   //this.router.navigate(['']);
   return this.active;
  }
  canLoad(route: Route): boolean{
    const url = route.path;
    console.log('Url:' + url);
    this.router.navigate([url,'supplierpricelist'])
    return false;

  }
}
