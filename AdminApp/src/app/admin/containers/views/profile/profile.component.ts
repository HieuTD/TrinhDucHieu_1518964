import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { environment } from '../../../../../environments/environment';
import { ToastServiceService } from '../../shared/toast-service.service';
import { ProfileService } from './profile.service';
import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  constructor(public toast: ToastrService, public service: ProfileService, public http: HttpClient) { }
  id: string
  userApp: any
  public newFormGroup: FormGroup;
  user:any;
  
  ngOnInit(): void {
    this.userApp = JSON.parse(localStorage.getItem("appuser"));
    this.http.get(environment.URL_API+"users/AuthHistory").subscribe(
      res=>{
        this.user = res;
      },
      error=>{
      }
      );
    this.newFormGroup = new FormGroup({
      SDT: new FormControl(null,
        [
          Validators.required,
          Validators.minLength(2),
        ]),
      DiaChi: new FormControl("",
        [
          Validators.required,
          Validators.minLength(5),
        ]),
      FirstName: new FormControl("",
        [
          Validators.required,
        ]
      ),
      LastName: new FormControl("",
        [
          Validators.required,
          Validators.minLength(5),
        ]
      ),
      Email: new FormControl("",
        [
          Validators.required,
          Validators.minLength(0),
        ]
      ),
      Quyen: new FormControl("",
        [
          Validators.required,
          Validators.minLength(0),
        ]
      ),
      Password: new FormControl(null,
        [
          Validators.required,
          Validators.minLength(0),
        ]),
      PasswordNew: new FormControl(null,
        [
          Validators.required,
          Validators.minLength(0),
        ])
    });
    this.id = localStorage.getItem("idUser")
  }
  onSubmit = (data) => {
    const formData = new FormData();
    formData.append('PhoneNumber', this.user.phoneNumber);
    formData.append('Address', this.user.address);
    formData.append('FirstName', this.user.firstName);
    formData.append('LastName', this.user.lastName);
    formData.append('Email', this.user.email);
    // formData.append('Role', data.Quyen);
    formData.append('Password', data.Password);
    formData.append('PasswordNew', data.PasswordNew);
    this.http.put(environment.URL_API + 'users/updateUser/' + this.id, formData).subscribe(
      response => {
        this.toast.success("Cập nhật thông tin thành công")
      },
      error => {
        this.toast.error("Cập nhật thông tin thất bại")
      }
    )
  }
}
