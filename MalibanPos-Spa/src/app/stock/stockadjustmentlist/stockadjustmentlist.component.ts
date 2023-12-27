import { StockAdjustmentHeader } from './../../models/stockadjustmentheader';
import { PaginatedResult, Pagination } from './../../models/pagination';
import { AfterViewInit, Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { StockadjustmentService } from 'src/app/_services/stockadjustment.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
@Component({
  templateUrl: './stockadjustmentlist.component.html',
  styleUrls: ['./stockadjustmentlist.component.css']
})
export class StockadjustmentlistComponent implements OnInit, AfterViewInit {
  public maxSize: number = 5;
  isLoadingResults: boolean;
  isRateLimitReached: boolean;
  shopstocks: StockAdjustmentHeader[];
  pagination = {
    currentPage: 0,
    itemsPerPage: 0,
    totalItems: 0,
    totalPages: 0
  };
  displayedColumns: string[] = [
    'stockAdjustmentHeaderId1',
    'id',
    'stockAdjustmentHeaderId',
    'stockTakenDate',
    "stockAdjustmentDate",
    "isProcess",
    "isApprove",
    "stockAdjustmentCostAmount",
    "adjustmentType",
    "createDate",
    "createdBy",
    'action',
  ];
  listData: MatTableDataSource<any>;
  // @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @Output() onPageSwitch = new EventEmitter();
  private destroyed$ = new Subject();

  constructor(private stockadjustmentService: StockadjustmentService,
    private alerifyService: AlertifyService,
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit() {
    this.isLoadingResults = true;
    this.isRateLimitReached = false;
    this.route.data.pipe(
      takeUntil(this.destroyed$)
    ).subscribe(data => {
      this.shopstocks = data.stockadjustments.result;
      this.pagination = data.stockadjustments.pagination;
      this.listData = new MatTableDataSource(this.shopstocks);
      this.listData.paginator = this.paginator;
      this.isLoadingResults = false;
    });
    console.log(this.shopstocks)
  }
  ngAfterViewInit() {
    this.listData.paginator = this.paginator;
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
    this.stockadjustmentService.getStockAdjustmentHeaders(true, this.pagination.currentPage, this.pagination.itemsPerPage).pipe(
      takeUntil(this.destroyed$)
    ).subscribe(
      (res: PaginatedResult<StockAdjustmentHeader[]>) => {
        this.shopstocks = res.result;
        this.pagination = res.pagination;
        this.listData = new MatTableDataSource(this.shopstocks);
        this.listData.paginator = this.paginator;
        this.isLoadingResults = false;
        console.log(res);
      },
      error => {
        this.alerifyService.error(error);
        this.router.navigate(['/stock/stockadjustmentlist']);
        console.log(error);
      },
      () => {
        this.router.navigate(['/stock/stockadjustmentlist']);
      }

    );
  }
}
