import { Data } from './../../models/data';
import { PaginatedResult, Pagination } from './../../models/pagination';
import { ActivatedRoute, Router } from '@angular/router';
import { SupplierService } from 'src/app/_services/supplier.service';
import { Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Supplier } from 'src/app/models/supplier';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AfterViewInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';


@Component({
  templateUrl: './supplierlist.component.html',
  styleUrls: ['./supplierlist.component.css'],
})
export class SupplierlistComponent implements OnInit, AfterViewInit {
  public maxSize: number = 5;
  status = true;
  pageNumber = 1;
  pageSize = 5;
  isLoadingResults: boolean;
  isRateLimitReached: boolean;
  suppliers: Supplier[];
  pagination = {
    currentPage: 0,
    itemsPerPage: 0,
    totalItems: 0,
    totalPages: 0
  };
  displayedColumns: string[] = [
    'supplierId',
    'id',
    'supplierName',
    'address1',
    'email',
    'phone',
    'fax',
    'action'
  ];
  listData: MatTableDataSource<any>;
  // @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @Output() onPageSwitch = new EventEmitter();
  private destroyed$ = new Subject();

  constructor(
    private supplierService: SupplierService,
    private alerifyService: AlertifyService,
    private route: ActivatedRoute,
    private router: Router,
    private toastrService: ToastrService,
  ) { }
  ngOnInit() {
    this.isLoadingResults = true;
    this.isRateLimitReached = false;
    this.initSupplier();

  }
  initSupplier() {
    this.route.data.pipe(
      takeUntil(this.destroyed$)
    ).subscribe(data => {
      this.suppliers = data.suppliers.result;
      this.pagination = data.suppliers.pagination;
      this.listData = new MatTableDataSource(this.suppliers);
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
    this.supplierService.getSuppliers(this.status, this.pageNumber, this.pageSize, filterValue).pipe(
      takeUntil(this.destroyed$)
    ).subscribe(
      (res: PaginatedResult<Supplier[]>) => {
        this.suppliers = res.result;
        this.pagination = res.pagination;
        this.listData = new MatTableDataSource(this.suppliers);
        this.listData.paginator = this.paginator;
        this.isLoadingResults = false;
        console.log(res);
      },
      error => {
        this.alerifyService.error(error);
        this.router.navigate(['/supplier/supplierlist']);
        console.log(error);
      },
      () => {
        this.router.navigate(['/supplier/supplierlist']);
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
    this.supplierService.getSuppliers(true, this.pagination.currentPage, this.pagination.itemsPerPage, "").pipe(
      takeUntil(this.destroyed$)
    ).subscribe(
      (res: PaginatedResult<Supplier[]>) => {
        this.suppliers = res.result;
        this.pagination = res.pagination;
        this.listData = new MatTableDataSource(this.suppliers);
        this.listData.paginator = this.paginator;
        this.isLoadingResults = false;
        console.log(res);
      },
      error => {
        this.alerifyService.error(error);
        this.router.navigate(['/supplier/supplierlist']);
        console.log(error);
      },
      () => {
        this.router.navigate(['/supplier/supplierlist']);
      }

    );
  }
  remove(id: any) {
    this.alerifyService.confirm("Please Confirm.", "Are you sure, you want to delete this supplier?", () => {
      this.supplierService.deletesupplier(id).pipe(
        takeUntil(this.destroyed$)
      ).subscribe(
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
  }
}
