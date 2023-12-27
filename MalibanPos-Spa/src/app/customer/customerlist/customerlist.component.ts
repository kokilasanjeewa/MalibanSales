import { AfterViewInit, Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { MI_Employer_Employee } from 'src/app/models/mI_employer_employee';
import { PaginatedResult } from 'src/app/models/pagination';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { CustomerService } from 'src/app/_services/customer.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-customerlist',
  templateUrl: './customerlist.component.html',
  styleUrls: ['./customerlist.component.css']
})
export class CustomerlistComponent implements OnInit, AfterViewInit {
  public maxSize: number = 5;
  status = true;
  pageNumber = 1;
  pageSize = 5;
  isLoadingResults: boolean;
  isRateLimitReached: boolean;
  customers: MI_Employer_Employee[];
  pagination = {
    currentPage: 0,
    itemsPerPage: 0,
    totalItems: 0,
    totalPages: 0
  };
   displayedColumns: string[] = [
    'mI_Employer_EmployeeId',
    'id',
    'number',
    'epfNo',
    'fullName',
    'scheme',
    'creditLimit',
    'creditBalance',
    'location',
    'deleted',
    'action',
  ];
  listData: MatTableDataSource<any>;
  // @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @Output() onPageSwitch = new EventEmitter();
  private destroyed$ = new Subject();

  constructor(
    private customerService: CustomerService,
    private alerifyService: AlertifyService,
    private route: ActivatedRoute,
    private router: Router) {
  }
  ngOnInit() {
    this.isLoadingResults = true;
    this.isRateLimitReached = false;
    this.route.data.pipe(
      takeUntil(this.destroyed$)
    ).subscribe(data => {
      console.log("product list data")
      console.log(data)
      this.customers = data.customers.result;
      this.pagination = data.customers.pagination;
      this.listData = new MatTableDataSource(this.customers);
      this.listData.paginator = this.paginator;
      this.isLoadingResults = false;
    });
  }
  ngAfterViewInit() {
    this.listData.paginator = this.paginator;
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.customerService.getCustomers(this.pageNumber, this.pageSize, filterValue).pipe(
      takeUntil(this.destroyed$)
    ).subscribe(
      (res: PaginatedResult<MI_Employer_Employee[]>) => {
        this.customers = res.result;
        this.pagination = res.pagination;
        this.listData = new MatTableDataSource(this.customers);
        this.listData.paginator = this.paginator;
        this.isLoadingResults = false;
        console.log(res);
      },
      error => {
        this.alerifyService.error(error);
        this.router.navigate(['/customer/customerlist']);
        console.log(error);
      },
      () => {
        this.router.navigate(['/customer/customerlist']);
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
    this.customerService.getCustomers(this.pagination.currentPage, this.pagination.itemsPerPage, "").pipe(
      takeUntil(this.destroyed$)
    ).subscribe(
      (res: PaginatedResult<MI_Employer_Employee[]>) => {
        this.customers = res.result;
        this.pagination = res.pagination;
        this.listData = new MatTableDataSource(this.customers);
        this.listData.paginator = this.paginator;
        this.isLoadingResults = false;
        console.log(res);
      },
      error => {
        this.alerifyService.error(error);
        this.router.navigate(['/customer/customerlist']);
        console.log(error);
      },
      () => {
        this.router.navigate(['/customer/customerlist']);
      }

    );
  }
}
