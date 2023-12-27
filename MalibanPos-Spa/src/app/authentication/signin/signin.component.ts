import { AlertifyService } from './../../_services/alertify.service';
import { AuthService } from './../../_services/auth.service';
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import {
  FormGroup,
  FormControl,
  Validators,
  FormBuilder,
} from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.css'],
})
export class SigninComponent implements OnInit {
  model: any = {};
  loginForm: FormGroup;
  genders = ['Male', 'Female'];
  @Output() buttonClicked: EventEmitter<string> = new EventEmitter<string>();
  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private authService: AuthService,
    private toastrService: ToastrService,
    private alertifyService: AlertifyService
  ) {}

  ngOnInit() {
    this.createLoginForm();
    this.route.queryParams.subscribe((params) => {
      const newLocal = 'username';
      console.log(params[newLocal]);
    });
  }
  createLoginForm() {
    this.loginForm = this.fb.group({
      username: [null, [Validators.required, Validators.email]],
      password: [
        null,
        [
          Validators.required,
          Validators.minLength(6),
          Validators.maxLength(20),
        ],
      ],
      gender: ['Male'],
    });
  }
  get f() {
    return this.loginForm.controls;
  }
  login() {
    //https://developer.mozilla.org/en-US/docs/Web/API/FormData
    const formData: any = new FormData();
    formData.append('username', this.loginForm.get('username').value);
    formData.append('password', this.loginForm.get('password').value);
    // stop here if form is invalid
    if (this.loginForm.invalid) {
      return;
    } else {
      this.authService.login(formData).subscribe(
        (next) => {},
        (error) => {
         // this.alertifyService.error(JSON.stringify(error).replace('"', '').replace('"', ''));
          this.toastrService.error(JSON.stringify(error).replace('"', '').replace('"', ''))
          console.log(error.error);
        },
        () => {
          this.router.navigate(['/dashboard']);
        }
      );
    }
    console.log(this.loginForm.value);
  }
  clickButton() {
    this.buttonClicked.emit(
      'this value coming from child as a result of a button click event'
    );
    console.log('hi from child');
  }
}
