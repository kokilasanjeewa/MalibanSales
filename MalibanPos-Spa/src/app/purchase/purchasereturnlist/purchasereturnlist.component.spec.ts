import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PurchasereturnlistComponent } from './purchasereturnlist.component';

describe('PurchasereturnlistComponent', () => {
  let component: PurchasereturnlistComponent;
  let fixture: ComponentFixture<PurchasereturnlistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PurchasereturnlistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PurchasereturnlistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
