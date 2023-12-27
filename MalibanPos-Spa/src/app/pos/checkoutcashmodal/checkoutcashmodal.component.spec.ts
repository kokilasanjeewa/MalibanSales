import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CheckoutcashmodalComponent } from './checkoutcashmodal.component';

describe('CheckoutcashmodalComponent', () => {
  let component: CheckoutcashmodalComponent;
  let fixture: ComponentFixture<CheckoutcashmodalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CheckoutcashmodalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CheckoutcashmodalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
