import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { BaseService } from './base.service';
import { Route } from '@angular/compiler/src/core';
import { Router } from '@angular/router';
import { map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import Swal from 'sweetalert2';
//import * as _ from 'lodash';
// Add the RxJS Observable operators we need in this app.
@Injectable()
export class UserService extends BaseService {
  baseUrl: string = '';
  // Observable navItem source
  private _authNavStatusSource = new BehaviorSubject<boolean>(false);
  // Observable navItem stream
  authNavStatus$ = this._authNavStatusSource.asObservable();
  private loggedIn = false;
  constructor(public http: HttpClient, public router: Router) {
    super();
    this.loggedIn = !!localStorage.getItem('auth_token');
    // ?? not sure if this the best way to broadcast the status but seems to resolve issue on page refresh where auth status is lost in
    // header component resulting in authed user nav links disappearing despite the fact user is still logged in
    this._authNavStatusSource.next(this.loggedIn);
  }
  login(userName, password): boolean {
    var check = false;
    this.http.post(
      environment.URL_API + 'users/login',
      JSON.stringify({ userName, password }),
      {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' }
        )
      }).subscribe(
        (res: any) => {
          localStorage.setItem('auth_token', res.auth_token);
          localStorage.setItem('idUser', res.id);
          const clicks = localStorage.getItem('idUser');
          this.http.get(environment.URL_API + "Carts/getcartbyuserid/" + clicks, {}).subscribe(
            res1 => {
              var list_item = res1;
              localStorage.setItem('products', JSON.stringify(list_item));
            }
          );
          this.http.get(environment.URL_API + "productlikes/listprodlikebyuserid/" + clicks).subscribe(
            res2 => {
              var list_sanphamyeuthich = res2;
              localStorage.setItem('loves', JSON.stringify(list_sanphamyeuthich));
            }
          );
          Swal.fire("Đăng nhập thành công .", '', 'success')
          window.location.href = "/";
          this.loggedIn = true;
          check = true;
          this._authNavStatusSource.next(true);
        },
        (error) => {                              //Error callback
          Swal.fire({
            title: 'Đăng nhập không thành công',
            text: "",
            icon: 'error',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'Đóng',
          }).then((result) => {
          }) //You can also throw the error to a global error handler
        }
      )
    return check;
  }
  logout() {
    localStorage.removeItem('auth_token');
    this.router.navigate(['/login']);
    this.loggedIn = false;
    this._authNavStatusSource.next(false);
  }
  isLoggedIn() {
    return this.loggedIn;
  }
  checkLogin(): boolean {
    if (localStorage.getItem("idUser") != null || localStorage.getItem("auth_token") != null) {
      return true;
    }
    else {
      this.router.navigate(['/login']);
      return false
    }
  }
  sendResetPasswordLink(email: string) {
    return this.http.post<any>(environment.URL_API + 'users/send-reset-email/' + email, {})
  }

  resetPassword(resetPasswordObj: ResetPassword) {
    return this.http.post<any>(environment.URL_API + 'users/reset-password', resetPasswordObj);
  }
}
export interface UserRegistration {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  location: string;
}

export class ResetPassword {
  public email!: string;
  public emailToken!: string;
  public newPassword!: string;
  public confirmPassword!: string;
}
