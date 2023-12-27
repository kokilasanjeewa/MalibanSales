import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StockadjustmentapproveComponent } from './stockadjustmentapprove.component';

describe('StockadjustmentapproveComponent', () => {
  let component: StockadjustmentapproveComponent;
  let fixture: ComponentFixture<StockadjustmentapproveComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StockadjustmentapproveComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StockadjustmentapproveComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
