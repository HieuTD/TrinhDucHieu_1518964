import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { UserService } from '../user.service';
import { UserRegistration } from '../user.service'
import { environment } from '../../../../../../environments/environment';
import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'app-registration-form',
  templateUrl: './registration-form.component.html',
  styleUrls: ['./registration-form.component.scss']
})
export class RegistrationFormComponent implements OnInit {
  urls = new Array<string>();
  public newForm: FormGroup;
  gopHam(event) {
    this.detectFiles(event)
    this.onSelectFile(event)
  }
  detectFiles(event) {
    this.urls = [];
    let files = event.target.files;
    for (let file of files) {
      let reader = new FileReader();
      reader.onload = (e: any) => {
        this.urls.push(e.target.result);
      }
      reader.readAsDataURL(file);
    }
  }
  selectedFile: FileList;
  onSelectFile(fileInput: any) {
    this.selectedFile = <FileList>fileInput.target.files;
  }
  errors: string;
  isRequesting: boolean;
  submitted: boolean = false;
  constructor(public userService: UserService, private router: Router, public http: HttpClient, public _snackBar: MatSnackBar, public toastr: ToastrService) {
  }
  onLogin() {
    this.router.navigate(['/login'])
  }
  ngOnInit() {
    this.newForm = new FormGroup({
      firstName: new FormControl(null),
      lastName: new FormControl(null),
      email: new FormControl(null),
      password: new FormControl(null),
      location: new FormControl(null),
    })
  }
  onSubmit = (data) => {
    let form = new FormData();
    form.append('Email', data.email);
    form.append('Password', data.password);
    form.append('FirstName', data.firstName);
    form.append('LastName', data.lastName);
    form.append('Address', data.location);
    form.append('Role', 'Admin');
    this.http.post(environment.URL_API + 'users/register', form).subscribe
      (result => {
        this.router.navigate(['/login'], { queryParams: { brandNew: true, email: data.email } });
        this.toastr.success('Đăng ký thành công', 'Thông báo', { timeOut: 2000 });
      },
        errors => 
        {
          this.errors = errors;
          this.toastr.error('Đăng ký không thành công, email đã tồn tại', 'Thông báo', { timeOut: 2000 });
        }
      )
  }
}