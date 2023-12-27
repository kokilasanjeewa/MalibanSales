import { CollectionViewer, DataSource } from '@angular/cdk/collections';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { catchError, finalize, map } from 'rxjs/operators';
import { PaginatedResult, Pagination } from '../models/pagination';
import { Supplier } from '../models/supplier';
import { SupplierService } from './supplier.service';

export class SupplierDataSource implements DataSource<Supplier> {
  private supplierList = new BehaviorSubject<Supplier[]>([]);
  private pagination = new BehaviorSubject<Pagination>(null);
  private loadingSuppliers = new BehaviorSubject<boolean>(false);
  public loading$ = this.loadingSuppliers.asObservable();
  constructor(private supplierService: SupplierService) {

  }
  loadSuppliers(status: boolean, pageNumber: number, pageSize: number) {
    this.loadingSuppliers.next(true);
    this.supplierService.getSuppliers(status, pageNumber, pageSize).pipe(
      catchError(() => of([])),
      finalize(() => this.loadingSuppliers.next(false))

    ).subscribe((res: PaginatedResult<Supplier[]>) => {
      this.supplierList.next(res.result);
      this.pagination.next(res.pagination);
    });
  }
  connect(collectionViewer: CollectionViewer): Observable<Supplier[]> {
    console.log('Connecting data source');
    return this.supplierList.asObservable();
  }
  connectp(collectionViewer: CollectionViewer): Observable<Pagination> {
    console.log('Connecting data source');
    return this.pagination.asObservable();

  }
  disconnect(collectionViewer: CollectionViewer): void {
    this.supplierList.complete();
    this.pagination.complete();
    this.loadingSuppliers.complete();
  }
  disconnectp(collectionViewer: CollectionViewer): void {
    this.pagination.complete();
  }
}