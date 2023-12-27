import { PurchaseReturnHeader } from './../../models/purchasereturnheader';
import { Component, OnInit } from '@angular/core';
import { PurchaseOrderHeader } from './../../models/purchaseorderheader';
import { Pagination, PaginatedResult } from './../../models/pagination';
import { AfterViewInit, EventEmitter } from '@angular/core';
import { PurchaseorderService } from 'src/app/_services/purchaseorder.service';
import { ActivatedRoute, Router } from '@angular/router';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { ViewChild } from '@angular/core';
import { Output } from '@angular/core';
import { PurchasereturnService } from 'src/app/_services/purchasereturn.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
@Component({
  templateUrl: './purchasereturnlist.component.html',
  styleUrls: ['./purchasereturnlist.component.css']
})
export class PurchasereturnlistComponent implements OnInit,AfterViewInit {
  public maxSize: number = 5;
  isLoadingResults: boolean;
  isRateLimitReached: boolean;
  length = 0;
  pagination: Pagination;
  displayedColumns: string[] = [
    'id',
    'purchaseReturnHeaderId',
    'description',
    'purchaseReturnDate',
    'purchaseReturnAmount',
    'supplier',
    'returnReason',
    'action'
  ];
  purchaseOrderHeaders: PurchaseReturnHeader[];
  listData: MatTableDataSource<any>;
  // @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @Output() onPageSwitch = new EventEmitter();
  private destroyed$ = new Subject();

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private purchasereturnService: PurchasereturnService,
    ) {
  }

  ngOnInit() {
     this.isLoadingResults = true;
    this.isRateLimitReached = false;
    this.route.data.pipe(
      takeUntil(this.destroyed$)
    ).subscribe(data => {
      console.log(data);
      this.purchaseOrderHeaders = data.purchaseReturnHeaders.result;
      this.pagination = data.purchaseReturnHeaders.pagination;
      this.listData = new MatTableDataSource(this.purchaseOrderHeaders);
      this.length = this.listData.data.length;
      console.log("data length is");
      console.log(this.length);
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
    this.purchasereturnService.getPurchaseReturnHeaders(true, this.pagination.currentPage, this.pagination.itemsPerPage).pipe(
      takeUntil(this.destroyed$)
    ).subscribe(
      (res: PaginatedResult<PurchaseReturnHeader[]>) => {
        this.purchaseOrderHeaders = res.result;
        this.pagination = res.pagination;
        this.listData = new MatTableDataSource(this.purchaseOrderHeaders);
        this.listData.paginator = this.paginator;
        this.isLoadingResults = false;
        console.log(res);
      },
      error => {
        // this.alerifyService.error(error);
        this.router.navigate(['/purchase/purchasereturnlist']);
        console.log(error);
      },
      () => {
        this.router.navigate(['/purchase/purchasereturnlist']);
      }

    );
  }

}
