import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ConfirmPasswordValidator } from 'src/app/helpers/confirm-password.validator';
import { BaseService } from 'src/app/service/base.service';
import { environment } from 'src/environments/environment';
import Swal from 'sweetalert2';
@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent extends BaseService implements OnInit {
  public register: any;
  userFormGroup: FormGroup
  public hide: boolean = true;
  username: any;
  private _unsubscribeAll: Subject<any>;
  constructor(public http: HttpClient, public router: Router, private _formBuilder: FormBuilder) {
    super();
    this.register = {};
    this._unsubscribeAll = new Subject();
  }
  check() {
    if (this.userFormGroup.value.Password != this.userFormGroup.value.confirmPassword) {
      Swal.fire("Mật khẩu không trùng nhau", '', 'warning').then(function () {
      }
      )
    }
    else {
      this.registerAccount(this.userFormGroup.value)
    }
  }
  registerAccount(data) {
    let form = new FormData();
    form.append('FirstName', data.FirstName);
    form.append('LastName', data.LastName);
    form.append('Email', data.Email);
    form.append('PhoneNumber', data.SDT);
    form.append('Address', data.DiaChi);
    form.append('Password', data.Password);
    form.append('Role', 'User');
    this.http.post(environment.URL_API + 'users/register', form).subscribe(resp => {
      Swal.fire("Đăng ký thành công", ' ', 'success').then(function () {
        // this.router.navigate(['/login']);
        window.location.href = '/login'
      }
      )
    }, error => {
      Swal.fire("Đăng ký không thành công, email đã tồn tại", '', 'error');
    }
    )
  }
  ngOnInit(): void {
    this.userFormGroup = this._formBuilder.group({
      FirstName: ['', [Validators.required]],
      LastName: ['', Validators.required],
      Email: ['', Validators.required],
      SDT: ['', Validators.required],
      DiaChi: ['', Validators.required],
      Password: ['', Validators.required],
      confirmPassword: ['', Validators.required],
    },
      {
        validator: ConfirmPasswordValidator("Password", "confirmPassword")
      });
    this.userFormGroup.get('Password').valueChanges
      .pipe(takeUntil(this._unsubscribeAll))
      .subscribe(() => {
        this.userFormGroup.get('confirmPassword').updateValueAndValidity();
      });
  }
  ngOnDestroy(): void {
    // Unsubscribe from all subscriptions
    this._unsubscribeAll.next();
    this._unsubscribeAll.complete();
  }
}
