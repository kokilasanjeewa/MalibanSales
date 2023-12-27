import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MapcustomerComponent } from './mapcustomer.component';

describe('MapcustomerComponent', () => {
  let component: MapcustomerComponent;
  let fixture: ComponentFixture<MapcustomerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MapcustomerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MapcustomerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
