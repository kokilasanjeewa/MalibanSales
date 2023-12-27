import { TestBed } from '@angular/core/testing';

import { CompanymastersettingService } from './companymastersetting.service';

describe('CompanymastersettingService', () => {
  let service: CompanymastersettingService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CompanymastersettingService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
