import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-purchaseorderapprovemodel',
  templateUrl: './purchaseorderapprovemodel.component.html',
  styleUrls: ['./purchaseorderapprovemodel.component.css']
})
export class PurchaseorderapprovemodelComponent implements OnInit {
  addPurchaseOrderApproveForm: FormGroup;
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
    this.addPurchaseOrderApproveForm = this.fb.group({
      cApprovalId: ['', [Validators.required]]
    });
  }
  onApprove() {
    this.cApprovalId=this.addPurchaseOrderApproveForm.get('cApprovalId')?.value;
    this.updateErrorAdd.emit({ cApprovalId: this.cApprovalId });
    this.close();
    this.addPurchaseOrderApproveForm.reset();
  }
  close() {
    this.bsModalRef.hide();
  }
  get f() {
    return this.addPurchaseOrderApproveForm.controls;
  }

}
