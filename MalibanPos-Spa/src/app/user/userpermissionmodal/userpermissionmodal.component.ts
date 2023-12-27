import { MenuService } from './../../_services/menu.service';
import { CommonService } from './../../_services/common.service';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { Subject } from 'rxjs';
@Component({
  selector: 'app-userpermissionmodal',
  templateUrl: './userpermissionmodal.component.html',
  styleUrls: ['./userpermissionmodal.component.css'],
})
export class UserpermissionmodalComponent implements OnInit {
  list: any;
  listMenu: any;
  initialState: any;
  listSubMenu: any;
  listSubMenuItem: any;
  applicationUserId: number;
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
    private alertifyService: AlertifyService
  ) {
    this.initialState =this.modalService.config.initialState;
    this.applicationUserId=this.initialState.userId;
    console.log(this.initialState.userId);
  }


  ngOnInit() {
    this.onClose = new Subject();
    this.createCanAccessPermissionForm();
    this.commonService.getList('Menu', 'get/menu').subscribe((data) => {
      // this.sections = data.filter((data) => { return data.idProcess === Number(id) })
      this.listMenu = data;
      console.log(data);
    });
    console.log(this.commonService.getUserRoles());
  }
  get f() {
    return this.canAccessPermissionForm.controls;
  }
  createCanAccessPermissionForm() {
    this.canAccessPermissionForm = this.fb.group({
      menuId: ['', [Validators.required]],
      submenuId: ['', [Validators.required]],
      submenuitemId: ['', [Validators.required]],
      branchLocationId: [
        this.commonService.getUserBranchLocationId(),
        [Validators.required],
      ],
    });
  }
  close() {
    this.bsModalRef.hide();
  }
  changeMenu(event: Event) {
    console.log((event.target as HTMLInputElement).value);
    const id = (event.target as HTMLInputElement).value;
    console.log(id);
    this.menuService.getMenuById('Menu', 'get/menu', Number(id)).subscribe(
      (data) => {
        this.listSubMenu = data;
        console.log(data);
      },
      (error) => {
        console.log(error);
      },
      () => { }
    );
  }
  changeSubMenu(event: Event) {
    console.log((event.target as HTMLInputElement).value);
    const id = (event.target as HTMLInputElement).value;
    console.log(id);
    this.menuService.getMenuById('Menu', 'get/submenu', Number(id)).subscribe(
      (data) => {
        this.listSubMenuItem = data;
        console.log(data);
      },
      (error) => {
        console.log(error);
      },
      () => { }
    );
  }
  canAccessPermissionAdd() {
    //https://developer.mozilla.org/en-US/docs/Web/API/FormData
    const formData: any = new FormData();
    formData.append(
      'mainMenuId',
      this.canAccessPermissionForm.get('menuId').value
    );
    formData.append(
      'submenuId',
      this.canAccessPermissionForm.get('submenuId').value
    );
    formData.append('submenuItemId', this.canAccessPermissionForm.get('submenuitemId').value);
    formData.append(
      'branchLocationId',
      this.commonService.getUserBranchLocationId()
    );
    formData.append(
      'applicationUserId',
      this.applicationUserId
    );
    formData.append(
      'role',
      this.commonService.getUserRoles()
    );
  //  console.log(formData);
   // alert(formData);
  //  alert(JSON.stringify(this.canAccessPermissionForm.value));
    console.log(this.canAccessPermissionForm.get('branchLocationId').value);
    this.menuService.createPermission(formData).subscribe(
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
        this.router.navigate(['/manage/userpermissions/',this.initialState.userId]);
       }
    );
  }
}
