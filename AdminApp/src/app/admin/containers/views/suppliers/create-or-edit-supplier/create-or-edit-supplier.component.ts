import { Component, OnInit } from '@angular/core';
import { SupplierService } from '../supplier.service';
import { HttpClient } from '@angular/common/http';
import { ToastServiceService } from '../../../shared/toast-service.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { environment } from '../../../../../../environments/environment';

@Component({
  selector: 'app-create-or-edit-supplier',
  templateUrl: './create-or-edit-supplier.component.html',
  styleUrls: ['./create-or-edit-supplier.component.scss']
})
export class CreateOrEditSupplierComponent implements OnInit {
  categories: any[] = [];
  constructor(public service: SupplierService,
    public http: HttpClient,
    public serviceToast: ToastServiceService,
  ) {
  }
  public newFormGroup: FormGroup;
  get Ten() { return this.newFormGroup.get('Ten'); }
  get SDT() { return this.newFormGroup.get('SDT'); }
  get ThongTin() { return this.newFormGroup.get('ThongTin'); }
  get DiaChi() { return this.newFormGroup.get('DiaChi'); }
  ngOnInit(): void {
    this.newFormGroup = new FormGroup({
      Ten: new FormControl(null,
        [
          Validators.required,
          Validators.minLength(1),
        ]),
      SDT: new FormControl(null,
        [
          Validators.required,
          Validators.minLength(7),
          Validators.maxLength(20),
        ]),
      ThongTin: new FormControl(null,
        [
          Validators.required,
        ]),
      DiaChi: new FormControl(null,
        [
          Validators.required,
        ]),
    });
  }
  onSubmit = (data) => {
    if (this.service.nhacungcap.id == 0) {
      const formData = new FormData();
      formData.append('SupplierName', data.Ten);
      formData.append('PhoneNumber', data.SDT);
      formData.append('Description', data.ThongTin);
      formData.append('Address', data.DiaChi);
      console.log(data)
      this.http.post(environment.URL_API + 'suppliers', formData)
        .subscribe(res => {
          this.serviceToast.showToastThemThanhCong()
          this.service.getAllNhaCungCaps();
          this.service.nhacungcap.id = 0;
        }, err => {
          this.serviceToast.showToastThemThatBai()
        });
      this.newFormGroup.reset();
    }
    else {
      const formData = new FormData();
      formData.append('SupplierName', data.Ten);
      formData.append('PhoneNumber', data.SDT);
      formData.append('Description', data.ThongTin);
      formData.append('Address', data.DiaChi);
      this.http.put(environment.URL_API + 'suppliers/' + `${this.service.nhacungcap.id}`, formData)
        .subscribe(res => {
          this.serviceToast.showToastSuaThanhCong()
          this.service.getAllNhaCungCaps();
          this.service.nhacungcap.id = 0;
        }, err => {
          this.serviceToast.showToastSuaThatBai()
        });
    }
  }
}
