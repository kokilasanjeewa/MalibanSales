import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PurchaseorderlistComponent } from './purchaseorderlist.component';

describe('PurchaseorderlistComponent', () => {
  let component: PurchaseorderlistComponent;
  let fixture: ComponentFixture<PurchaseorderlistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PurchaseorderlistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PurchaseorderlistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
