import { TestBed } from '@angular/core/testing';

import { BranchlocationService } from './branchlocation.service';

describe('BranchlocationService', () => {
  let service: BranchlocationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BranchlocationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
