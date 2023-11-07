import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { environment } from '../../../../../environments/environment';
import { ToastServiceService } from '../../shared/toast-service.service';
import { ProfileService } from './profile.service';
@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  constructor(public toast: ToastServiceService, public service: ProfileService, public http: HttpClient) { }
  id: string
  userApp: any
  public newFormGroup: FormGroup;
  ngOnInit(): void {
    this.userApp = JSON.parse(localStorage.getItem("appuser"));
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
      UserName: new FormControl("",
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
    formData.append('PhoneNumber', data.SDT);
    formData.append('Address', data.DiaChi);
    formData.append('FirstName', data.FirstName);
    formData.append('LastName', data.LastName);
    formData.append('UserName', data.UserName);
    formData.append('Role', data.Quyen);
    formData.append('Password', data.Password);
    formData.append('PasswordNew', data.PasswordNew);
    this.http.put(environment.URL_API + 'users/updateUser/' + `${this.id}`, formData).subscribe(
      response => {
        this.toast.showToastSuaThanhCong()
      },
      error => {
      }
    )
  }
}
