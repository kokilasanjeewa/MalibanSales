import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExpirationstockComponent } from './expirationstock.component';

describe('ExpirationstockComponent', () => {
  let component: ExpirationstockComponent;
  let fixture: ComponentFixture<ExpirationstockComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExpirationstockComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExpirationstockComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
