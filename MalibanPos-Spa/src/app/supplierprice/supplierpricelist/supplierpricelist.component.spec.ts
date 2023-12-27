import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SupplierpricelistComponent } from './supplierpricelist.component';

describe('SupplierpricelistComponent', () => {
  let component: SupplierpricelistComponent;
  let fixture: ComponentFixture<SupplierpricelistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SupplierpricelistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SupplierpricelistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
