import { TestBed } from '@angular/core/testing';

import { StockadjustmentService } from './stockadjustment.service';

describe('StockadjustmentService', () => {
  let service: StockadjustmentService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(StockadjustmentService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
