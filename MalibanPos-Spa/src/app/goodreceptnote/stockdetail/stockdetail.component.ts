import { CommonService } from 'src/app/_services/common.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-stockdetail',
  templateUrl: './stockdetail.component.html',
  styleUrls: ['./stockdetail.component.css']
})
export class StockdetailComponent implements OnInit {
  title: string;
  barcode: string;
  items$: Observable<[]>;

  constructor(
    private router: Router,
    private toastrService: ToastrService,
    private commonService: CommonService,
    public bsModalRef: BsModalRef) { }

  ngOnInit() {
    this.items$ = this.commonService.getListByLocation(this.commonService.getUserBranchLocationId(), 'Common', 'getallstockdetail', this.barcode);

  }

}
