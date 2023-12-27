import { StockAdjustmentDetail } from './../../models/stockadjustmentdetail';
import { Pagination, PaginatedResult } from './../../models/pagination';
import { AfterViewInit, Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { StockadjustmentService } from 'src/app/_services/stockadjustment.service';

@Component({
  templateUrl: './stockadjustmentdetail.component.html',
  styleUrls: ['./stockadjustmentdetail.component.css']
})
export class StockadjustmentdetailComponent implements OnInit, AfterViewInit {
  public maxSize: number = 5;
  isLoadingResults: boolean;
  isRateLimitReached: boolean;
  pagination: Pagination;
  displayedColumns: string[] = [
    'stockAdjustmentDetailId',
    'id',
    'barcode',
    'productName',
    'costPrice',
    'retailPrice',
    'adjustmentQuantity',
    'approveQuantity',
    'stockAdjustmentTotal',
    'currentQuantity',
    'previousQuantity',
    'stockAdjustmentHeader',
    'createDate',
  ];
  stockAdjustmentDetail: StockAdjustmentDetail[];
  listData: MatTableDataSource<any>;
  // @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @Output() onPageSwitch = new EventEmitter();
  constructor(private route: ActivatedRoute,
    private router: Router,
    private stockadjustmentService: StockadjustmentService) { }

  ngOnInit() {
    const id = this.route.snapshot.params['stockadjustmentHeaderId'];
    console.log('StockadjustmentdetailComponent works')
    console.log(id);
    console.log(+this.route.snapshot.params.stockadjustmentHeaderId);
    this.isLoadingResults = true;
    this.isRateLimitReached = false;
    this.route.data.subscribe(data => {
      this.stockAdjustmentDetail = data.stockadjustmentDetails.result;
      this.pagination = data.stockadjustmentDetails.pagination;
      this.listData = new MatTableDataSource(this.stockAdjustmentDetail);
      this.listData.paginator = this.paginator;
      this.isLoadingResults = false;
    });
  }
  ngAfterViewInit() {
    this.listData.paginator = this.paginator;

  }
  change(pageEvent: PageEvent) {
    this.paginator.pageIndex = pageEvent.pageIndex + 1;
    this.paginator.pageSize = pageEvent.pageSize;
    this.paginator.length = pageEvent.length;
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.listData.filter = filterValue.trim().toLowerCase();
    if (this.listData.paginator) {
      this.listData.paginator.firstPage();
    }
  }
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.stockadjustmentService.getStockAdjustmentDetails(true, this.pagination.currentPage, this.pagination.itemsPerPage, +this.route.snapshot.params.stockadjustmentHeaderId).subscribe(
      (res: PaginatedResult<StockAdjustmentDetail[]>) => {
        this.stockAdjustmentDetail = res.result;
        this.pagination = res.pagination;
        this.listData = new MatTableDataSource(this.stockAdjustmentDetail);
        this.listData.paginator = this.paginator;
        this.isLoadingResults = false;
        console.log(res);
      },
      error => {
        // this.alerifyService.error(error);
        this.router.navigate(['/requset/requsetlist']);
        console.log(error);
      },
      () => {
        this.router.navigate(['/requset/requsetlist']);
      }

    );
  }
}
