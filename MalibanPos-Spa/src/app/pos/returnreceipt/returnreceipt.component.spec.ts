import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReturnreceiptComponent } from './returnreceipt.component';

describe('ReturnreceiptComponent', () => {
  let component: ReturnreceiptComponent;
  let fixture: ComponentFixture<ReturnreceiptComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReturnreceiptComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReturnreceiptComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
