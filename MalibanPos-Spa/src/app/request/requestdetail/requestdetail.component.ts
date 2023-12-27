import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { PurchaseRequestDetail } from './../../models/purchaserequestdetail';
import { Pagination, PaginatedResult } from './../../models/pagination';
import { PurchaseService } from './../../_services/purchase.service';
import { Component, OnInit, Output, EventEmitter, ViewChild, AfterViewInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  templateUrl: './requestdetail.component.html',
  styleUrls: ['./requestdetail.component.css']
})
export class RequestdetailComponent implements OnInit, AfterViewInit {
  public maxSize: number = 5;
  isLoadingResults: boolean;
  isRateLimitReached: boolean;
  pagination: Pagination;
  displayedColumns: string[] = [
    'purchaseRequestDetailId',
    'id',
    'product',
    'purchaseRequestHeader',
    'requestDate',
    'quantity',
    'rejectQuantity',
    'stockQuantity',
    'approvedQuantity',
    'isComfirm',
    'createdBy',
    'createDate',
    'action'
  ];
  purchaseRequestDetails: PurchaseRequestDetail[];
  listData: MatTableDataSource<any>;
  // @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @Output() onPageSwitch = new EventEmitter();
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private purchaserequestService: PurchaseService) { }

  ngOnInit() {
    const id = this.route.snapshot.params['purchaseRequestHeaderId'];
    console.log('requestdetailscomponents works')
    console.log(id);
    console.log(+this.route.snapshot.params.purchaseRequestHeaderId);
    /*   this.purchaserequestService.getPurchaseRequestDetails(true, 1, 5, +this.route.snapshot.params.purchaseRequestHeaderId)
         .subscribe((data) => {
           console.log(data);
         }); */
    this.isLoadingResults = true;
    this.isRateLimitReached = false;
    this.route.data.subscribe(data => {
      this.purchaseRequestDetails = data.purchaseRequestDetails.result;
      this.pagination = data.purchaseRequestDetails.pagination;
      this.listData = new MatTableDataSource(this.purchaseRequestDetails);
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
    this.purchaserequestService.getPurchaseRequestDetails(true, this.pagination.currentPage, this.pagination.itemsPerPage, +this.route.snapshot.params.purchaseRequestHeaderId).subscribe(
      (res: PaginatedResult<PurchaseRequestDetail[]>) => {
        this.purchaseRequestDetails = res.result;
        this.pagination = res.pagination;
        this.listData = new MatTableDataSource(this.purchaseRequestDetails);
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
