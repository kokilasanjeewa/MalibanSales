import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { CommonService } from 'src/app/_services/common.service';
import { MenuService } from 'src/app/_services/menu.service';

@Component({
  selector: 'app-edituser',
  templateUrl: './edituser.component.html',
  styleUrls: ['./edituser.component.css']
})
export class EdituserComponent implements OnInit {
  list: any;
  listMenu: any;
  listSubMenu: any;
  listSubMenuItem: any;
  canAccessPermissionForm: FormGroup;
  @Output() updateItemAdd = new EventEmitter();
  constructor(
    public bsModalRef: BsModalRef,
    public modalService: BsModalService,
    public commonService: CommonService,
    private menuService: MenuService,
    private fb: FormBuilder,

  ) {
    this.list =this.modalService.config.initialState;
    //this.listMenu =this.list.menu;
    console.log(this.listMenu);
    console.log(this.list.id);

  }

  ngOnInit() {
    this.createCanAccessPermissionForm();
    this.commonService.getList('Menu', 'get/menu').subscribe((data) => {
      // this.sections = data.filter((data) => { return data.idProcess === Number(id) })
      this.listMenu = data;
      console.log(data);
    });
  }
  get f() {
    return this.canAccessPermissionForm.controls;
  }
  createCanAccessPermissionForm() {
    this.canAccessPermissionForm = this.fb.group({
      menuId: ['', [Validators.required]],
      submenuId: ['', [Validators.required]],
      submenuitemId: ['', [Validators.required]],
      branchLocationId: [this.commonService.getUserBranchLocationId(), [Validators.required]],
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
      () => {}
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
      () => {}
    );
  }
  canAccessPermissionAdd() {

  }
}
