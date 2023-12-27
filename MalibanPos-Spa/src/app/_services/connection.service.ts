import { Injectable } from "@angular/core";
import { ToastrService } from "ngx-toastr";
import { Observable, Subject, Subscription, fromEvent } from "rxjs";
@Injectable()
export class ConnectionService {
  private connEvents: Subject<boolean>;
  constructor(    private toastr: ToastrService,
    ) {
    this.connEvents = new Subject<boolean>();
    window.addEventListener("online", (e) => this.handleConnectionChange(e));
    window.addEventListener("offline", (e) => this.handleConnectionChange(e));
    this.checkOnlineStatus;
  }
  private handleConnectionChange(event) {
    this.connEvents.next(this.connected);
  }
  get connected(): boolean {
    return window.navigator.onLine;
  }
  get Changes(): Observable<boolean> {
    return this.connEvents;
  }
  // Observables & Subscriptions to check online/offline connectivity status
  onlineEvent: Observable<Event>;
  offlineEvent: Observable<Event>;
  subscriptions: Subscription[] = [];

//Get the online/offline connectivity status from browser window
checkOnlineStatus = () => {
    this.onlineEvent = fromEvent(window, 'online');
    this.offlineEvent = fromEvent(window, 'offline');

    this.subscriptions.push(this.onlineEvent.subscribe(() =>     this.toastr.success('Connected!')));
    this.subscriptions.push(this.offlineEvent.subscribe(() => this.toastr.error('Check Connectivity!')));
  }
}
