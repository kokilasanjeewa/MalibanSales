import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditcustomermastersettingComponent } from './editcustomermastersetting.component';

describe('EditcustomermastersettingComponent', () => {
  let component: EditcustomermastersettingComponent;
  let fixture: ComponentFixture<EditcustomermastersettingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditcustomermastersettingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditcustomermastersettingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
