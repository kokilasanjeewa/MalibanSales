import { Directive, ElementRef, AfterViewInit } from '@angular/core';
import { fromEvent } from 'rxjs';

@Directive({
  selector: '[stopPropagation]',
})
export class StopPropagationDirective implements AfterViewInit {
  constructor(private elementRef: ElementRef) {}

  public ngAfterViewInit() {
    fromEvent<MouseEvent>(this.elementRef.nativeElement, 'click', {
      capture: true,
    }).subscribe((event) => {
      console.log('catched');
      event.preventDefault();
      event.stopPropagation();
    });
  }
}
