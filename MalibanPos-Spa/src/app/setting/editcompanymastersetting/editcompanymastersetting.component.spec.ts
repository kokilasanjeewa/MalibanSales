import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditcompanymastersettingComponent } from './editcompanymastersetting.component';

describe('EditcompanymastersettingComponent', () => {
  let component: EditcompanymastersettingComponent;
  let fixture: ComponentFixture<EditcompanymastersettingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditcompanymastersettingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditcompanymastersettingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
