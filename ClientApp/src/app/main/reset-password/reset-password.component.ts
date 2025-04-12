import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfirmPasswordValidator } from 'src/app/helpers/confirm-password.validator';
import { ResetPassword, UserService } from 'src/app/service/account/user.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {
  resetPasswordForm!: FormGroup;
  emailToReset!: string;
  emailToken!: string;
  resetPasswordObj = new ResetPassword();
  public hide: boolean = true;

  constructor(private _formBuilder: FormBuilder, private activatedRoute: ActivatedRoute, public userService: UserService, public router: Router) { }

  ngOnInit() {
    this.resetPasswordForm = this._formBuilder.group({
      password: [null, [Validators.required]],
      confirmPassword: [null, Validators.required],
    },{
      validator: ConfirmPasswordValidator("password", "confirmPassword")
    });

    this.activatedRoute.queryParams.subscribe(val => {
      this.emailToReset = val['email'];
      let uriToken = val['code'];

      this.emailToken = uriToken.replace(/ /g, '+');
      console.log(this.emailToReset);
      console.log(this.emailToken);
    })
  }


  reset() {
    if(this.resetPasswordForm.valid){
      this.resetPasswordObj.email = this.emailToReset;
      this.resetPasswordObj.newPassword = this.resetPasswordForm.value.password;
      this.resetPasswordObj.confirmPassword = this.resetPasswordForm.value.confirmPassword;
      this.resetPasswordObj.emailToken = this.emailToken;

      this.userService.resetPassword(this.resetPasswordObj)
      .subscribe({
        next:(res) => {
          Swal.fire("Đặt lại mật khẩu thành công", '', 'success')
          this.router.navigate(['/login']);
        },
        error:(err) => {
          Swal.fire("Thời hạn đặt lại mật khẩu đã hết, Vui lòng chọn lại yêu cầu quên mật khẩu để nhận đường link mới", '', 'error')
          this.router.navigate(['/login']);
        }
      })
    }
  }
}



// export function ConfirmPasswordValidator(controlName: string, matchControlName: string) {
//   return (formGroup: FormGroup) => {
//     const passwordControl = formGroup.controls[controlName];
//     const confirmPasswordControl = formGroup.controls[matchControlName];
//     if (confirmPasswordControl.errors && confirmPasswordControl.errors['confirmPasswordValidator']){
//       return;
//     }

//     if(passwordControl.value !== confirmPasswordControl.value){
//       confirmPasswordControl.setErrors({ confirmPasswordValidator : true })
//     }else {
//       confirmPasswordControl.setErrors(null)
//     }
//   }
// }