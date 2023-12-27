import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UploadcustomerComponent } from './uploadcustomer.component';

describe('UploadcustomerComponent', () => {
  let component: UploadcustomerComponent;
  let fixture: ComponentFixture<UploadcustomerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UploadcustomerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UploadcustomerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
