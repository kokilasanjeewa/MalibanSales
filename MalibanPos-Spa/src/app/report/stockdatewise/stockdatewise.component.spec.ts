import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StockdatewiseComponent } from './stockdatewise.component';

describe('StockdatewiseComponent', () => {
  let component: StockdatewiseComponent;
  let fixture: ComponentFixture<StockdatewiseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StockdatewiseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StockdatewiseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
