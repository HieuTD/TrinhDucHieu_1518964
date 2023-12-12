import { Component, OnInit } from '@angular/core';
import { ProductVariantService } from '../product-variant.service';
import { FileToUploadService } from '../../../shared/file-to-upload.service';
import { ToastServiceService } from '../../../shared/toast-service.service';
import { CategoryService } from '../../categories/category.service';
import { ProductService } from '../../products/product.service';
import { HttpClient } from '@angular/common/http';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { environment } from '../../../../../../environments/environment';
import { Size } from '../../sizes/size.service';

@Component({
  selector: 'app-create-or-edit-product-variant',
  templateUrl: './create-or-edit-product-variant.component.html',
  styleUrls: ['./create-or-edit-product-variant.component.scss']
})
export class CreateOrEditProductVariantComponent implements OnInit {
  public imgsrc: string = "./assets/Resources/Images/san-pham-bien-the/blog-02.jpg";
  public imgsrc1: string = "./assets/Resources/Images/item/pin.png";
  fileChange(event) {
    var reader = new FileReader()
    reader.readAsDataURL(event.target.files[0])
    reader.onload = (event: any) => {
      this.imgsrc = event.target.result
    }
  }
  urls = new Array<string>();
  selectedFile: File = null;
  detectFiles(event) {
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
    this.selectedFile = <File>fileInput.target.files[0];
  }
  gopHam(event) {
    this.detectFiles(event)
    this.onSelectFile(event)
    this.fileChange(event)
  }
  categorys: any[] = [];
  mausacs: any[] = [];
  sanphams: any[] = [];
  sizes: any[] = [];
  loaitenmau: any[] = [];
  loaitensize: any[] = [];
  constructor(public service: ProductVariantService,
    public upfile: FileToUploadService,
    public serviceToast: ToastServiceService,
    public serviceCategory: CategoryService,
    public serviceSanPham: ProductService,
    public http: HttpClient) {
  }
  get Id_Mau() { return this.newFormGroup.get('Id_Mau'); }
  get Id_SanPham() { return this.newFormGroup.get('Id_SanPham'); }
  get Id_Size() { return this.newFormGroup.get('Id_Size'); }
  get SoLuongTon() { return this.newFormGroup.get('SoLuongTon') }
  ngOnInit(): void {
    this.newFormGroup = new FormGroup({
      ImagePath: new FormControl(null),
      Id_Mau: new FormControl(null, [
        Validators.required,
      ]),
      Id_SanPham: new FormControl(null, [
        Validators.required,
      ]),
      Id_Size: new FormControl(null, [
        Validators.required,
      ]),
      SoLuongTon: new FormControl(100, [
        Validators.required
      ])
    });
    // this.service.getAllTenMauLoai().subscribe(
    //   data => {
    //     Object.assign(this.loaitenmau, data)
    //   }
    // )
    // this.service.getAllTenSizeLoai().subscribe(
    //   data => {
    //     Object.assign(this.loaitensize, data)
    //     console.log(this.loaitensize);
    //   }
    // )
    this.service.getAllSanPhams().subscribe(
      data => {
        Object.assign(this.sanphams, data)
      }
    )

    var val = {
      prodId: this.service.sanphambienthe.prodId
    }
    this.http.post(environment.URL_API + "colors/listColorByProdIdAdmin/", val).subscribe(
      res => {
        Object.assign(this.loaitenmau, res)
      });

    this.http.post(environment.URL_API + "sizes/listSizeByProdId/", val).subscribe(
      res => {
        Object.assign(this.loaitensize, res)
      });
  }
  onChangeProd(ProdId) {
    var val = {
      prodId: ProdId
    }
    this.http.post(environment.URL_API + "colors/listColorByProdIdAdmin/", val).subscribe(
      res => {
        Object.assign(this.loaitenmau, res)
      });

    this.http.post(environment.URL_API + "sizes/listSizeByProdId/", val).subscribe(
      res => {
        Object.assign(this.loaitensize, res)
      });
  }
  public newFormGroup: FormGroup;
  onSubmit = (data) => {
    if (this.service.sanphambienthe.id == undefined) {
      const formData = new FormData();
      formData.append('ColorId', data.Id_Mau);
      formData.append('ProdId', data.Id_SanPham);
      formData.append('SizeId', data.Id_Size);
      formData.append('Stock', '0');
      console.log(data);
      this.http.post(environment.URL_API + 'productVariants', formData)
        .subscribe(res => {
          this.serviceToast.showToastThemThanhCong()
          this.service.getAllGiaSanPhamMauSacSanPhamSizes();
          this.service.sanphambienthe.id = 0;
        }, err => {
          this.serviceToast.showToastThemThatBai()
        });
      this.newFormGroup.reset();
    }
    else {
      const formData = new FormData();
      formData.append('ColorId', data.Id_Mau);
      formData.append('ProdId', data.Id_SanPham);
      formData.append('SizeId', data.Id_Size);
      this.http.put(environment.URL_API + 'productVariants/' + `${this.service.sanphambienthe.id}`, formData)
        .subscribe(res => {
          this.serviceToast.showToastSuaThanhCong()
          this.service.getAllGiaSanPhamMauSacSanPhamSizes();
          this.service.sanphambienthe.id = 0;
        }, err => {
          this.serviceToast.showToastSuaThatBai()
        });
    }
  }
}
