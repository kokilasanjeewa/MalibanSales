import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GrnlistComponent } from './grnlist.component';

describe('GrnlistComponent', () => {
  let component: GrnlistComponent;
  let fixture: ComponentFixture<GrnlistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GrnlistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GrnlistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
