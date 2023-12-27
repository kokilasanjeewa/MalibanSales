import { Pagination, PaginatedResult } from './../../models/pagination';
import { Product } from './../../models/product';
import { AfterViewInit, Component, EventEmitter, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ProductService } from 'src/app/_services/product.service';

@Component({
  templateUrl: './productlist.component.html',
  styleUrls: ['./productlist.component.css'],
})
export class ProductlistComponent implements OnInit, AfterViewInit,OnDestroy {
  public maxSize: number = 5;
  status = true;
  pageNumber = 1;
  pageSize = 5;
  isLoadingResults: boolean;
  isRateLimitReached: boolean;
  products: Product[];
  product: string;
  pagination = {
    currentPage: 0,
    itemsPerPage: 0,
    totalItems: 0,
    totalPages: 0
  };
   displayedColumns: string[] = [
    'productId',
    'id',
    'vendor',
    'barcode',
    'productName',
    'productDescription',
    'unitWeight',
    'unitOfMeasurement',
    'createDate',
    'action',
  ];
  listData: MatTableDataSource<any>;
  // @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @Output() onPageSwitch = new EventEmitter();
  private destroyed$ = new Subject();
  constructor(
    private productService: ProductService,
    private alerifyService: AlertifyService,
    private route: ActivatedRoute,
    private router: Router) {
  }
  ngOnInit() {
    this.isLoadingResults = true;
    this.isRateLimitReached = false;
    this.route.data.pipe(
      takeUntil(this.destroyed$)
    ).subscribe(data => {
      console.log("product list data")
      console.log(data)
      this.products = data.products.result;
      this.pagination = data.products.pagination;
      this.listData = new MatTableDataSource(this.products);
      this.listData.paginator = this.paginator;
      this.isLoadingResults = false;
    });
  }
  ngOnDestroy() {this.destroyed$.next();this.destroyed$.complete();}

  ngAfterViewInit() {
    this.listData.paginator = this.paginator;
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.productService.getProducts(this.status, this.pageNumber, this.pageSize, this.product).pipe(
      takeUntil(this.destroyed$)
    ).subscribe(
      (res: PaginatedResult<Product[]>) => {
        this.products = res.result;
        this.pagination = res.pagination;
        this.listData = new MatTableDataSource(this.products);
        this.listData.paginator = this.paginator;
        this.isLoadingResults = false;
        console.log(res);
      },
      error => {
        this.alerifyService.error(error);
        this.router.navigate(['/product/productlist']);
        console.log(error);
      },
      () => {
        this.router.navigate(['/product/productlist']);
      }
    );
    this.listData.filter = filterValue.toLocaleLowerCase();
    /*     const filterValue = (event.target as HTMLInputElement).value;
        this.listData.filter = filterValue.trim().toLowerCase();

        if (this.listData.paginator) {
          this.listData.paginator.firstPage();
        } */
  }
  change(pageEvent: PageEvent) {
    this.paginator.pageIndex = pageEvent.pageIndex + 1;
    this.paginator.pageSize = pageEvent.pageSize;
    this.paginator.length = pageEvent.length;
  }
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.productService.getProducts(true, this.pagination.currentPage, this.pagination.itemsPerPage, "").pipe(
      takeUntil(this.destroyed$)
    ).subscribe(
      (res: PaginatedResult<Product[]>) => {
        this.products = res.result;
        this.pagination = res.pagination;
        this.listData = new MatTableDataSource(this.products);
        this.listData.paginator = this.paginator;
        this.isLoadingResults = false;
        console.log(res);
      },
      error => {
        this.alerifyService.error(error);
        this.router.navigate(['/product/productlist']);
        console.log(error);
      },
      () => {
        this.router.navigate(['/product/productlist']);
      }

    );
  }
}
