import { Claim } from './../../models/claim';
import { AuthService } from './../../_services/auth.service';
import { Root } from './../../models/root';
import { PriceService } from './../../_services/price.service';
import { SupplierPrice } from './../../models/supplierprice';
import { Pagination, PaginatedResult } from './../../models/pagination';
import { AfterViewInit, Component, EventEmitter, OnInit, Output } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonService } from 'src/app/_services/common.service';
import { Observable, Subject } from 'rxjs';
import { SupplierPriceSupplier } from 'src/app/models/supplierpricesupplier';
import { takeUntil } from 'rxjs/operators';

@Component({
  templateUrl: './supplierpricelist.component.html',
  styleUrls: ['./supplierpricelist.component.css']
})
export class SupplierpricelistComponent implements OnInit, AfterViewInit {
  public maxSize: number = 5;
  pageNumber = 1;
  pageSize = 5;
  isLoadingResults: boolean;
  isRateLimitReached: boolean;
  barCode: string;
  supplierName: string;
  root: Root;
  claims : string[];
  pagination = {
    currentPage: 0,
    itemsPerPage: 0,
    totalItems: 0,
    totalPages: 0
  };  displayedColumns: string[] = [
    'supplierPriceId',
    'id',
    'code',
    'product',
    'supplier',
    'unitPrice',
    'profit',
    'maximumRetailPrice',
    'discountPrice',
    'discountPrecentage',
    'promotionPrice',
    'promotionPrecentage',
    'taxPrecentage',
    'taXMethod',
    'statusType',
    'priceDate',
    'active',
    // 'isDeleted',
  ];
  supplierPrices: SupplierPrice[];
  supplierPricesFilter: Observable<SupplierPriceSupplier[]>;

  listData: MatTableDataSource<any>;
  // @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @Output() onPageSwitch = new EventEmitter();
  private destroyed$ = new Subject();

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private priceService: PriceService,
    public commonService: CommonService,
    private auth : AuthService) { }

  ngOnInit() {
    this.isLoadingResults = true;
    this.isRateLimitReached = false;
    this.route.data.pipe(
      takeUntil(this.destroyed$)
    ).subscribe(data => {
      console.log(data);
      this.supplierPrices = data.supplierPrices.result;
      console.log(this.supplierPricesFilter);
      //this.claims =data.claim;
      console.log("canAccessAddSupplierPrice");
      // console.log(data.claim);
      this.pagination = data.supplierPrices.pagination;
      this.listData = new MatTableDataSource(this.supplierPrices);
      this.listData.paginator = this.paginator;
      this.isLoadingResults = false;
    });
     this.supplierPricesFilter=this.priceService.getallsupplierpricesuppliers(this.commonService.getUserBranchLocationId(),false);
     console.log('this.supplierPricesFilter');
     console.log(this.supplierPricesFilter);
          //https://www.usefuldev.com/blog
     /*      this.supplierPricesFilter =this.supplierPrices.filter( (thing, i, arr) => {
            return arr.indexOf(arr.find(t => t.supplier.supplierId===thing.supplier.supplierId)) === i
          }); */
  /*   for(var claim of this.auth.rolePermissionMatch(this.claims)){
      console.log(claim);
    } */
  }
  ngAfterViewInit() {
    this.listData.paginator = this.paginator;

  }
  change(pageEvent: PageEvent) {
    this.paginator.pageIndex = pageEvent.pageIndex + 1;
    this.paginator.pageSize = pageEvent.pageSize;
    this.paginator.length = pageEvent.length;
  }
  resetCalculations(){
    this.priceService.getSupplierPrices(true, this.pageNumber, this.pageSize,"").pipe(
      takeUntil(this.destroyed$)
    ).subscribe(
      (res: PaginatedResult<SupplierPrice[]>) => {
        this.supplierPrices = res.result;
        this.pagination = res.pagination;
        this.listData = new MatTableDataSource(this.supplierPrices);
        this.listData.paginator = this.paginator;
        this.isLoadingResults = false;
        console.log(res);
      },
      error => {
        // this.alerifyService.error(error);
        this.router.navigate(['/supplierprice/supplierpricelist']);
        console.log(error);
      },
      () => {
        this.router.navigate(['/supplierprice/supplierpricelist']);
      }

    );
  }
  itemChange(){
    console.log(this.supplierName);
    var query ='supplierName:'+this.supplierName+','+'barCode:'+'';
    this.priceService.getSupplierPrices(true, this.pageNumber, this.pageSize,query).pipe(
      takeUntil(this.destroyed$)
    ).subscribe(
      (res: PaginatedResult<SupplierPrice[]>) => {
        this.supplierPrices = res.result;
        this.pagination = res.pagination;
        this.listData = new MatTableDataSource(this.supplierPrices);
        this.listData.paginator = this.paginator;
        this.isLoadingResults = false;
        console.log(res);
      },
      error => {
        // this.alerifyService.error(error);
        this.router.navigate(['/supplierprice/supplierpricelist']);
        console.log(error);
      },
      () => {
        this.router.navigate(['/supplierprice/supplierpricelist']);
      }

    );
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    let spn =this.supplierName;
    var queryString ='supplierName:'+spn+','+'barCode:'+this.barCode;
    this.priceService.getSupplierPrices(true, this.pageNumber, this.pageSize,queryString).pipe(
      takeUntil(this.destroyed$)
    ).subscribe(
      (res: PaginatedResult<SupplierPrice[]>) => {
        this.supplierPrices = res.result;
        this.pagination = res.pagination;
        this.listData = new MatTableDataSource(this.supplierPrices);
        this.listData.paginator = this.paginator;
        this.isLoadingResults = false;
        console.log(res);
      },
      error => {
        // this.alerifyService.error(error);
        this.router.navigate(['/supplierprice/supplierpricelist']);
        console.log(error);
      },
      () => {
        this.router.navigate(['/supplierprice/supplierpricelist']);
      }

    );
    this.listData.filter = filterValue.trim().toLowerCase();
    if (this.listData.paginator) {
      this.listData.paginator.firstPage();
    }
  }
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    var queryString ='supplierName:'+this.supplierName+','+'barCode:'+'';
    this.priceService.getSupplierPrices(true, this.pagination.currentPage, this.pagination.itemsPerPage,queryString).pipe(
      takeUntil(this.destroyed$)
    ).subscribe(
      (res: PaginatedResult<SupplierPrice[]>) => {
        this.supplierPrices = res.result;
        this.pagination = res.pagination;
        this.listData = new MatTableDataSource(this.supplierPrices);
        this.listData.paginator = this.paginator;
        this.isLoadingResults = false;
        console.log(res);
      },
      error => {
        // this.alerifyService.error(error);
        this.router.navigate(['/supplierprice/supplierpricelist']);
        console.log(error);
      },
      () => {
        this.router.navigate(['/supplierprice/supplierpricelist']);
      }

    );
  }
}
