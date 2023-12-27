import { Subject } from 'rxjs';
import { Injectable } from '@angular/core';
import { Subscription } from 'rxjs';
import { filter, map } from 'rxjs/operators';


@Injectable()
export class EventBusService {
  private subject$ = new Subject();
  on(event: Events, action: any): Subscription {
    return this.subject$
      .pipe(
        filter((e: EmitEvent) => e.name === event),
        map((e: EmitEvent) => e.value)
      )
      .subscribe(action);
  }
  emit(event: EmitEvent) {
    this.subject$.next(event);
  }

}
export class EmitEvent {

  constructor(public name: any, public value?: any) { }

}

export enum Events {
  CustomerSelected,
  DeleteCustomer,
  AddCustomer
}
/* https://www.youtube.com/watch?v=_q-HL9YX_pk

inject eventbus service in constructor on another component to publish data

constructor(private eventBusService: EventBusService){}

selectCustomer(cust: Customer){
  this.eventBusService.emit(new EmitEvent(Events.CustomerSelected,))
}
inject eventbus service in constructor on another component to subscribe data

constructor(private eventBusService: EventBusService){}

  this.eventBusService.on(Events.CustomerSelected,(cust => {this.customer=cust}))

*/
