import { TransactionDetail } from './../../models/transactiondetail';
import { PaginatedResult, Pagination } from './../../models/pagination';
import { Shopstock } from './../../models/shopstock';
import { AfterViewInit, Component, EventEmitter, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ShopstockService } from 'src/app/_services/shopstock.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  templateUrl: './stocklist.component.html',
  styleUrls: ['./stocklist.component.css']
})
export class StocklistComponent implements OnInit, AfterViewInit,OnDestroy {
  public maxSize: number = 5;
  isLoadingResults: boolean;
  isRateLimitReached: boolean;
  shopstocks: TransactionDetail[];
  pagination = {
    currentPage: 0,
    itemsPerPage: 0,
    totalItems: 0,
    totalPages: 0
  };
  displayedColumns: string[] = [
    'transactionDetailId',
    'id',
    'barcode',
    "product",
    "productExpireDate",
    "dDocDate",
    "cExtRefNum",
    "dNetQuantity",
    "dIssueQuantity",
    "dCostPrice",
    "dSalePrice",
    "dValue"
  ];
  listData: MatTableDataSource<any>;
  // @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @Output() onPageSwitch = new EventEmitter();
  private destroyed$ = new Subject();

  constructor(private shopstockService: ShopstockService,
    private alerifyService: AlertifyService,
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit() {
    this.isLoadingResults = true;
    this.isRateLimitReached = false;
    this.route.data.pipe(
      takeUntil(this.destroyed$)
    ).subscribe({
      next: data => {
      this.shopstocks = data.shopstocks.result;
      this.pagination = data.shopstocks.pagination;
      this.listData = new MatTableDataSource(this.shopstocks);
      this.listData.paginator = this.paginator;
      this.isLoadingResults = false;
    },
    error: (err) => {

    },
    complete: () => {

    },
  });
    console.log(this.shopstocks)
  }
  ngAfterViewInit() {
    this.listData.paginator = this.paginator;
  }
  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.listData.filter = filterValue.trim().toLowerCase();

    if (this.listData.paginator) {
      this.listData.paginator.firstPage();
    }
  }
  change(pageEvent: PageEvent) {
    this.paginator.pageIndex = pageEvent.pageIndex + 1;
    this.paginator.pageSize = pageEvent.pageSize;
    this.paginator.length = pageEvent.length;
  }
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.shopstockService.getStockDetails(true, this.pagination.currentPage, this.pagination.itemsPerPage).pipe(
      takeUntil(this.destroyed$)
    ).subscribe({
      next: (res: PaginatedResult<TransactionDetail[]>) => {
        this.shopstocks = res.result;
        this.pagination = res.pagination;
        this.listData = new MatTableDataSource(this.shopstocks);
        this.listData.paginator = this.paginator;
        this.isLoadingResults = false;
        console.log(res);
      },
      error: (error) => {
        this.alerifyService.error(error);
        this.router.navigate(['/stock/stocklist']);
        console.log(error);
      },
      complete:() => {
        this.router.navigate(['/stock/stocklist']);
      }}

    );
  }
}
