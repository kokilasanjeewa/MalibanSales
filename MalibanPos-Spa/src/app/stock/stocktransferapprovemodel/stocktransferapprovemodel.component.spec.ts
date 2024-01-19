import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StocktransferapprovemodelComponent } from './stocktransferapprovemodel.component';

describe('StocktransferapprovemodelComponent', () => {
  let component: StocktransferapprovemodelComponent;
  let fixture: ComponentFixture<StocktransferapprovemodelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StocktransferapprovemodelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StocktransferapprovemodelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
