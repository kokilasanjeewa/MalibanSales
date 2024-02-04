import { PaginatedResult, Pagination } from './../../models/pagination';
import { AfterViewInit, Component, EventEmitter, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { CommonService } from 'src/app/_services/common.service';
import { StockTransferHeader, StocktransferService } from 'src/app/_services/stocktransfer.service';
import { StocktransferapprovemodelComponent } from '../stocktransferapprovemodel/stocktransferapprovemodel.component';
import { StocktransferreceivemodelComponent } from '../stocktransferreceivemodel/stocktransferreceivemodel.component';
@Component({
  selector: 'app-stocktransferlist',
  templateUrl: './stocktransferlist.component.html',
  styleUrls: ['./stocktransferlist.component.css']
})
export class StocktransferlistComponent implements OnInit, AfterViewInit,OnDestroy {
  public maxSize: number = 5;
  status = true;
  pageNumber = 1;
  pageSize = 5;
  isLoadingResults: boolean;
  isRateLimitReached: boolean;
  pagination: Pagination;
  displayedColumns: string[] = [
    'id',
    'stockTransferHeaderId',
    'stockTransferHeaderReferenceNo',
    'poDate',
    'receiveDate',
    'isProcess',
    'isApprove',
    'isDeleted',
    'purchaseAmount',
    'branch',
    'transfer',
    'transferType',
    'paymentMethod',
    'receivedBy',
    'approvedBy',
    'authorizedBy',
    'action',
  ];
  stockTransferHeaders: StockTransferHeader[];
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
    private stocktransferService: StocktransferService,
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
        this.stockTransferHeaders = data.stocktransferlist.result;
        this.pagination = data.stocktransferlist.pagination;
        this.listData = new MatTableDataSource(this.stockTransferHeaders);
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
    this.stocktransferService
      .getStockTransferLIst(
        this.status,
        this.pageNumber,
        this.pageSize,
        filterValue.trim().toLowerCase()
      )
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
       next: (res: PaginatedResult<StockTransferHeader[]>) => {
          this.stockTransferHeaders = res.result;
          this.pagination = res.pagination;
          this.listData = new MatTableDataSource(this.stockTransferHeaders);
          this.listData.paginator = this.paginator;
          this.isLoadingResults = false;
          console.log(res);
        },
       error: (error) => {
          // this.alerifyService.error(error);
          this.router.navigate(['/stock/stocktransferlist']);
          console.log(error);
        },
       complete:  () => {
          this.router.navigate(['/stock/stocktransferlist']);
        }
  });
    if (this.listData.paginator) {
      this.listData.paginator.firstPage();
    }
  }
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.stocktransferService
      .getStockTransferLIst(
        true,
        this.pagination.currentPage,
        this.pagination.itemsPerPage,
        ''
      )
      .pipe(takeUntil(this.destroyed$))
      .subscribe({
        next: (res: PaginatedResult<StockTransferHeader[]>) => {
          this.stockTransferHeaders = res.result;
          this.pagination = res.pagination;
          this.listData = new MatTableDataSource(this.stockTransferHeaders);
          this.listData.paginator = this.paginator;
          this.isLoadingResults = false;
          console.log(res);
        },
        error: (error) => {
          // this.alerifyService.error(error);
          this.router.navigate(['/stock/stocktransferlist']);
          console.log(error);
        },
        complete: () => {
          this.router.navigate(['/stock/stocktransferlist']);
        },
      });
  }

  openModalWithStockTransferApproveComponent(
    event,
    stockTransferHeaderId: number
  ) {
    this.bsModalRef = this.modalService.show(
      StocktransferapprovemodelComponent,
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
        stockTransferHeaderId: stockTransferHeaderId,
        cApprovalType: this.cApprovalId,
        createdBy: this.commonService.getUser(),
        id: this.commonService.getUserId(),
      };
      this.stocktransferService
        .ApproveStockTransferHeader(approvalModel)
        .pipe(takeUntil(this.destroyed$))
        .subscribe({
          next: (res: any) => {
            console.log(res);
            this.alertifyService.success(res.message);
          },
          error: (error) => {
            this.alertifyService.error(error);
            this.router.navigate(['/stock/stocktransferlist']);
            console.log(error);
          },
          complete: () => {
            this.router.navigate([
              '../stock/stocktransferreport',
              stockTransferHeaderId,
            ]);
          },
        });
    });
  }
  openModalWithStockTransferReceiveComponent(
    event,
    stockTransferHeaderId: number
  ) {
    this.bsModalRef = this.modalService.show(
      StocktransferreceivemodelComponent,
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
        stockTransferHeaderId: stockTransferHeaderId,
        cApprovalType: this.cApprovalId,
        createdBy: this.commonService.getUser(),
        id: this.commonService.getUserId(),
      };
      this.stocktransferService
        .ReceiveStockTransferHeader(approvalModel)
        .pipe(takeUntil(this.destroyed$))
        .subscribe({
          next: (res: any) => {
            console.log(res);
            this.alertifyService.success(res.message);
          },
          error: (error) => {
            this.alertifyService.error(error);
            this.router.navigate(['/stock/stocktransferlist']);
            console.log(error);
          },
          complete: () => {
            this.router.navigate([
              '../stock/stocktransferreport',
              stockTransferHeaderId,
            ]);
          },
        });
    });
  }
}
