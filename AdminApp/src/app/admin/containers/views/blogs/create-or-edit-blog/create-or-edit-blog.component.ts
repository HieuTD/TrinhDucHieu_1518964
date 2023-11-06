import { Component, OnInit } from '@angular/core';
import { BlogService } from '../blog.service';
import { HttpClient } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { ToastServiceService } from '../../../shared/toast-service.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic';

@Component({
  selector: 'app-create-or-edit-blog',
  templateUrl: './create-or-edit-blog.component.html',
  styleUrls: ['./create-or-edit-blog.component.scss']
})
export class CreateOrEditBlogComponent implements OnInit {
  constructor(public service: BlogService,
    public http: HttpClient,
    public toastr: ToastrService,
    public serviceToast: ToastServiceService,
  ) {
  }
  idUser: any

  ngOnInit(): void {
    this.newFormGroup = new FormGroup({
      TieuDe: new FormControl(null,
        [
          Validators.required,
          Validators.minLength(2),
        ]),
      NoiDung: new FormControl(null,
        [
          Validators.required,
          Validators.minLength(5),
        ]),
      Hinh: new FormControl(null,
        [
          Validators.required,
        ]
      )
    });
  }
  public Editor = ClassicEditor;
  public newFormGroup: FormGroup;
  urls = new Array<string>();
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
  onSelectFile(fileInput: any) {
    this.selectedFile = <FileList>fileInput.target.files;
  }
  selectedFile: FileList;
  clearForm() {
    this.newFormGroup.reset();
  }
  onSubmit = (data) => {
    this.idUser = localStorage.getItem("idUser")
    if (this.service.blog.id == 0) {
      let form = new FormData();
      for (let i = 0; i < this.urls.length; i++) {
        form.append('files', this.selectedFile.item(i))
      }
      form.append('Title', data.TieuDe);
      form.append('Description', data.NoiDung);
      form.append('UserId', this.idUser);
      this.service.post(form)
        .subscribe(res => {
          this.service.getAllBlogs();
          this.serviceToast.showToastThemThanhCong()
          this.clearForm();
        }, err => {
          this.serviceToast.showToastThemThatBai()
        }
        );
    }
    else {
      const form = new FormData();
      form.append('Title', data.TieuDe);
      form.append('Description', data.NoiDung);
      form.append('UserId', this.idUser);
      for (let i = 0; i < this.urls.length; i++) {
        form.append('files', this.selectedFile.item(i))
      }
      this.service.put(this.service.blog.id, form)
        .subscribe(res => {
          this.service.getAllBlogs();
          this.serviceToast.showToastSuaThanhCong()
          this.clearForm();
        }, err => {
          this.serviceToast.showToastSuaThatBai()
        });
    }
  }
}
