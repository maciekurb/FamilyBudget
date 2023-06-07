import { Injectable } from '@angular/core';
import { DataService } from '../data.service';
import { TokenService} from "./token.service";
import { Router } from '@angular/router';
import { MatSnackBar } from "@angular/material/snack-bar";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private isAuthenticated = false;

  constructor(private dataService: DataService,
              private tokenService: TokenService,
              private router: Router,
              private snackBar: MatSnackBar) { }

  login(username: string, password: string): Promise<boolean> {
    return new Promise<boolean>((resolve, reject) => {
      this.dataService.login(username, password)
        .subscribe(
          (response: any) => {
            this.isAuthenticated = true;
            this.tokenService.token = response.token;
            resolve(true);
          },
          (error: any) => {
            this.isAuthenticated = false;
            reject(error);
          }
        );
    });
  }

  logout(): void {
    this.isAuthenticated = false;
    this.tokenService.token = '';
    this.snackBar.open('Logout successful', undefined, { duration: 3000 });
    this.router.navigate(['/user-login']);
  }

  isAuthenticatedUser(): boolean {
    return this.tokenService.token !== '' && this.tokenService.token !== null;
  }
}
