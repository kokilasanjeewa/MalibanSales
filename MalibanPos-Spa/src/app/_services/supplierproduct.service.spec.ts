import { TestBed } from '@angular/core/testing';

import { SupplierproductService } from './supplierproduct.service';

describe('SupplierproductService', () => {
  let service: SupplierproductService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SupplierproductService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
