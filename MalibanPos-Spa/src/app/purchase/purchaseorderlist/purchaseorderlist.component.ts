import { PurchaseOrderHeader } from './../../models/purchaseorderheader';
import { Pagination, PaginatedResult } from './../../models/pagination';
import { AfterViewInit, EventEmitter, OnDestroy } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { PurchaseorderService } from 'src/app/_services/purchaseorder.service';
import { ActivatedRoute, Router } from '@angular/router';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { ViewChild } from '@angular/core';
import { Output } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { PurchaseorderapprovemodelComponent } from '../purchaseorderapprovemodel/purchaseorderapprovemodel.component';
import { CommonService } from 'src/app/_services/common.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  templateUrl: './purchaseorderlist.component.html',
  styleUrls: ['./purchaseorderlist.component.css'],
})
export class PurchaseorderlistComponent
  implements OnInit, AfterViewInit, OnDestroy
{
  public maxSize: number = 5;
  status = true;
  pageNumber = 1;
  pageSize = 5;
  isLoadingResults: boolean;
  isRateLimitReached: boolean;
  pagination: Pagination;
  displayedColumns: string[] = [
    'id',
    'purchaseOrderHeaderId',
    'purchaseOrderReferenceNo',
    'poDate',
    'deliveryDate',
    'isProcess',
    'isApprove',
    'isDeleted',
    'quatationNo',
    'purchaseAmount',
    'supplier',
    'paymentMethod',
    'approvedBy',
    'authorizedBy',
    'action',
  ];
  purchaseOrderHeaders: PurchaseOrderHeader[];
  listData: MatTableDataSource<any>;
  // @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @Output() onPageSwitch = new EventEmitter();
  public modalRef: BsModalRef;
  bsModalRef: BsModalRef;
  cApprovalId: any;
  private destroyed$ = new Subject();

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private purchaseorderService: PurchaseorderService,
    private modalService: BsModalService,
    private commonService: CommonService,
    private alertifyService: AlertifyService
  ) {}

  ngOnInit() {
    this.isLoadingResults = true;
    this.isRateLimitReached = false;
    this.route.data.pipe(takeUntil(this.destroyed$)).subscribe({
      next: (data) => {
        console.log(data);
        this.purchaseOrderHeaders = data.purchaseOrderHeaders.result;
        this.pagination = data.purchaseOrderHeaders.pagination;
        this.listData = new MatTableDataSource(this.purchaseOrderHeaders);
        this.listData.paginator = this.paginator;
        this.isLoadingResults = false;
      },
      error: (err) => {},
      complete: () => {},
    });
  }
  ngOnDestroy() {
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
    this.purchaseorderService
      .getPurchaseOrderHeaders(
        this.status,
        this.pageNumber,
        this.pageSize,
        filterValue.trim().toLowerCase()
      )
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
       next: (res: PaginatedResult<PurchaseOrderHeader[]>) => {
          this.purchaseOrderHeaders = res.result;
          this.pagination = res.pagination;
          this.listData = new MatTableDataSource(this.purchaseOrderHeaders);
          this.listData.paginator = this.paginator;
          this.isLoadingResults = false;
          console.log(res);
        },
       error: (error) => {
          // this.alerifyService.error(error);
          this.router.navigate(['/purchase/purchaseorderlist']);
          console.log(error);
        },
       complete:  () => {
          this.router.navigate(['/purchase/purchaseorderlist']);
        }
  });
    if (this.listData.paginator) {
      this.listData.paginator.firstPage();
    }
  }
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.purchaseorderService
      .getPurchaseOrderHeaders(
        true,
        this.pagination.currentPage,
        this.pagination.itemsPerPage,
        ''
      )
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: (res: PaginatedResult<PurchaseOrderHeader[]>) => {
          this.purchaseOrderHeaders = res.result;
          this.pagination = res.pagination;
          this.listData = new MatTableDataSource(this.purchaseOrderHeaders);
          this.listData.paginator = this.paginator;
          this.isLoadingResults = false;
          console.log(res);
        },
        error: (error) => {
          // this.alerifyService.error(error);
          this.router.navigate(['/purchase/purchaseorderlist']);
          console.log(error);
        },
        complete: () => {
          this.router.navigate(['/purchase/purchaseorderlist']);
        },
      });
  }
  openModalWithPurchaseOrderApproveComponent(
    event,
    purchaseOrderHeaderId: number
  ) {
    this.bsModalRef = this.modalService.show(
      PurchaseorderapprovemodelComponent,
      {
        initialState: null,
        class: 'gray modal-md modal-dialog-centered',
        backdrop: true,
        ignoreBackdropClick: false,
        animated: false,
      }
    );
    this.bsModalRef.content.updateErrorAdd.subscribe((data) => {
      console.log(data.cApprovalId);
      this.cApprovalId = data.cApprovalId;
      var approvalModel = {
        purchaseOrderHeaderId: purchaseOrderHeaderId,
        cApprovalType: this.cApprovalId,
        createdBy: this.commonService.getUser(),
        id: this.commonService.getUserId(),
      };
      this.purchaseorderService
        .ApprovePurchaseOrderHeader(approvalModel)
        .pipe(takeUntil(this.destroyed$))
        .subscribe({
          next: (res: any) => {
            console.log(res);
            this.alertifyService.success(res.message);
          },
          error: (error) => {
            this.alertifyService.error(error);
            this.router.navigate(['/purchase/purchaseorderlist']);
            console.log(error);
          },
          complete: () => {
            this.router.navigate([
              '../purchase/purchaseorderreport',
              purchaseOrderHeaderId,
            ]);
          },
        });
    });
  }
}
