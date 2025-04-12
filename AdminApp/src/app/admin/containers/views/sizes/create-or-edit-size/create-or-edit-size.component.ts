import { Component, OnInit } from '@angular/core';
import { SizesComponent } from '../sizes.component';
import { SizeService } from '../size.service';
import { HttpClient } from '@angular/common/http';
import { ToastServiceService } from '../../../shared/toast-service.service';
import { CategoryService } from '../../categories/category.service';
import { environment } from '../../../../../../environments/environment';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-create-or-edit-size',
  templateUrl: './create-or-edit-size.component.html',
  styleUrls: ['./create-or-edit-size.component.scss']
})
export class CreateOrEditSizeComponent implements OnInit {
  categories: any[] = [];
  public categorysComponent: SizesComponent
  constructor(public service: SizeService,
    public http: HttpClient,
    public serviceToast: ToastServiceService,
    public serviceCategory: CategoryService,
  ) {
    this.serviceCategory.get().subscribe(
      data => {
        Object.assign(this.categories, data)
      }
    )
  }
  get TenSize() { return this.newFormGroup.get('TenSize'); }
  get Id_Loai() { return this.newFormGroup.get('Id_Loai'); }
  ngOnInit(): void {
    console.log(this.service.size)
    this.newFormGroup = new FormGroup({
      Id_Loai: new FormControl(null,
        [
          Validators.required,
        ]),
      TenSize: new FormControl(null,
        [
          Validators.required,
          Validators.minLength(1),
        ]),
    });
  }
  selectedFile: File = null;
  public newFormGroup: FormGroup;
  onSelectFile(fileInput: any) {
    this.selectedFile = <File>fileInput.target.files[0];
  }
  onSubmit = (data) => {
    if (this.service.size.id == 0) {
      const formData = new FormData();
      formData.append('CategoryId', data.Id_Loai);
      formData.append('SizeName', data.TenSize);
      console.log(data)
      this.http.post(environment.URL_API + 'sizes', formData)
        .subscribe(res => {
          this.serviceToast.showToastThemThanhCong()
          this.service.getAllSizes();
          this.service.size.id = 0;
        }, err => {
          this.serviceToast.showToastThemThatBai()
        });
      this.newFormGroup.reset();
    }
    else {
      const formData = new FormData();
      formData.append('CategoryId', data.Id_Loai);
      formData.append('SizeName', data.TenSize);
      this.http.put(environment.URL_API + 'sizes/' + `${this.service.size.id}`, formData)
        .subscribe(res => {
          this.serviceToast.showToastSuaThanhCong()
          this.service.getAllSizes();
          this.service.size.id = 0;
        }, err => {
          this.serviceToast.showToastSuaThatBai()
        });
    }
  }
}