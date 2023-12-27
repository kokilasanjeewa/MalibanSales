import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PurchasereturnreportComponent } from './purchasereturnreport.component';

describe('PurchasereturnreportComponent', () => {
  let component: PurchasereturnreportComponent;
  let fixture: ComponentFixture<PurchasereturnreportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PurchasereturnreportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PurchasereturnreportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
