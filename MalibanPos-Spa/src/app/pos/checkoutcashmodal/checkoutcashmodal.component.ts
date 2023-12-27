import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
@Component({
  selector: 'app-checkoutcashmodal',
  templateUrl: './checkoutcashmodal.component.html',
  styleUrls: ['./checkoutcashmodal.component.css']
})
export class CheckoutcashmodalComponent implements OnInit {
  addPaymentForm: FormGroup;
  @Output() updateErrorAdd = new EventEmitter<{paymentReferenceId:string}>();
  paidpaymentReferenceId:string
  constructor(private fb: FormBuilder,
    public bsModalRef: BsModalRef,public modalService: BsModalService) { }

  ngOnInit() {
    this.createPaymentForm();
  }
  createPaymentForm() {
    this.addPaymentForm = this.fb.group({
      paymentReferenceId: ['', [Validators.required,Validators.minLength(12),Validators.maxLength(12)]]
    });
  }
  onAddPay() {
    this.updateErrorAdd.emit({ paymentReferenceId: this.paidpaymentReferenceId });
    this.close();
    this.addPaymentForm.reset();
  }
  close() {
    this.bsModalRef.hide();
  }
  get f() {
    return this.addPaymentForm.controls;
  }
}
