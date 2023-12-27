import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddsupplierpriceComponent } from './addsupplierprice.component';

describe('AddsupplierpriceComponent', () => {
  let component: AddsupplierpriceComponent;
  let fixture: ComponentFixture<AddsupplierpriceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddsupplierpriceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddsupplierpriceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
