import { TestBed } from '@angular/core/testing';

import { StocktransferService } from './stocktransfer.service';

describe('StocktransferService', () => {
  let service: StocktransferService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(StocktransferService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
