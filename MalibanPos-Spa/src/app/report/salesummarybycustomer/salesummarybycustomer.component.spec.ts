import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SalesummarybycustomerComponent } from './salesummarybycustomer.component';

describe('SalesummarybycustomerComponent', () => {
  let component: SalesummarybycustomerComponent;
  let fixture: ComponentFixture<SalesummarybycustomerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SalesummarybycustomerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SalesummarybycustomerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
