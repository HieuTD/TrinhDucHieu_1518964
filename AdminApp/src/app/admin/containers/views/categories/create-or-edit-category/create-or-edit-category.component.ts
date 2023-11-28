import { Component, OnInit } from '@angular/core';
import { CategoryService } from '../category.service';
import { ToastServiceService } from '../../../shared/toast-service.service';
import { HttpClient } from '@angular/common/http';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { environment } from '../../../../../../environments/environment';

@Component({
  selector: 'app-create-or-edit-category',
  templateUrl: './create-or-edit-category.component.html',
  styleUrls: ['./create-or-edit-category.component.scss']
})
export class CreateOrEditCategoryComponent implements OnInit {
  constructor(public service: CategoryService,
    public toastService: ToastServiceService,
    public http: HttpClient,
  ) {
  }
  get name() { return this.newFormGroup.get('Name'); }
  ngOnInit(): void {
    this.newFormGroup = new FormGroup({
      Name: new FormControl("",
        [
          Validators.required,
        ]),
    });
  }
  public newFormGroup: FormGroup;
  onSubmit = (data) => {
    if (this.service.category.id == 0) {
      const formData = new FormData();
      formData.append('CategoryName', data.Name);
      this.http.post(environment.URL_API + 'categories', formData)
        .subscribe(res => {
          this.toastService.showToastThemThanhCong();
          this.service.getAllCategories();
          this.service.category.id = 0;
        }, err => {
          this.toastService.showToastThemThatBai()
        });
      this.newFormGroup.reset();
    }
    else {
      const formData = new FormData();
      formData.append('CategoryName', data.Name);
      this.http.put(environment.URL_API + 'categories/' + `${this.service.category.id}`, formData)
        .subscribe(res => {
          this.toastService.showToastSuaThanhCong();
          this.service.getAllCategories();
          this.service.category.id = 0;
        },
          error => {
            this.toastService.showToastXoaThatBai()
          });
    }
  }
}
