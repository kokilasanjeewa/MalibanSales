import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReceptComponent } from './recept.component';

describe('ReceptComponent', () => {
  let component: ReceptComponent;
  let fixture: ComponentFixture<ReceptComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReceptComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReceptComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
