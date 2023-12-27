import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PurchaseorderconfirmComponent } from './purchaseorderconfirm.component';

describe('PurchaseorderconfirmComponent', () => {
  let component: PurchaseorderconfirmComponent;
  let fixture: ComponentFixture<PurchaseorderconfirmComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PurchaseorderconfirmComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PurchaseorderconfirmComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
