import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GrndetailComponent } from './grndetail.component';

describe('GrndetailComponent', () => {
  let component: GrndetailComponent;
  let fixture: ComponentFixture<GrndetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GrndetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GrndetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
