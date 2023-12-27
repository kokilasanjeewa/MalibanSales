import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ItemwisesaledetailsbydaterangeComponent } from './itemwisesaledetailsbydaterange.component';

describe('ItemwisesaledetailsbydaterangeComponent', () => {
  let component: ItemwisesaledetailsbydaterangeComponent;
  let fixture: ComponentFixture<ItemwisesaledetailsbydaterangeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ItemwisesaledetailsbydaterangeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ItemwisesaledetailsbydaterangeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
