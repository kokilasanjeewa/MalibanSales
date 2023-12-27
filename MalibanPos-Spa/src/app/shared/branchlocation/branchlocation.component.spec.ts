import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BranchlocationComponent } from './branchlocation.component';

describe('BranchlocationComponent', () => {
  let component: BranchlocationComponent;
  let fixture: ComponentFixture<BranchlocationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BranchlocationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BranchlocationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
