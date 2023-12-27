import { TestBed } from '@angular/core/testing';

import { CustomermasterService } from './customermaster.service';

describe('CustomermasterService', () => {
  let service: CustomermasterService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CustomermasterService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
