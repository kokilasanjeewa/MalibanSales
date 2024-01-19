import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StocktransferreceivemodelComponent } from './stocktransferreceivemodel.component';

describe('StocktransferreceivemodelComponent', () => {
  let component: StocktransferreceivemodelComponent;
  let fixture: ComponentFixture<StocktransferreceivemodelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StocktransferreceivemodelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StocktransferreceivemodelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
