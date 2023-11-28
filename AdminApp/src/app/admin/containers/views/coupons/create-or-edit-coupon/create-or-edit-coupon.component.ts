import { Component, OnInit } from '@angular/core';
import { CouponService } from '../coupon.service';
import { ToastServiceService } from '../../../shared/toast-service.service';
import { HttpClient } from '@angular/common/http';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { environment } from '../../../../../../environments/environment';

@Component({
  selector: 'app-create-or-edit-coupon',
  templateUrl: './create-or-edit-coupon.component.html',
  styleUrls: ['./create-or-edit-coupon.component.scss']
})
export class CreateOrEditCouponComponent implements OnInit {
  constructor(public service: CouponService,
    public toastService: ToastServiceService,
    private http: HttpClient,
  ) {
  }
  get SoTienGiam() { return this.newFormGroup.get('SoTienGiam'); }
  ngOnInit(): void {
    this.newFormGroup = new FormGroup({
      SoTienGiam: new FormControl(null,
        [
          Validators.required,
        ]),
    });
  }
  public newFormGroup: FormGroup;
  onSubmit = (data) => {
    if (this.service.magiamgia.id == 0) {
      const formData = new FormData()
      formData.append('Discount', data.SoTienGiam);
      this.http.post(environment.URL_API + 'coupons', formData)
        .subscribe(res => {
          this.toastService.showToastThemThanhCong();
          this.service.getAllMaGiamGias()
          this.service.magiamgia.id = 0;
        }, err => {
          this.toastService.showToastThemThatBai()
        });
      this.newFormGroup.reset();
    }
    else {
      const formData = new FormData();
      formData.append('Discount', data.SoTienGiam);
      this.http.put(environment.URL_API + 'coupons/' + `${this.service.magiamgia.id}`, formData)
        .subscribe(res => {
          this.toastService.showToastSuaThanhCong();
          this.service.getAllMaGiamGias()
          this.service.magiamgia.id = 0;
        },
          error => {
            this.toastService.showToastSuaThatBai()
          });
    }
  }
}
