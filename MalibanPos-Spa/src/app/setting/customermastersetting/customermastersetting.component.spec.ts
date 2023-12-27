import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomermastersettingComponent } from './customermastersetting.component';

describe('CustomermastersettingComponent', () => {
  let component: CustomermastersettingComponent;
  let fixture: ComponentFixture<CustomermastersettingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CustomermastersettingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomermastersettingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
