import { Pagination, PaginatedResult } from './../../models/pagination';
import {
  AfterViewInit,
  Component,
  EventEmitter,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { CustomermasterService } from 'src/app/_services/customermaster.service';
import { CustomerMasterSetting } from 'src/app/models/customermastersetting';
@Component({
  selector: 'app-customermastersettinglist',
  templateUrl: './customermastersettinglist.component.html',
  styleUrls: ['./customermastersettinglist.component.css'],
})
export class CustomermastersettinglistComponent implements OnInit {
  public maxSize: number = 5;
  status = true;
  pageNumber = 1;
  pageSize = 5;
  isLoadingResults: boolean;
  isRateLimitReached: boolean;
  customerMasterSettings: CustomerMasterSetting[];
  customerMasterSetting: string;
  pagination = {
    currentPage: 0,
    itemsPerPage: 0,
    totalItems: 0,
    totalPages: 0,
  };
  displayedColumns: string[] = [
    'customerMasterSettingId',
    'id',
    'customerMasterSettingDescription',
    'branchCode',
    'customerCategory',
    'billingCycleStartDate',
    'billingCycleEndDate',
    'creditHoldingDays',
    'creaditBalancePerDays',
    'createDate',
    'action',
  ];
  listData: MatTableDataSource<any>;
  // @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @Output() onPageSwitch = new EventEmitter();
  constructor(
    private customermasterService: CustomermasterService,
    private alerifyService: AlertifyService,
    private route: ActivatedRoute,
    private router: Router
  ) {}
  ngOnInit() {
    this.isLoadingResults = true;
    this.isRateLimitReached = false;
    this.route.data.subscribe((data) => {
      console.log('customer master list data');
      console.log(data);
      this.customerMasterSettings = data.customerMasterlist.result;
      this.pagination = data.customerMasterlist.pagination;
      this.listData = new MatTableDataSource(this.customerMasterSettings);
      this.listData.paginator = this.paginator;
      this.isLoadingResults = false;
    });
  }
  ngAfterViewInit() {
    this.listData.paginator = this.paginator;
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.customermasterService
      .getAllCustomerMasterSetting(this.status, this.pageNumber, this.pageSize, this.customerMasterSetting)
      .subscribe(
        (res: PaginatedResult<CustomerMasterSetting[]>) => {
          this.customerMasterSettings = res.result;
          this.pagination = res.pagination;
          this.listData = new MatTableDataSource(this.customerMasterSettings);
          this.listData.paginator = this.paginator;
          this.isLoadingResults = false;
          console.log(res);
        },
        (error) => {
          this.alerifyService.error(error);
          this.router.navigate(['/setting/customermasterlist']);
          console.log(error);
        },
        () => {
          this.router.navigate(['/setting/customermasterlist']);
        }
      );
    this.listData.filter = filterValue.toLocaleLowerCase();
    /*     const filterValue = (event.target as HTMLInputElement).value;
        this.listData.filter = filterValue.trim().toLowerCase();

        if (this.listData.paginator) {
          this.listData.paginator.firstPage();
        } */
  }
  change(pageEvent: PageEvent) {
    this.paginator.pageIndex = pageEvent.pageIndex + 1;
    this.paginator.pageSize = pageEvent.pageSize;
    this.paginator.length = pageEvent.length;
  }
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.customermasterService
      .getAllCustomerMasterSetting(
        true,
        this.pagination.currentPage,
        this.pagination.itemsPerPage,
        ''
      )
      .subscribe(
        (res: PaginatedResult<CustomerMasterSetting[]>) => {
          this.customerMasterSettings = res.result;
          this.pagination = res.pagination;
          this.listData = new MatTableDataSource(this.customerMasterSettings);
          this.listData.paginator = this.paginator;
          this.isLoadingResults = false;
          console.log(res);
        },
        (error) => {
          this.alerifyService.error(error);
          this.router.navigate(['/setting/customermasterlist']);
          console.log(error);
        },
        () => {
          this.router.navigate(['/setting/customermasterlist']);
        }
      );
  }
}
