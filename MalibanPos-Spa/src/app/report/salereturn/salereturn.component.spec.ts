import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SalereturnComponent } from './salereturn.component';

describe('SalereturnComponent', () => {
  let component: SalereturnComponent;
  let fixture: ComponentFixture<SalereturnComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SalereturnComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SalereturnComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
