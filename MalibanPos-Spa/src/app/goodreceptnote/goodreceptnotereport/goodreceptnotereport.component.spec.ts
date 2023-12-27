import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GoodreceptnotereportComponent } from './goodreceptnotereport.component';

describe('GoodreceptnotereportComponent', () => {
  let component: GoodreceptnotereportComponent;
  let fixture: ComponentFixture<GoodreceptnotereportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GoodreceptnotereportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GoodreceptnotereportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
