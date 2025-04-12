import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import Swal from 'sweetalert2';
@Component({
  selector: 'app-info-user',
  templateUrl: './info-user.component.html',
  styleUrls: ['./info-user.component.scss']
})
export class InfoUserComponent implements OnInit {
  user: any;
  PasswordCurrent = "";
  PasswordNew = "";
  constructor(public http: HttpClient) {

  }
  ngOnInit(): void {
    this.http.get(environment.URL_API + "users/AuthHistory").subscribe(
      res => {
        this.user = res;
      },
      error => {
      }
    );
  }

  Submit() {
    const clicks = localStorage.getItem('idUser');
    const formData = new FormData();
    formData.append('FirstName', this.user.firstName);
    formData.append('LastName', this.user.lastName);
    formData.append('PhoneNumber', this.user.phoneNumber);
    formData.append('Address', this.user.address);
    formData.append('Email', this.user.email);
    formData.append('Password', this.PasswordCurrent);
    formData.append('PasswordNew', this.PasswordNew);
    this.http.put(environment.URL_API + 'users/updateUser/' + clicks, formData).subscribe(
      res => {
        Swal.fire("Cập nhật thông tin thành công.", '', 'success')
      },
      error => {
        Swal.fire({
          title: 'Cập nhật thông tin thất bại',
          text: "",
          icon: 'error',
          confirmButtonColor: '#3085d6',
          confirmButtonText: 'Đóng',
        })
      });
  }
}
