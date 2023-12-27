import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanymastersettinglistComponent } from './companymastersettinglist.component';

describe('CompanymastersettinglistComponent', () => {
  let component: CompanymastersettinglistComponent;
  let fixture: ComponentFixture<CompanymastersettinglistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CompanymastersettinglistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanymastersettinglistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
