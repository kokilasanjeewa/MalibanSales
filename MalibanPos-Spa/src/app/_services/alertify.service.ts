import { Injectable } from '@angular/core';
import * as alertify from 'alertifyjs';
import * as $ from 'jquery';

@Injectable({
  providedIn: 'root',
})
export class AlertifyService {
  constructor() { }
  confirm(
    title: string,
    message: string,
    okCallback: () => any,
    cancelCallback: () => any
  ) {
    alertify
      .confirm(
        title,
        message,
        () => {
          okCallback();
        },
        () => {
          cancelCallback();
        }
      );

  }
  success(message: string) {
  //  alertify.success(message);
    var msg = alertify.success(message);
 msg.delay(1).setContent(message);
  }
  error(message: string) {
    alertify.error(message);
  }
  warning(message: string) {
    alertify.warning(message);
  }
  message(message: string) {
    alertify.message(message);
  }
}
