import { TestBed } from '@angular/core/testing';

import { ShopstockService } from './shopstock.service';

describe('ShopstockService', () => {
  let service: ShopstockService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ShopstockService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
