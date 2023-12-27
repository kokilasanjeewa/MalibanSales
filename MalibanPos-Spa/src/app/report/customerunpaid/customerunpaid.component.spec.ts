import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerunpaidComponent } from './customerunpaid.component';

describe('CustomerunpaidComponent', () => {
  let component: CustomerunpaidComponent;
  let fixture: ComponentFixture<CustomerunpaidComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CustomerunpaidComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomerunpaidComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
