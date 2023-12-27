import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ItemwisemovingnonmovingstockComponent } from './itemwisemovingnonmovingstock.component';

describe('ItemwisemovingnonmovingstockComponent', () => {
  let component: ItemwisemovingnonmovingstockComponent;
  let fixture: ComponentFixture<ItemwisemovingnonmovingstockComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ItemwisemovingnonmovingstockComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ItemwisemovingnonmovingstockComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
