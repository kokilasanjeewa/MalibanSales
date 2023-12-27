import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DatewiselunchpacketsummaryComponent } from './datewiselunchpacketsummary.component';

describe('DatewiselunchpacketsummaryComponent', () => {
  let component: DatewiselunchpacketsummaryComponent;
  let fixture: ComponentFixture<DatewiselunchpacketsummaryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DatewiselunchpacketsummaryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DatewiselunchpacketsummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
