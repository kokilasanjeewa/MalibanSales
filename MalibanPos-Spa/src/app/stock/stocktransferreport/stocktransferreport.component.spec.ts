import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StocktransferreportComponent } from './stocktransferreport.component';

describe('StocktransferreportComponent', () => {
  let component: StocktransferreportComponent;
  let fixture: ComponentFixture<StocktransferreportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StocktransferreportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StocktransferreportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
