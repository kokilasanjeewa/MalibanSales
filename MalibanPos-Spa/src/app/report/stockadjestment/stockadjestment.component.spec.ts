import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StockadjestmentComponent } from './stockadjestment.component';

describe('StockadjestmentComponent', () => {
  let component: StockadjestmentComponent;
  let fixture: ComponentFixture<StockadjestmentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StockadjestmentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StockadjestmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
