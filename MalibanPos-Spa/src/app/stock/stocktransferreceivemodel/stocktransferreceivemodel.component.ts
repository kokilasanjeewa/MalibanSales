import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
@Component({
  selector: 'app-stocktransferreceivemodel',
  templateUrl: './stocktransferreceivemodel.component.html',
  styleUrls: ['./stocktransferreceivemodel.component.css']
})
export class StocktransferreceivemodelComponent implements OnInit {
  addApproveForm: FormGroup;
  @Output() updateErrorAdd = new EventEmitter<{cApprovalId:number}>();
  cApprovalId:number;
  cApprovalTypes: {};
  constructor(private fb: FormBuilder,
    public bsModalRef: BsModalRef,public modalService: BsModalService) {
      this.cApprovalTypes = [
        { id: 4, name: 'Receive' },
        { id: 3, name: 'Cancel' }
      ];
     }

  ngOnInit() {
    this.createPaymentForm();
  }
  createPaymentForm() {
    this.addApproveForm = this.fb.group({
      cApprovalId: ['', [Validators.required]]
    });
  }
  onApprove() {
    this.cApprovalId=this.addApproveForm.get('cApprovalId')?.value;
    this.updateErrorAdd.emit({ cApprovalId: this.cApprovalId });
    this.close();
    this.addApproveForm.reset();
  }
  close() {
    this.bsModalRef.hide();
  }
  get f() {
    return this.addApproveForm.controls;
  }

}
