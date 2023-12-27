import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditgrnComponent } from './editgrn.component';

describe('EditgrnComponent', () => {
  let component: EditgrnComponent;
  let fixture: ComponentFixture<EditgrnComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditgrnComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditgrnComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
