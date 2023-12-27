import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StockadjustmentconfirmComponent } from './stockadjustmentconfirm.component';

describe('StockadjustmentconfirmComponent', () => {
  let component: StockadjustmentconfirmComponent;
  let fixture: ComponentFixture<StockadjustmentconfirmComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StockadjustmentconfirmComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StockadjustmentconfirmComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
