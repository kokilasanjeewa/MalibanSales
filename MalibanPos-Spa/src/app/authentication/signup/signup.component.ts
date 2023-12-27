import { Branch } from './../../models/branch';
import { CommonService } from 'src/app/_services/common.service';
import { AlertifyService } from './../../_services/alertify.service';
import { AuthService } from './../../_services/auth.service';
import { Component, OnInit } from '@angular/core';
import {
  FormGroup,
  FormControl,
  Validators,
  FormBuilder,
  ValidatorFn,
  AbstractControl,
} from '@angular/forms';
import { CompileShallowModuleMetadata } from '@angular/compiler';
import { HttpParams, HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';

@Component({
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css'],
})
export class SignupComponent implements OnInit {
  model: any = {};
  registerForm: FormGroup;
  branches: {};
  branches$: Observable<Branch[]>;

  baseUrl = environment.apiUrl + 'BranchLocation/getallbranchlocations';
  constructor(
    private authService: AuthService,
    private alertifyService: AlertifyService,
    private fb: FormBuilder,
    private httpClient: HttpClient,
    private router: Router,
    private commonService: CommonService
  ) {
    // this.sendGETRequestWithParameters();
    of(
      this.branches$ = this.commonService
        .getList('Common', 'getallbranch')
    );
  }
  ngOnInit() {
    this.createRegisterForm();
  }
  createRegisterForm() {
    this.registerForm = this.fb.group(
      {
        firstName: ['', [Validators.required]],
        lastName: ['', [Validators.required]],
        email: ['', [Validators.required, Validators.email]],
        password: [
          '',
          [
            Validators.required,
            Validators.minLength(6),
            Validators.maxLength(20),
          ],
        ],
        confirmPassword: ['', Validators.required],
        branchLocationId: ['null'],
      },
      { validator: [this.passwordMatchvalidator] }
    );
  }
  passwordMatchvalidator(g: FormGroup) {
    return g.get('password').value === g.get('confirmPassword').value
      ? null
      : { mismatch: true };
  }
  get f() {
    return this.registerForm.controls;
  }
  signup() {
    //https://developer.mozilla.org/en-US/docs/Web/API/FormData
    const formData: any = new FormData();
    formData.append('firstName', this.registerForm.get('firstName').value);
    formData.append('lastName', this.registerForm.get('lastName').value);
    formData.append('email', this.registerForm.get('email').value);
    formData.append(
      'branchLocationId',
      this.registerForm.get('branchLocationId').value
    );
    formData.append('password', this.registerForm.get('password').value);
    console.log(formData);
    console.log(this.registerForm.get('branchLocationId').value);
    this.authService.register(formData).subscribe(
      (next) => {},
      (error) => {
        this.alertifyService.error(
          JSON.stringify(error).replace('"', '').replace('"', '')
        );
        console.log(error.error);
      },
      () => {
        this.router.navigate(['/authentication/signin'], {
          queryParams: { username: this.registerForm.get('email').value },
        });
      }
    );
  }
  /*public sendGETRequestWithParameters() {
    const opts = { params: new HttpParams({ fromString: 'status=true' }) };
    return this.httpClient.get(this.baseUrl, opts).subscribe((data) => {
      this.branches = data;
      console.log(data);
    });
  }*/
  /* public sendGETRequestWithParameters(){
  let params = new HttpParams();
  params = params.append('_page', 1);
  params = params.append('_limit', 10);

  return this.httpClient.get("http://server.com/api/products", {params: params});
 } */
}
