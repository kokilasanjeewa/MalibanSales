import { AuthService } from './../../_services/auth.service';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { CommonService } from 'src/app/_services/common.service';
import { BranchLocation } from 'src/app/models/branchlocation';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Subject } from 'rxjs';


@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit {
  jwtHelper = new JwtHelperService();
  userName: string;
  branches: BranchLocation[];
  changeBranchForm: FormGroup;
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private authService: AuthService,
    private alertifyService: AlertifyService,
    private commonService : CommonService,
    private fb: FormBuilder,

  ) {
      this.getBranches();
      this.changeBranchControl();
   }

  ngOnInit() {
    const token = localStorage.getItem('token');
    if (token) {
      this.authService.decodedToken = this.jwtHelper.decodeToken(token);
      this.userName=this.commonService.getUser();
    }
  }
  logout() {
    return this.authService.logOut();
  }
  loggedIn() {
    return this.authService.loggedIn();
  }
   getBranches(){
    this.commonService.getList('Common', 'getallbranch').subscribe(data => {
      const mapBranches=this.commonService.getMapUserBranchLocations().length >0 ?  this.commonService.getMapUserBranchLocations() : this.commonService.getUserBranchLocationId();
      const branches = mapBranches.length >1 ?
      data.filter(array => mapBranches.some(filter => filter == array.branchLocationId))
      : data.filter(d => d.branchLocationId==this.commonService.getUserBranchLocationId());
      this.branches =branches;
    });
  }
  changeBranch()
  {
    localStorage.setItem('branchLocationId', JSON.stringify(this.changeBranchForm.get('branchLocationId').value));
    this.router.navigate(['/dashboard']);

  }
  get f() {
    return this.changeBranchForm.controls;
  }
  changeBranchControl() {
    this.changeBranchForm  = this.fb.group({
      "branchLocationId": [this.commonService.getUserBranchLocationId(), Validators.required]
  });

  }
}
