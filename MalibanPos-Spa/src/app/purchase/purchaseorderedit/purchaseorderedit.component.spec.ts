import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PurchaseordereditComponent } from './purchaseorderedit.component';

describe('PurchaseorderComponent', () => {
  let component: PurchaseordereditComponent;
  let fixture: ComponentFixture<PurchaseordereditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PurchaseordereditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PurchaseordereditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
