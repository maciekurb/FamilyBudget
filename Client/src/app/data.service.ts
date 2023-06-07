import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import { Budget } from './shared/models/budget';
import { Observable } from "rxjs";
import {TokenService} from "./services/token.service";

@Injectable({
  providedIn: 'root'
})
export class DataService {
  apiUrl = 'http://localhost:5000/api';

  constructor(private http: HttpClient, private tokenService: TokenService,) { }

  createAccount(username: string, password: string) {
    const body = { username, password };
    return this.http.post(this.apiUrl + '/auth', body);
  }

  login(username: string, password: string) {
    const body = { username, password };
    return this.http.post(this.apiUrl + '/auth/login', body);
  }

  getBudgets(): Observable<Budget[]> {
    const headers = new HttpHeaders()
      .set('Authorization',  `Bearer ${this.tokenService.token}`);

    return this.http.get<Budget[]>(this.apiUrl + '/budgets', { headers: headers });
  }

  postBudget(budget : Budget) {
    const headers = new HttpHeaders()
      .set('Authorization',  `Bearer ${this.tokenService.token}`);

    return this.http.post(this.apiUrl + '/budgets', budget, { headers: headers });
  }
}
