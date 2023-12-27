import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PricemodalComponent } from './pricemodal.component';

describe('PricemodalComponent', () => {
  let component: PricemodalComponent;
  let fixture: ComponentFixture<PricemodalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PricemodalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PricemodalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
