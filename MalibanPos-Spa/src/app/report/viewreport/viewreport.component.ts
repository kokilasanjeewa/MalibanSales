import { ReportService } from "./../../_services/report.service";
import { CommonService } from "src/app/_services/common.service";
import { Component, OnInit } from "@angular/core";
import { DomSanitizer } from "@angular/platform-browser";
import { Observable } from "rxjs";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router, ActivatedRoute } from "@angular/router";
import { ToastrService } from "ngx-toastr";
@Component({
  templateUrl: "./viewreport.component.html",
  styleUrls: ["./viewreport.component.css"],
})
export class ViewreportComponent implements OnInit {
  addReportForm: FormGroup;
  reports: Observable<[]>;
  constructor(
    private commonService: CommonService,
    private fb: FormBuilder,
    private router: Router
  ) {
    this.reports = this.commonService.getList("Common", "getallreport");
  }

  ngOnInit() {
    this.createReportForm();
  }
  /*   https://www.storehub.com/blog/pos-data-and-reports/
   */ reportAdd() {
    const reportName = this.addReportForm.get("reportName").value;
    if (!this.addReportForm.valid) {
      return false;
    } else {
      switch (reportName) {
        case "Customer Deduction Details":
          this.router.navigate(["/report/customer"]);
          break;
        case "Customer Deduction Unpaid Details":
          this.router.navigate(["/report/unpaidcustomer"]);
          break;
        case "Stock Details":
          this.router.navigate(["/report/stock"]);
          break;
        case "Sales Details":
          this.router.navigate(["/report/sale"]);
          break;
        case "Sales Summary Details":
          this.router.navigate(["/report/salesummary"]);
          break;
        case "Sales Summary By Customer":
          this.router.navigate(["/report/salesummarybycustomer"]);
          break;
        case "Sale Return":
          this.router.navigate(["/report/salereturn"]);
          break;
        case "Purchase Return":
          this.router.navigate(["/report/purchasereturn"]);
          break;
        case "Purchase Order":
          this.router.navigate(["/report/purchaseorder"]);
          break;
        case "Good Receipt Note":
          this.router.navigate(["/report/goodreceiptnote"]);
          break;
        case "Stock Adjustment":
          this.router.navigate(["/report/stockadjustment"]);
          break;
        case "Supplier List":
          this.router.navigate(["/report/supplierlist"]);
          break;
        case "Sales Details By Customer":
          this.router.navigate(["/report/salesdetailsbycustomer"]);
          break;
        case "Item Wise Bin Card":
          this.router.navigate(["/report/bincard"]);
          break;
        case "Date Wise Product Sales":
          this.router.navigate(["/report/datewiseproductsales"]);
          break;
        case "Date Wise Lunch Packet Summary":
          this.router.navigate(["/report/datewiselunchpacketsummary"]);
          break;
        case "Date Wise Receipt Summary":
          this.router.navigate(["/report/datewisereceiptsummary"]);
          break;
        case "Date Wise Stock Details":
          this.router.navigate(["/report/datewisestockdetails"]);
          break;
        case "Item Wise Sale Details Summary":
          this.router.navigate(["/report/datewisesaledetailsummary"]);
          break;
        case "Item Wise Moving Nonmoving Stock":
          this.router.navigate(["/report/itemwisemovingnonmovingstock"]);
          break;
        case "Item Expiry Report":
          this.router.navigate(["/report/expirationstock"]);
          break;
        case "Purchase Order List":
          this.router.navigate(["/report/purchaseorderfilterlist"]);
          break;
      }
    }
  }
  get f() {
    return this.addReportForm.controls;
  }
  createReportForm() {
    this.addReportForm = this.fb.group({
      reportName: ["", [Validators.required]],
    });
  }
}
