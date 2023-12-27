import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanymastersettingComponent } from './companymastersetting.component';

describe('CompanymastersettingComponent', () => {
  let component: CompanymastersettingComponent;
  let fixture: ComponentFixture<CompanymastersettingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CompanymastersettingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CompanymastersettingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
