import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GoodreceiptnoteComponent } from './goodreceiptnote.component';

describe('GoodreceiptnoteComponent', () => {
  let component: GoodreceiptnoteComponent;
  let fixture: ComponentFixture<GoodreceiptnoteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GoodreceiptnoteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GoodreceiptnoteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
