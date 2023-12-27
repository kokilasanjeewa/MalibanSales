import { Component, OnInit } from '@angular/core';
import { AuthService } from './../../_services/auth.service';
import { PaginatedResult } from './../../models/pagination';
import { CommonService } from './../../_services/common.service';
import { ReportService } from './../../_services/report.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CustomerDetail } from '../../models/customerdetail';
import * as fileSaver from 'file-saver';
import * as $ from 'jquery';
import { Observable } from 'rxjs';
import { AlertifyService } from './../../_services/alertify.service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { CustomerService } from 'src/app/_services/customer.service';
@Component({
  selector: 'app-uploadcustomer',
  templateUrl: './uploadcustomer.component.html',
  styleUrls: ['./uploadcustomer.component.css']
})
export class UploadcustomerComponent implements OnInit {
  customerReportForm: FormGroup;
  customerDetail: CustomerDetail[];
  selectedFile: File = null;

  constructor(
    private commonService: CommonService,
    private fb: FormBuilder,
    private router: Router,
    private toastrService: ToastrService,
    private customerService: CustomerService,
  ) {
   }

  ngOnInit() {
    this.createCustomerForm();
    $('.custom-file-input').on('change', function () {
      const fileName = $(this).val().split('\\').pop();
      $(this).next('.custom-file-label').html(fileName);
    });
   }
   get f() {
    return this.customerReportForm.controls;
  }
  createCustomerForm() {
    this.customerReportForm = this.fb.group({
      uploadFile: ['', [Validators.required]],
    });
  }
   onFileChange(event) {
    this.selectedFile = <File>event.target.files[0];
    console.log(this.selectedFile);
  }
  customerAdd() {
    const formData: any = new FormData();
    formData.append('Files', this.selectedFile);
    if (!this.customerReportForm.valid) {
      return false;
    } else {
      this.customerService.uploadExcel(formData).subscribe(
        (res) => {
          console.log(res);
        },
        (error) => {
          this.toastrService.error(
            JSON.stringify(error).replace('"', '').replace('"', '')
          );
          console.log(error.error);
        },
        () => {
          this.toastrService.success('Data Loading Successfully.');
          this.router.navigate(['/customer/uploadcustomer']);
        }
      );
      this.customerService.createCustomer(formData).subscribe(
        (res) => {
          console.log(res);
        },
        (error) => {
          this.toastrService.error(
            JSON.stringify(error).replace('"', '').replace('"', '')
          );
          console.log(error.error);
        },
        () => {
          this.toastrService.success('Data Loading Successfully.');
          this.router.navigate(['/customer/customerlist']);
        }
      );
    }
  }

}
