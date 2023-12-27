import { Directive, HostListener } from '@angular/core';
import { NavigationService } from '../_services/navigation.service';

@Directive({
  selector: 'back-Button'
})
export class BackButtonDirective {

  constructor(private navigation: NavigationService) { }

  @HostListener('click')
  onClick(): void {
    this.navigation.goBack();
  }

}
