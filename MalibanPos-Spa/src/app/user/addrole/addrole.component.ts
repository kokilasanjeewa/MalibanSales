import { MenuService } from './../../_services/menu.service';
import { CommonService } from './../../_services/common.service';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { Observable, Subject } from 'rxjs';
import { AuthService } from 'src/app/_services/auth.service';
@Component({
  selector: 'app-addrole',
  templateUrl: './addrole.component.html',
  styleUrls: ['./addrole.component.css']
})
export class AddroleComponent implements OnInit {
  initialState: any;
  roles: Observable<[]>;
  userswithroles: Observable<[]>;
  canAccessPermissionForm: FormGroup;
  @Output() updateItemAdd = new EventEmitter();
  public onClose: Subject<boolean>;

  constructor(
    public bsModalRef: BsModalRef,
    public modalService: BsModalService,
    public commonService: CommonService,
    private menuService: MenuService,
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private authService: AuthService,
    private alertifyService: AlertifyService
  ) {
    this.initialState =this.modalService.config.initialState;
    console.log("user id testing");
    console.log(this.initialState.id);
    this.roles = this.commonService.getListbyCode('Common', 'getallrole',this.commonService.getUserId());
    this.userswithroles = this.authService.userwithroles(this.initialState.id);
    console.log('userswithroles');
    console.log(this.userswithroles);
  }


  ngOnInit() {
    this.onClose = new Subject();
    this.createCanAccessPermissionForm();
    console.log(this.commonService.getUserRoles());
  }
  get f() {
    return this.canAccessPermissionForm.controls;
  }
  createCanAccessPermissionForm() {
    this.canAccessPermissionForm = this.fb.group({
      roleNames: [[], [Validators.required]],
     });
  }
  canAccessPermissionAdd() {
    //https://developer.mozilla.org/en-US/docs/Web/API/FormData
    const formData: any = new FormData();
    formData.append(
      'roleNames',
      this.canAccessPermissionForm.get('roleNames').value
    );
  //  console.log(formData);
   // alert(formData);
   // alert(JSON.stringify(this.canAccessPermissionForm.value));
    this.authService.createRole(formData,this.initialState.id).subscribe(
      (next) => { },
      (error) => {
        this.alertifyService.error(
          JSON.stringify(error).replace('"', '').replace('"', '')
        );
        console.log(error.error);
        this.close();
        this.onClose.next(true);
      },
      () => {
        this.close();
        this.onClose.next(true);
        this.router.navigate(['/manage/userpermissions/',this.initialState.id]);
       }
    );
  }
  close() {
    this.bsModalRef.hide();
  }
}
