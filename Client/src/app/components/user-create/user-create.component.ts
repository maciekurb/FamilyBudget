import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { DataService } from '../../data.service';

@Component({
  selector: 'app-user-create',
  templateUrl: './user-create.component.html',
  styleUrls: ['./user-create.component.css']
})
export class UserCreateComponent implements OnInit {
  hide = true;
  loginForm = this.fb.group({
    username: ['', [Validators.required]],
    password: ['', [Validators.required]]
  });

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private snackBar: MatSnackBar,
    private dataService: DataService
  ) { }

  ngOnInit() { }

  get username() { return this.loginForm.get('username'); }
  get password() { return this.loginForm.get('password'); }

  onSubmit() {
    const username = this.username?.value;
    const password = this.password?.value;

    if(username && password){
      this.dataService.createAccount(username, password).subscribe(
        response => {
          this.snackBar.open('Account created successfully. Please log in.', undefined, {
            duration: 5000, // Dismiss after 3 seconds
          });

          this.router.navigate(['/user-login']);
        },
        error => {
          this.snackBar.open(error.error, undefined, {
            duration: 5000, // Dismiss after 3 seconds
          });
        }
      );
    }
  }
}

