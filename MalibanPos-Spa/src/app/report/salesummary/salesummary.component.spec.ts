import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SalesummaryComponent } from './salesummary.component';

describe('SalesummaryComponent', () => {
  let component: SalesummaryComponent;
  let fixture: ComponentFixture<SalesummaryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SalesummaryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SalesummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
