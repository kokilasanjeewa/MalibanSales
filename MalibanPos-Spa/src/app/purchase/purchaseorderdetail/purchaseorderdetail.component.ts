import { PurchaseOrderDetail } from './../../models/purchaseorderdetail';
import { Pagination, PaginatedResult } from './../../models/pagination';
import { Component, OnInit, AfterViewInit, EventEmitter, Output } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { ActivatedRoute, Router } from '@angular/router';
import { PurchaseorderService } from 'src/app/_services/purchaseorder.service';

@Component({
  templateUrl: './purchaseorderdetail.component.html',
  styleUrls: ['./purchaseorderdetail.component.css']
})
export class PurchaseorderdetailComponent implements OnInit, AfterViewInit {
  public maxSize: number = 5;
  isLoadingResults: boolean;
  isRateLimitReached: boolean;
  pagination: Pagination;
  displayedColumns: string[] = [
    'purchaseOrderDetailId',
    'id',
    'product',
    'supplierPrice',
    'quantity',
    'amount',
    'currentQuantity',
    'returnQuantity',
     'gRNQuantity',
    'freeQuantity',
    'purchaseOrderHeaders',
    'createdBy',
    'action'
  ];
  purchaseOrderDetails: PurchaseOrderDetail[];
  listData: MatTableDataSource<any>;
  // @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @Output() onPageSwitch = new EventEmitter();
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private purchaseorderService: PurchaseorderService) { }

  ngOnInit() {
    const id = this.route.snapshot.params['purchaseOrderHeaderId'];
    console.log('requestdetailscomponents works')
    console.log(id);
    console.log(+this.route.snapshot.params.purchaseOrderHeaderId);
    /*   this.purchaserequestService.getPurchaseRequestDetails(true, 1, 5, +this.route.snapshot.params.purchaseRequestHeaderId)
         .subscribe((data) => {
           console.log(data);
         }); */
    this.isLoadingResults = true;
    this.isRateLimitReached = false;
    this.route.data.subscribe(data => {
      this.purchaseOrderDetails = data.purchaseOrderDetails.result;
      this.pagination = data.purchaseOrderDetails.pagination;
      this.listData = new MatTableDataSource(this.purchaseOrderDetails);
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
    this.purchaseorderService.getPurchaseOrderDetails(true, this.pagination.currentPage, this.pagination.itemsPerPage, +this.route.snapshot.params.purchaseOrderHeaderId).subscribe(
      (res: PaginatedResult<PurchaseOrderDetail[]>) => {
        this.purchaseOrderDetails = res.result;
        this.pagination = res.pagination;
        this.listData = new MatTableDataSource(this.purchaseOrderDetails);
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
