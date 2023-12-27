import { TransactionDetail } from './../../models/transactiondetail';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-pricemodal',
  templateUrl: './pricemodal.component.html',
  styleUrls: ['./pricemodal.component.css']
})
export class PricemodalComponent implements OnInit {
  list: any;
  @Output() updateItemAdd = new EventEmitter();
  constructor(public bsModalRef: BsModalRef,public modalService: BsModalService) { }

  ngOnInit() {
    this.list =this.modalService.config.initialState;
  //  console.log(this.list);
  }
  addStyle(index : number) {
    this.bsModalRef.hide();
    this.updateItemAdd.emit(index);
  }
  close() {
    this.bsModalRef.hide();
  }
}
