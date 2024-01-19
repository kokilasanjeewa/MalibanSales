import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StocktransfereditComponent } from './stocktransferedit.component';

describe('StocktransfereditComponent', () => {
  let component: StocktransfereditComponent;
  let fixture: ComponentFixture<StocktransfereditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StocktransfereditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StocktransfereditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
