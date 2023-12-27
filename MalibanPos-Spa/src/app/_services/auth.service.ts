import { JwtHelperService } from '@auth0/angular-jwt';
import { element } from 'protractor';
//https://dev.to/thisdotmedia/angular-and-the-rest-authentication-with-jwt-5em0

import { environment } from '../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router';
import { AlertifyService } from './alertify.service';
import { EmailValidator } from '@angular/forms';
import { Root } from '../models/root';
import { Permission } from '../models/permission';
import { Claim } from '../models/claim';
@Injectable({
  providedIn: 'root',
})
export class AuthService {
  baseUrl = environment.apiUrl + 'auth/';
  changePasswordUrl = 'http://localhost:4200/authentication/resetpassword';
  fetchUsersUrl = this.baseUrl + 'users';
  jwtHelper = new JwtHelperService();
  decodedToken: any;
  root: Root;
  public permissions: Permission[];
  constructor(
    private http: HttpClient,
    private alertifyService: AlertifyService,
    private router: Router
  ) { }

  login(model: any) {
    return this.http.post(this.baseUrl + 'login', model).pipe(
      map((response: any) => {
        const user = response;
        if (user) {
          localStorage.setItem('token', user.token);
          this.decodedToken = this.jwtHelper.decodeToken(user.token);
          this.alertifyService.success('logging in successfully');
        }
      })
    );
  }
  loggedIn() {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
    // return !!token;
  }
  logOut() {
    localStorage.removeItem('token');
    localStorage.removeItem('branchLocationId');
    localStorage.removeItem('postData');
    this.router.navigate(['']);
    this.alertifyService.message('logging out successfully');
  }
  register(model: any) {
    return this.http.post(this.baseUrl + 'register', model);
  }
  resetPassword(model: any) {
    // let options = new RequestOptions({ headers: headers });
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      changePasswordUrl: this.changePasswordUrl,
    });
    //  const options = { headers: headers };
    const options = { headers };
    return this.http.post(this.baseUrl + 'resetpassword', model, options);
  }
  changePassword(model: any) {
    return this.http.post(this.baseUrl + 'changepassword', model);
  }
  sendConfirmEmail(model: any) {
    return this.http.post(this.baseUrl + 'sendconfirmemail', model);
  }
  createRole(model: any,userId: number) {
    return this.http.post(this.baseUrl + 'create-roles/'+userId, model);
  }

  confirmEmail(model: any) {
    return this.http.post(this.baseUrl + 'confirmemail', model);
  }
  verifyEmail(email: any) {
    return this.http.get(this.baseUrl + 'verifyemail', email);
  }
  userwithroles(userId: number):any {
    return this.http.get(this.baseUrl + 'user-with-roles/'+userId);
  }
  roleMatch(allowedRoles: string[]): boolean {
    let isMatch = false;
    const role = this.jwtHelper.decodeToken(localStorage.getItem('token'));
    const userRoles =role.role;
    allowedRoles.forEach(element => {
      if (userRoles.includes(element)) {
        isMatch = true;
        return;
      }
    });
    return isMatch;
  }
  rolePermissionMatch(accessControllerName: string[]): Claim[] {
    const role = this.jwtHelper.decodeToken(localStorage.getItem('token'));
    console.log(role);
    const permissions=JSON.parse(role.rolePermission) as Permission[];
    console.log(permissions);
    let claims: Claim[] = [];
    accessControllerName.forEach(element => {
      let permission = permissions.find((e: Permission) => {return e.AccessControllerName == element} );
      console.log('permission')
      console.log(permission?.Allow); // prints values: 10, 20, 30, 40
       let claim: Claim = {
        accessControllerName: element,
        allow: permission?.Allow
      };
      claims.push(claim);
    })
    return claims;
  }
  rolePermission(accessControllerName: string): boolean {
    const role = this.jwtHelper.decodeToken(localStorage.getItem('token'));
    console.log(role);
    const permissions=JSON.parse(role.rolePermission) as Permission[];
    console.log(permissions);

      let permission = permissions.find((e: Permission) => {return e.AccessControllerName == accessControllerName} );
      console.log('rolePermission')
      console.log(permission?.Allow); // prints values: 10, 20, 30, 40
      if(permission){
        return permission?.Allow;
      }
    return false;
  }
}
