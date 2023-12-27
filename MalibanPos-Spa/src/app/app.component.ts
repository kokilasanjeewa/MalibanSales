import { AuthService } from './_services/auth.service';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import * as $ from 'jquery';
import { CommonService } from './_services/common.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  showHeader = false;
  showSidebar = false;
  showFooter = false;
  title = 'MalibanPos-Spa';
  valueEmittedFromChildComponent: string = '';
  jwtHelper = new JwtHelperService();

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private authService: AuthService,
    private commonService: CommonService,
  ) {

   }
  ngOnInit() {
    $(document).ready(function(){
      $(".wrapper").addClass("collapse");
  });
    this.router.events.subscribe((event) => {
      /*       https://www.bennadel.com/blog/3545-enabling-the-second-click-of-a-routerlink-fragment-using-onsameurlnavigation-reload-in-angular-7-1-3.htm */
      if (event instanceof NavigationEnd) {
        this.showHeader =
          this.activatedRoute.firstChild.snapshot.data.showHeader !== false;
        this.showSidebar =
          this.activatedRoute.firstChild.snapshot.data.showSidebar !== false;
        this.showFooter =
          this.activatedRoute.firstChild.snapshot.data.showFooter !== false;
      }
    });
/*       const token = localStorage.getItem('token');
    if (token) {
      this.authService.decodedToken = this.jwtHelper.decodeToken(token);
          this.rolesService.update(["User"]);

    }   */
 /*    this.commonService.getListByLocation(2, 'Common', 'getallcustomerdetail', '').subscribe((data) => {
      localStorage.setItem("ListaPermessi", JSON.stringify(data));
    }); */

  }
  parrentEventHandlerFunction(valueEmitted) {
    this.valueEmittedFromChildComponent = valueEmitted;
    console.log('hi from parent');
  }
}
