import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomermastersettinglistComponent } from './customermastersettinglist.component';

describe('CustomermastersettinglistComponent', () => {
  let component: CustomermastersettinglistComponent;
  let fixture: ComponentFixture<CustomermastersettinglistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CustomermastersettinglistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomermastersettinglistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
