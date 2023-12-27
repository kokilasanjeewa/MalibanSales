import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MessagemodelComponent } from './messagemodel.component';

describe('MessagemodelComponent', () => {
  let component: MessagemodelComponent;
  let fixture: ComponentFixture<MessagemodelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MessagemodelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MessagemodelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
