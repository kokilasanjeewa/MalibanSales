//https://stackoverflow.com/questions/58138928/how-to-call-a-javascript-function-from-a-typescript-class-that-works-with-intern

export declare function test1(receiptPrint);
/* You could call javascript function in .ts file like below:

Put the .js file in assets/javascript folder. For example, I use demo.js here:

export function test1(){
  console.log('Calling test 1 function');
}
Create demo.d.ts file in assets/javascript folder. The .js file and the .d.ts file should have the same name:

export declare function test1();
Call the javascript function in component.ts file:

import { test1 } from '../assets/javascript/demo';
...
export class AppComponent {
  constructor() {
    test1();
  } */
