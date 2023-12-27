import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DatewisesaledetailsummaryComponent } from './datewisesaledetailsummary.component';

describe('DatewisesaledetailsummaryComponent', () => {
  let component: DatewisesaledetailsummaryComponent;
  let fixture: ComponentFixture<DatewisesaledetailsummaryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DatewisesaledetailsummaryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DatewisesaledetailsummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
