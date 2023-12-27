import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReceiptdetailsbydaterangeComponent } from './receiptdetailsbydaterange.component';

describe('ReceiptdetailsbydaterangeComponent', () => {
  let component: ReceiptdetailsbydaterangeComponent;
  let fixture: ComponentFixture<ReceiptdetailsbydaterangeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReceiptdetailsbydaterangeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReceiptdetailsbydaterangeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
