import { AlertifyService } from 'src/app/_services/alertify.service';
import { UserService } from './../../_services/user.service';
import { PaginatedResult } from './../../models/pagination';
import { MatTableDataSource } from '@angular/material/table';
import { ApplicationUser } from './../../models/applicationuser';
import { AfterViewInit, Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { MainMenu } from 'src/app/models/mainmenu';
import { AdduserComponent } from '../adduser/adduser.component';
import { EdituserComponent } from '../edituser/edituser.component';
import { AddroleComponent } from '../addrole/addrole.component';
import { CommonService } from 'src/app/_services/common.service';

@Component({
  selector: 'app-manageuser',
  templateUrl: './manageuser.component.html',
  styleUrls: ['./manageuser.component.css']
})
export class ManageuserComponent implements OnInit,AfterViewInit{
  public maxSize: number = 5;
  status = true;
  pageNumber = 1;
  pageSize = 5;
  isLoadingResults: boolean;
  isRateLimitReached: boolean;
  users: ApplicationUser[];
  pagination = {
    currentPage: 0,
    itemsPerPage: 0,
    totalItems: 0,
    totalPages: 0
  };
  displayedColumns: string[] = [
    'userId',
    'id',
    'firstName',
    'lastName',
    'email',
    'userName',
    'password',
    'phoneNumber',
    'regDate',
     'action'
  ];
  listData: MatTableDataSource<any>;
  // @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @Output() onPageSwitch = new EventEmitter();
  bsModalRef: BsModalRef;
  mainMenu1 : MainMenu[];
  constructor(
    private userService: UserService,
    private alerifyService: AlertifyService,
    private route: ActivatedRoute,
    private router: Router,
    private toastrService: ToastrService,
    private modalService: BsModalService,
    private commonService: CommonService,
  ) { }
  ngOnInit() {
    this.isLoadingResults = true;
    this.isRateLimitReached = false;
    this.initSupplier();

  }
  initSupplier() {
    this.route.data.subscribe(data => {
      this.users = data.users.result;
      this.pagination = data.users.pagination;
      this.listData = new MatTableDataSource(this.users);
      this.listData.paginator = this.paginator;
      this.isLoadingResults = false;
    });
  }
  ngAfterViewInit() {
    this.initSupplier();
    this.listData.paginator = this.paginator;
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.userService.getUsers(this.status, this.pageNumber, this.pageSize,'').subscribe(
      (res: PaginatedResult<ApplicationUser[]>) => {
        this.users = res.result;
        this.pagination = res.pagination;
        this.listData = new MatTableDataSource(this.users);
        this.listData.paginator = this.paginator;
        this.isLoadingResults = false;
        console.log(res);
      },
      error => {
        this.alerifyService.error(error);
        this.router.navigate(['/manage/manageuser']);
        console.log(error);
      },
      () => {
        this.router.navigate(['/manage/manageuser']);
      }
    );
    this.listData.filter = filterValue.toLocaleLowerCase();
  }
  change(pageEvent: PageEvent) {
    this.paginator.pageIndex = pageEvent.pageIndex + 1;
    this.paginator.pageSize = pageEvent.pageSize;
    this.paginator.length = pageEvent.length;
  }
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.userService.getUsers(true, this.pagination.currentPage, this.pagination.itemsPerPage, "",this.commonService.getUserId()).subscribe(
      (res: PaginatedResult<ApplicationUser[]>) => {
        this.users = res.result;
        this.pagination = res.pagination;
        this.listData = new MatTableDataSource(this.users);
        this.listData.paginator = this.paginator;
        this.isLoadingResults = false;
        console.log(res);
      },
      error => {
        this.alerifyService.error(error);
        this.router.navigate(['/manage/manageuser']);
        console.log(error);
      },
      () => {
        this.router.navigate(['/manage/manageuser']);
      }

    );
  }
  openModalWithComponent(menu :MainMenu[]) {
    const initialState = {
       menu: this.mainMenu1
    };
    this.bsModalRef = this.modalService.show(AdduserComponent, { initialState, class: 'gray' });
    //this.bsModalRef.content.closeBtnName = 'Close';
 /*    this.bsModalRef.content.updateStyleAdd.subscribe((data) => {
      this.style = data;
      this.addSmvForm.patchValue({ styleId: this.style.idStyle });
      console.log(this.style);
    });*/
  }
  openModalWithComponentEdit(id :number) {
    const initialState = {
       id: id
    };
    this.bsModalRef = this.modalService.show(EdituserComponent, { initialState, class: 'gray' });
    //this.bsModalRef.content.closeBtnName = 'Close';
 /*    this.bsModalRef.content.updateStyleAdd.subscribe((data) => {
      this.style = data;
      this.addSmvForm.patchValue({ styleId: this.style.idStyle });
      console.log(this.style);
    });*/
  }
  openModalWithComponentAddRole(id :number) {
    const initialState = {
       id: id
    };
    this.bsModalRef = this.modalService.show(AddroleComponent, { initialState, class: 'gray' });
    //this.bsModalRef.content.closeBtnName = 'Close';
 /*    this.bsModalRef.content.updateStyleAdd.subscribe((data) => {
      this.style = data;
      this.addSmvForm.patchValue({ styleId: this.style.idStyle });
      console.log(this.style);
    });*/
  }
  /* remove(id: any) {
    this.alerifyService.confirm("Please Confirm.", "Are you sure, you want to delete this supplier?", () => {
      this.userService.deletesupplier(id).subscribe(
        (data: any) => {
          console.log(data);
          this.toastrService.success("Deleted successfully.");
          this.initSupplier();
        }, error => {
          console.log(error);
          this.toastrService.error(error);
        }
      );
    }, () => {
      // type your execution here
      this.router.navigate(['/supplier/supplierlist']);
      this.initSupplier();

    });
  } */
}
