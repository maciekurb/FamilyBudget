import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { MatSnackBar } from "@angular/material/snack-bar";

@Component({
  selector: 'app-user-login',
  templateUrl: './user-login.component.html',
  styleUrls: ['./user-login.component.css']
})
export class UserLoginComponent implements OnInit {
  hide = true;
  loginForm = this.fb.group({
    username: ['', [Validators.required]],
    password: ['', [Validators.required]]
  });

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private snackBar: MatSnackBar,
    private authService: AuthService
  ) { }

  ngOnInit() {
  }

  get username() { return this.loginForm.get('username'); }
  get password() { return this.loginForm.get('password'); }

  onSubmit() {
    const username = this.username?.value;
    const password = this.password?.value;

    if(username && password) {
      this.authService.login(username, password).then(
        () => {
          // On success
          this.snackBar.open('Login successful', undefined, { duration: 5000 });
          this.router.navigate(['/budgets']);
        },
      ).catch(
        error => {
          // On error
          this.snackBar.open(error.message, undefined, { duration: 5000 });
        }
      );
    }
  }

  goToSignUp() {
    this.router.navigate(['/user-create']);
  }
}
