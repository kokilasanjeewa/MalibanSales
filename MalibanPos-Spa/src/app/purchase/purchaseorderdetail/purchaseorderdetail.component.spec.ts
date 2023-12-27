import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PurchaseorderdetailComponent } from './purchaseorderdetail.component';

describe('PurchaseorderdetailComponent', () => {
  let component: PurchaseorderdetailComponent;
  let fixture: ComponentFixture<PurchaseorderdetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PurchaseorderdetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PurchaseorderdetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
