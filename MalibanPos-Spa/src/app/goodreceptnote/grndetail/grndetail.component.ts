import { GrnService } from './../../_services/grn.service';
import { GoodReceptNoteDetail } from './../../models/goodreceptnotedetail';
import { Pagination, PaginatedResult } from './../../models/pagination';
import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Output } from '@angular/core';
import { EventEmitter } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  templateUrl: './grndetail.component.html',
  styleUrls: ['./grndetail.component.css']
})
export class GrndetailComponent implements OnInit {
  public maxSize: number = 5;
  isLoadingResults: boolean;
  isRateLimitReached: boolean;
  pagination: Pagination;
  displayedColumns: string[] = [
    'goodReceptNoteDetailId',
    'id',
    'product',
    'costPrice',
    'retailPrice',
    'itemQuantity',
    'balanceItemQuantity',
    'freeItemQuantity',
    'invoiceItemDate',
    'groupDiscountAmount',
    'productExpiryDate',
    'batchNumber',
    'goodReceptNoteLine',
    'createdBy',
    'action'
  ];
  goodReceptNoteDetails: GoodReceptNoteDetail[];
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
    const id = this.route.snapshot.params['goodReceptNoteHeaderId'];
    console.log('GrndetailComponent works')
    console.log(id);
    console.log(+this.route.snapshot.params.goodReceptNoteHeaderId);
    /*   this.purchaserequestService.getPurchaseRequestDetails(true, 1, 5, +this.route.snapshot.params.purchaseRequestHeaderId)
         .subscribe((data) => {
           console.log(data);
         }); */
    this.isLoadingResults = true;
    this.isRateLimitReached = false;
    this.route.data.pipe(
      takeUntil(this.destroyed$)
    ).subscribe(data => {
      this.goodReceptNoteDetails = data.goodReceptNoteDetails.result;
      this.pagination = data.goodReceptNoteDetails.pagination;
      this.listData = new MatTableDataSource(this.goodReceptNoteDetails);
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
    this.grnService.getGoodReceiptDetails(true, this.pagination.currentPage, this.pagination.itemsPerPage, +this.route.snapshot.params.purchaseOrderHeaderId).pipe(
      takeUntil(this.destroyed$)
    ).subscribe(
      (res: PaginatedResult<GoodReceptNoteDetail[]>) => {
        this.goodReceptNoteDetails = res.result;
        this.pagination = res.pagination;
        this.listData = new MatTableDataSource(this.goodReceptNoteDetails);
        this.listData.paginator = this.paginator;
        this.isLoadingResults = false;
        console.log(res);
      },
      error => {
        // this.alerifyService.error(error);
        this.router.navigate(['/grn/grndetail']);
        console.log(error);
      },
      () => {
        this.router.navigate(['/grn/grndetail']);
      }

    );
  }
}
