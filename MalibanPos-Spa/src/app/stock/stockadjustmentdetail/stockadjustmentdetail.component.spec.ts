import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StockadjustmentdetailComponent } from './stockadjustmentdetail.component';

describe('StockadjustmentdetailComponent', () => {
  let component: StockadjustmentdetailComponent;
  let fixture: ComponentFixture<StockadjustmentdetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StockadjustmentdetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StockadjustmentdetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
