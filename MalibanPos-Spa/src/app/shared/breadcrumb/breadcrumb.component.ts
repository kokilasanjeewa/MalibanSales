import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'bread-crumb',
  templateUrl: './breadcrumb.component.html',
  styleUrls: ['./breadcrumb.component.css']
})
export class BreadcrumbComponent implements OnInit {
  @Input() data: string[];
  @Input() index: number;

  @Input() filter = () => true;
  @Output() outputFromChild : EventEmitter<string> = new EventEmitter();

  constructor() { }

  ngOnInit(): void {
  }
  get displayValue(): string[] {
    return this.data.filter(this.filter);
  }
  getClassMap(index : number): string {
    console.log(index);
    return this.index==index ? "active": "";
   }
}
