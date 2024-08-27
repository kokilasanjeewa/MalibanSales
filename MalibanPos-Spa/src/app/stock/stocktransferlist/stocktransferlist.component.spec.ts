import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StocktransferlistComponent } from './stocktransferlist.component';

describe('StocktransferlistComponent', () => {
  let component: StocktransferlistComponent;
  let fixture: ComponentFixture<StocktransferlistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StocktransferlistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StocktransferlistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
