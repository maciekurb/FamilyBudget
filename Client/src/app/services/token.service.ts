import {Injectable} from "@angular/core";

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  private _token: string | null = null;

  constructor() {
    this._token = localStorage.getItem('auth_token');
  }

  get token(): string | null {
    return this._token;
  }

  set token(value: string | null) {
    if (value) {
      localStorage.setItem('auth_token', value);
    } else {
      localStorage.removeItem('auth_token');
    }
    this._token = value;
  }
}
