import { AuthService } from './../../_services/auth.service';
import { Component, OnInit } from '@angular/core';
import * as $ from 'jquery';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css'],
})
export class SidebarComponent implements OnInit {
  jwtHelper = new JwtHelperService();
  userRoles : Array<string>;
  constructor(public authService: AuthService) { }

  ngOnInit() {
      
      $('#slide-bar-btn-nav').click(function () {
      $('.wrapper').toggleClass('collapse');
      $('footer').toggleClass('collapse');
    });
    $('.sidebar .menu-btn .sub-menu').click(function () {
      $('.wrapper').toggleClass('collapse');
      $('footer').toggleClass('collapse');
    });
/*     $('.sidebar .menu-btn').click(function () {
      $('.wrapper').toggleClass('collapse');
      $('footer').toggleClass('collapse');
    }); */
    $('#top-sub-menu-profile').click(function () {
      $('.top-sub-menu').toggleClass('collapse');
    });
    //  $('.fa.fa-chevron-down.drop-down').click(function () { $('.sub-menu').toggle(); });
    //  $('.sidebar').click(this.handler).find('.sub-menu').hide();
    //  $('.sidebar').click(this.handler).find('.sub-menu').hide();
    $(".sidebar .menu-btn").each(function () {
      $(this).click(function () {
        if ($(this).hasClass('opened') == false) {
          $('.sidebar .menu-btn').find('.opened').removeClass('opened').find('.sub-menu').slideUp();
          $(this).addClass('opened');
          $(this).find(".sub-menu").slideDown();
        } else {
          $(this).removeClass('opened');
          $(this).find(".sub-menu").slideUp();
        }
      });
    });
     const token = localStorage.getItem('token');
    if (token) {
      this.authService.decodedToken = this.jwtHelper.decodeToken(token);
      this.userRoles = this.authService.decodedToken.role as Array<string>;
      console.log("loading decode role details start")
      console.log(this.authService.decodedToken.role);
      console.log("loading decode role details end")

    }
  }
  loggedIn() {
    return this.authService.loggedIn();
  }
/*   handler(event) {
    const target = $(event.target);
    console.log(target);
    if (target.is('.menu-btn')) {  // fa.fa-chevron-down.drop-down .menu-btn
      // target.children().toggle();
      console.log('clicked');
      // $('.sub-menu').toggle();
      target.find('.sub-menu').toggle();
    }
  } */
}
