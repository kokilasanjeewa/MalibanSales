import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SalesdetailsbycustomerComponent } from './salesdetailsbycustomer.component';

describe('SalesdetailsbycustomerComponent', () => {
  let component: SalesdetailsbycustomerComponent;
  let fixture: ComponentFixture<SalesdetailsbycustomerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SalesdetailsbycustomerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SalesdetailsbycustomerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
