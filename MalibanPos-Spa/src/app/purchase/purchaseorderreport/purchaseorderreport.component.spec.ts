import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PurchaseorderreportComponent } from './purchaseorderreport.component';

describe('PurchaseorderreportComponent', () => {
  let component: PurchaseorderreportComponent;
  let fixture: ComponentFixture<PurchaseorderreportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PurchaseorderreportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PurchaseorderreportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
