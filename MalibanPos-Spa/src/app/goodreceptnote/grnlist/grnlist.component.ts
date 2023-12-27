import { GoodReceptNoteHeader } from './../../models/goodreceptnoteheader';
import { GrnService } from './../../_services/grn.service';
import { PaginatedResult, Pagination } from './../../models/pagination';
import { AfterViewInit, EventEmitter, OnDestroy, Output, ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  templateUrl: './grnlist.component.html',
  styleUrls: ['./grnlist.component.css']
})
export class GrnlistComponent implements OnInit, AfterViewInit,OnDestroy {
  public maxSize: number = 5;
  status = true;
  pageNumber = 1;
  pageSize = 5;
  isLoadingResults: boolean;
  isRateLimitReached: boolean;
  pagination: Pagination;
  displayedColumns: string[] = [
    'id',
    'goodReceptNoteReferenceNo',
    'supplierInvoiceId',
    'supplierInvoiceDate',
    'totalCost',
    'freeItemsTotalCost',
    'totalDiscountAmount',
    'supplier',
    'store',
    'purchaseOrderHeader',
    'goodReceptNoteHeaderId',
    'createDate',
    'createdBy',
    'action'
  ];
  goodReceptNoteHeaders: GoodReceptNoteHeader[];
  listData: MatTableDataSource<any>;
  // @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @Output() onPageSwitch = new EventEmitter();
  private destroyed$ = new Subject();

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private grnService: GrnService) { }

  ngOnInit() {
    this.isLoadingResults = true;
    this.isRateLimitReached = false;
    this.route.data.pipe(
      takeUntil(this.destroyed$)
    ).subscribe({
     next:  data => {
      console.log(data);
      this.goodReceptNoteHeaders = data.goodReceptNoteHeaders.result;
      this.pagination = data.goodReceptNoteHeaders.pagination;
      this.listData = new MatTableDataSource(this.goodReceptNoteHeaders);
      this.listData.paginator = this.paginator;
      this.isLoadingResults = false;
    },
    error(err) {

    },
    complete: () => {}
  });
  }
  ngOnDestroy(): void {
    this.destroyed$.next();
    this.destroyed$.complete();
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
    this.grnService.getGoodReceiptHeaders(this.status, this.pageNumber, this.pageSize,filterValue.trim().toLowerCase()).pipe(
      takeUntil(this.destroyed$)
    ).subscribe({
     next: (res: PaginatedResult<GoodReceptNoteHeader[]>) => {
        this.goodReceptNoteHeaders = res.result;
        this.pagination = res.pagination;
        this.listData = new MatTableDataSource(this.goodReceptNoteHeaders);
        this.listData.paginator = this.paginator;
        this.isLoadingResults = false;
        console.log(res);
      },
    error:  error => {
        // this.alerifyService.error(error);
        this.router.navigate(['/grn/grnlist']);
        console.log(error);
      },
    complete:  () => {
        this.router.navigate(['/grn/grnlist']);
      }

  });
    if (this.listData.paginator) {
      this.listData.paginator.firstPage();
    }
  }
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.grnService.getGoodReceiptHeaders(true, this.pagination.currentPage, this.pagination.itemsPerPage,'').pipe(
      takeUntil(this.destroyed$)
    ).subscribe({
     next: (res: PaginatedResult<GoodReceptNoteHeader[]>) => {
        this.goodReceptNoteHeaders = res.result;
        this.pagination = res.pagination;
        this.listData = new MatTableDataSource(this.goodReceptNoteHeaders);
        this.listData.paginator = this.paginator;
        this.isLoadingResults = false;
        console.log(res);
      },
     error: error => {
        // this.alerifyService.error(error);
        this.router.navigate(['/grn/grnlist']);
        console.log(error);
      },
     complete: () => {
        this.router.navigate(['/grn/grnlist']);
      }

  });
  }

}
