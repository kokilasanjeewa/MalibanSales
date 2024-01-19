import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-stocktransferapprovemodel',
  templateUrl: './stocktransferapprovemodel.component.html',
  styleUrls: ['./stocktransferapprovemodel.component.css']
})
export class StocktransferapprovemodelComponent implements OnInit {
  addApproveForm: FormGroup;
  @Output() updateErrorAdd = new EventEmitter<{cApprovalId:number}>();
  cApprovalId:number;
  cApprovalTypes: {};
  constructor(private fb: FormBuilder,
    public bsModalRef: BsModalRef,public modalService: BsModalService) {
      this.cApprovalTypes = [
        { id: 1, name: 'Approve' },
        { id: 2, name: 'Reject' },
        { id: 3, name: 'Cancel' },
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
