import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PurchaseorderapprovemodelComponent } from './purchaseorderapprovemodel.component';

describe('PurchaseorderapprovemodelComponent', () => {
  let component: PurchaseorderapprovemodelComponent;
  let fixture: ComponentFixture<PurchaseorderapprovemodelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PurchaseorderapprovemodelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PurchaseorderapprovemodelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
