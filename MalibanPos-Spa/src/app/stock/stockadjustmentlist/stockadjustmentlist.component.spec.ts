import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StockadjustmentlistComponent } from './stockadjustmentlist.component';

describe('StockadjustmentlistComponent', () => {
  let component: StockadjustmentlistComponent;
  let fixture: ComponentFixture<StockadjustmentlistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StockadjustmentlistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StockadjustmentlistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
