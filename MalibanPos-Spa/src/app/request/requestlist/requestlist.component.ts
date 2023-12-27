import { PurchaseRequestHeader } from './../../models/purchaserequestheader';
import { PurchaseService } from './../../_services/purchase.service';
import { PaginatedResult, Pagination } from './../../models/pagination';
import { AfterViewInit, Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  templateUrl: './requestlist.component.html',
  styleUrls: ['./requestlist.component.css']
})
export class RequestlistComponent implements OnInit, AfterViewInit {
  public maxSize: number = 5;
  isLoadingResults: boolean;
  isRateLimitReached: boolean;
  pagination: Pagination
  displayedColumns: string[] = [
    'purchaseRequestHeaderId',
    'id',
    'department',
    'section',
    'createDate',
    'expectedDate',
    'isProcess',
    'isComfirm',
    'purchaseRequestStatus',
    'approvedBy',
    'authorizedBy',
    'action'
  ];
  purchaseRequestHeaders: PurchaseRequestHeader[];
  listData: MatTableDataSource<any>;
  // @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @Output() onPageSwitch = new EventEmitter();

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private purchaserequestService: PurchaseService) { }

  ngOnInit() {
    this.isLoadingResults = true;
    this.isRateLimitReached = false;
    this.route.data.subscribe(data => {
      this.purchaseRequestHeaders = data.purchaseRequestHeaders.result;
      this.pagination = data.purchaseRequestHeaders.pagination;
      this.listData = new MatTableDataSource(this.purchaseRequestHeaders);
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
    this.purchaserequestService.getPurchaseRequestHeaders(true, this.pagination.currentPage, this.pagination.itemsPerPage).subscribe(
      (res: PaginatedResult<PurchaseRequestHeader[]>) => {
        this.purchaseRequestHeaders = res.result;
        this.pagination = res.pagination;
        this.listData = new MatTableDataSource(this.purchaseRequestHeaders);
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
