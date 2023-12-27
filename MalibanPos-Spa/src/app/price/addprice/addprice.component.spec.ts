import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddpriceComponent } from './addprice.component';

describe('AddpriceComponent', () => {
  let component: AddpriceComponent;
  let fixture: ComponentFixture<AddpriceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddpriceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddpriceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
