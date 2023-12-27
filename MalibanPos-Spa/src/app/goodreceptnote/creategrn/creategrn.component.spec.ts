import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreategrnComponent } from './creategrn.component';

describe('CreategrnComponent', () => {
  let component: CreategrnComponent;
  let fixture: ComponentFixture<CreategrnComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreategrnComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreategrnComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
