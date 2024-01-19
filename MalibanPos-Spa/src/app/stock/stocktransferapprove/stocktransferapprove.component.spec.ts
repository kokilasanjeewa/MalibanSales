import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StocktransferapproveComponent } from './stocktransferapprove.component';

describe('StocktransferapproveComponent', () => {
  let component: StocktransferapproveComponent;
  let fixture: ComponentFixture<StocktransferapproveComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StocktransferapproveComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StocktransferapproveComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
