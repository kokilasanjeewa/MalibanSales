import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Observable, of } from 'rxjs';
import { Branch } from 'src/app/models/branch';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import { BranchlocationService } from 'src/app/_services/branchlocation.service';
import { CommonService } from 'src/app/_services/common.service';
import { MenuService } from 'src/app/_services/menu.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-adduser',
  templateUrl: './adduser.component.html',
  styleUrls: ['./adduser.component.css']
})
export class AdduserComponent implements OnInit {
  listMenu: any;
  @Output() updateItemAdd = new EventEmitter();
  model: any = {};
  registerForm: FormGroup;
  branches: {};
  branches$: Observable<Branch[]>;
  branchCode: string;
  baseUrl = environment.apiUrl + 'BranchLocation/getallbranchlocations';
  constructor(
    public bsModalRef: BsModalRef,
    public modalService: BsModalService,
    public commonService: CommonService,
    private menuService: MenuService,
    private fb: FormBuilder,
    private authService: AuthService,
    private alertifyService: AlertifyService,
    private httpClient: HttpClient,
    private branchlocationService: BranchlocationService,
    private router: Router,

  ) {
    //this.list =this.modalService.config.initialState;
    //this.listMenu =this.list.menu;
    console.log(this.listMenu);
    of(
      this.branches$ = this.commonService
        .getList('Common', 'getallbranch')
    );
  }
  ngOnInit() {
    this.createRegisterForm();
    this.commonService.getList('Menu', 'get/menu').subscribe((data) => {
      // this.sections = data.filter((data) => { return data.idProcess === Number(id) })
      this.listMenu = data;
      console.log(data);
    });
    this.branchlocationService.getBranchLocatio(this.commonService.getUserBranchLocationId())
    .subscribe((data:any) => {
      this.branchCode=data.branchCode;
    });
  }
  close() {
    this.bsModalRef.hide();
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
        branchLocationId: ['',Validators.required],
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
        this.close();
        this.router.navigate(['/manage/manageuser']);
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
