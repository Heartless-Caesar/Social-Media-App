import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { ReplaySubject } from 'rxjs';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  baseUrl: String = 'https://localhost:7225/api/';
  currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) {}

  register(model: any) {
    return this.http.post(this.baseUrl + 'auth/register', model).pipe(
      map((res: User) => {
        if (res) {
          localStorage.setItem('user', JSON.stringify(res));
          this.currentUserSource.next(res);
        }
      })
    );
  }

  login(model: any) {
    return this.http.post(this.baseUrl + 'auth/login', model).pipe(
      map((res: User) => {
        const user = res;
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
    );
  }

  setCurrentUser(user: User) {
    this.currentUserSource.next(user);
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }
}
