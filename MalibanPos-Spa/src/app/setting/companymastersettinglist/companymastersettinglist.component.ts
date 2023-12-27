import { Pagination, PaginatedResult } from './../../models/pagination';
import { AfterViewInit,Component,EventEmitter,OnInit,Output,ViewChild,} from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { CalculationMaster } from 'src/app/models/calculationmaster';
import { CompanymastersettingService } from 'src/app/_services/companymastersetting.service';
@Component({
  selector: 'app-companymastersettinglist',
  templateUrl: './companymastersettinglist.component.html',
  styleUrls: ['./companymastersettinglist.component.css'],
})
export class CompanymastersettinglistComponent implements OnInit {
  public maxSize: number = 5;
  status = true;
  pageNumber = 1;
  pageSize = 5;
  isLoadingResults: boolean;
  isRateLimitReached: boolean;
  companyMasterlist: CalculationMaster[];
  product: string;
  pagination = {
    currentPage: 0,
    itemsPerPage: 0,
    totalItems: 0,
    totalPages: 0,
  };
  displayedColumns: string[] = [
    'calculationMasterId',
    'id',
    'calculationMasterDescription',
    'branchLocationId',
    'salePricePrecentageCompany',
    'salePricePrecentageEmployee',
    'managerCreditLimitPrecentage',
    'staffCreditLimitPrecentage',
    'workerCreditLimitPrecentage',
    'vatPrecentage',
    'nbtPrecentage',
    'createDate',
    'action',
  ];
  listData: MatTableDataSource<any>;
  // @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @Output() onPageSwitch = new EventEmitter();
  constructor(
    private mastersettingService: CompanymastersettingService,
    private alerifyService: AlertifyService,
    private route: ActivatedRoute,
    private router: Router
  ) {}
  ngOnInit() {
    this.isLoadingResults = true;
    this.isRateLimitReached = false;
    this.route.data.subscribe((data) => {
      console.log('product list data');
      console.log(data);
      this.companyMasterlist = data.companyMasterlist.result;
      this.pagination = data.companyMasterlist.pagination;
      this.listData = new MatTableDataSource(this.companyMasterlist);
      this.listData.paginator = this.paginator;
      this.isLoadingResults = false;
    });
  }
  ngAfterViewInit() {
    this.listData.paginator = this.paginator;
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.mastersettingService
      .getAllCompanyMasterSetting(this.status, this.pageNumber, this.pageSize, this.product)
      .subscribe(
        (res: PaginatedResult<CalculationMaster[]>) => {
          this.companyMasterlist = res.result;
          this.pagination = res.pagination;
          this.listData = new MatTableDataSource(this.companyMasterlist);
          this.listData.paginator = this.paginator;
          this.isLoadingResults = false;
          console.log(res);
        },
        (error) => {
          this.alerifyService.error(error);
          this.router.navigate(['/setting/companymasterlist']);
          console.log(error);
        },
        () => {
          this.router.navigate(['/setting/companymasterlist']);
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
    this.mastersettingService.getAllCompanyMasterSetting(
        true,
        this.pagination.currentPage,
        this.pagination.itemsPerPage,
        ''
      )
      .subscribe(
        (res: PaginatedResult<CalculationMaster[]>) => {
          this.companyMasterlist = res.result;
          this.pagination = res.pagination;
          this.listData = new MatTableDataSource(this.companyMasterlist);
          this.listData.paginator = this.paginator;
          this.isLoadingResults = false;
          console.log(res);
        },
        (error) => {
          this.alerifyService.error(error);
          this.router.navigate(['/setting/companymasterlist']);
          console.log(error);
        },
        () => {
          this.router.navigate(['/setting/companymasterlist']);
        }
      );
  }
}
