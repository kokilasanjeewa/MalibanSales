import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Observable } from 'rxjs';
import { CommonService } from 'src/app/_services/common.service';
import { filter } from 'rxjs/operators';
import { Card } from 'src/app/models/card';
@Component({
  selector: 'app-checkoutcashmodal',
  templateUrl: './checkoutcashmodal.component.html',
  styleUrls: ['./checkoutcashmodal.component.css']
})
export class CheckoutcashmodalComponent implements OnInit {
  addPaymentForm: FormGroup;
  @Output() updateErrorAdd = new EventEmitter<{ paymentReferenceId: string, cardType: number }>();
  paidpaymentReferenceId: string
  cardType: number=0 ;
  totalAmount: string = '';
  cardTypeList: Observable<Card[]>;
  card: Card;
  constructor(private fb: FormBuilder,
    public bsModalRef: BsModalRef, public modalService: BsModalService,
    private commonService: CommonService,

  ) { }

  ngOnInit() {
    this.createPaymentForm();
    this.cardTypeList = this.commonService.getListByLocation(this.commonService.getUserBranchLocationId(), 'Common', 'getallcardtype');
    this.totalAmount = Number(this.modalService.config.initialState).toFixed(2);
  }
  createPaymentForm() {
    this.addPaymentForm = this.fb.group({
      paymentReferenceId: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(24)]],
      cardType: [0, [Validators.required]],
      totalAmount: ['', [Validators.required]]
    });
  }
  onAddPay() {
    this.updateErrorAdd.emit({ paymentReferenceId: this.paidpaymentReferenceId, cardType: this.cardType });
    this.close();
    this.addPaymentForm.reset();
  }
  close() {
    this.bsModalRef.hide();
  }
  get f() {
    return this.addPaymentForm.controls;
  }
  changePayment() {
    if(this.cardType>0)
      {
        this.commonService.getListByLocation(this.commonService.getUserBranchLocationId(), 'Common', 'getallcardtype').subscribe((data: Card[]) => {
          this.card = data.find((user: Card) => { return user.cardMethodId == this.cardType });
          this.totalAmount = ((Number(this.modalService.config.initialState)) * (1 + this.card.serviceCharge / 100)).toFixed(2)

        });
      //  console.log(this.card.serviceCharge)
      }
  }
}
