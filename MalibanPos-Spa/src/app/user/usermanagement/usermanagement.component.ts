import { CommonService } from './../../_services/common.service';
import { AfterViewInit, Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { UserpermissionmodalComponent } from '../userpermissionmodal/userpermissionmodal.component';
import { ActivatedRoute, Router } from '@angular/router';
import { MenuService } from 'src/app/_services/menu.service';
import { RolePermissionCustom } from 'src/app/models/rolepermissioncustom';
import { AlertifyService } from 'src/app/_services/alertify.service';
const HEROES = [
  { id: 1, name: 'Superman' },
  { id: 2, name: 'Batman' },
  { id: 5, name: 'BatGirl' },
  { id: 3, name: 'Robin' },
  { id: 4, name: 'Flash' }
];
@Component({
  selector: 'app-usermanagement',
  templateUrl: './usermanagement.component.html',
  styleUrls: ['./usermanagement.component.css'],
})
export class UsermanagementComponent implements OnInit, AfterViewInit {
  data: any;
  bsModalRef: BsModalRef;
  rolePermissionCustom: RolePermissionCustom[];
  heroes: any;
  userId: number;


  constructor(private router: Router, private alertifyService: AlertifyService, private modalService: BsModalService, public commonService: CommonService, private route: ActivatedRoute, private menuService: MenuService
  ) {
    this.heroes = HEROES;
  }

  ngOnInit() {
    this.data = ['Home', '/usermanagement'];
    this.userId = +this.route.snapshot.params.userId || 0;
    this.menuService.getMenuId(this.userId).subscribe((data) => {
      this.rolePermissionCustom = data;
      console.log(data);
    });
  }
  remove(Id: number) {
    this.alertifyService.confirm("Please Confirm.", "Are you sure you want to delete this line?", () => {
      this.menuService.deletePermission(Id)
        .subscribe((data) => {
          console.log(data);
          this.alertifyService.success("Deleted successfully.");
        }, (error) => {
          console.log(error);
          this.alertifyService.error(error);
        }, () => {
          this.menuService.getMenuId(this.userId).subscribe((data) => {
            this.rolePermissionCustom = data;
            console.log(data);
          });
        });
    },
      () => {
        this.menuService.getMenuId(this.userId).subscribe((data) => {
          this.rolePermissionCustom = data;
          console.log(data);
        });
      });
  }
  ngAfterViewInit() {
    this.menuService.getMenuId(this.userId).subscribe((data) => {
      this.rolePermissionCustom = data;
      console.log(data);
    });
  }
  openModalWithComponent() {
    const initialState = {
      userId: this.userId
    };
    this.bsModalRef = this.modalService.show(UserpermissionmodalComponent, { initialState, class: 'gray' });
    this.bsModalRef.content.onClose.subscribe(result => {
      if (result) {
        this.menuService.getMenuId(this.userId).subscribe((data) => {
          this.rolePermissionCustom = data;
          console.log(data);
        });
      }
      console.log('results', result);
    });
  }
  changePermission(isChecked: boolean, accessControllerName: string) {
    console.log(isChecked);
    const formData: any = new FormData();
    formData.append(
      'allow',
      isChecked
    );
    formData.append('accessControllerName', accessControllerName);
    formData.append(
      'branchLocationId',
      this.commonService.getUserBranchLocationId()
    );
    formData.append(
      'applicationUserId',
      this.userId
    );
    formData.append(
      'role',
      this.commonService.getUserRoles()
    );
    this.menuService.updateCompany(formData).subscribe(
      (next) => { },
      (error) => {
        this.alertifyService.error(
          JSON.stringify(error).replace('"', '').replace('"', '')
        );
        console.log(error.error);
      },
      () => {
        this.router.navigate(['/manage/userpermissions/', this.userId || 0]);
      }
    );
    if (isChecked) {
      console.log(isChecked);
      console.log(accessControllerName);
    }
    else {
      console.log(isChecked);
    }
  }

}
