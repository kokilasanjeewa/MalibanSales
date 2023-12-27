import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PurchaseorderfilterlistComponent } from './purchaseorderfilterlist.component';

describe('PurchaseorderfilterlistComponent', () => {
  let component: PurchaseorderfilterlistComponent;
  let fixture: ComponentFixture<PurchaseorderfilterlistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PurchaseorderfilterlistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PurchaseorderfilterlistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
