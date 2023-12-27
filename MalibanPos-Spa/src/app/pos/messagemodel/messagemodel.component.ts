import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
@Component({
  selector: 'app-messagemodel',
  templateUrl: './messagemodel.component.html',
  styleUrls: ['./messagemodel.component.css']
})
export class MessagemodelComponent implements OnInit {
  @Output() updateErrorAdd = new EventEmitter();
  message: any;

  constructor(public bsModalRef: BsModalRef,public modalService: BsModalService) { }

  ngOnInit() {
    this.message =this.modalService.config.initialState;
    console.log(this.message);
  }
  close() {
    this.bsModalRef.hide();
  }
}
