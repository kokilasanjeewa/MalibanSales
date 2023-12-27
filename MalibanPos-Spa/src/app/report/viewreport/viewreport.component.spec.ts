import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewreportComponent } from './viewreport.component';

describe('ViewreportComponent', () => {
  let component: ViewreportComponent;
  let fixture: ComponentFixture<ViewreportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewreportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewreportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
