import { Component, OnInit } from '@angular/core';
import { Product, ProductService } from '../product.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { SupplierService } from '../../suppliers/supplier.service';
import { BrandService } from '../../brands/brand.service';
import { CategoryService } from '../../categories/category.service';
import { ToastServiceService } from '../../../shared/toast-service.service';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic';

@Component({
  selector: 'app-create-or-edit-product',
  templateUrl: './create-or-edit-product.component.html',
  styleUrls: ['./create-or-edit-product.component.scss']
})
export class CreateOrEditProductComponent implements OnInit {
  public product: Product
  //Begin Review multile file before upload
  public newForm: FormGroup;
  urls = new Array<string>();
  nhacungcaps: any[] = [];
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
  //End Review multile file before upload
  public Editor = ClassicEditor;
  selectedFile: FileList;
  categories: any[] = [];
  brands: any[] = [];
  constructor(public service: ProductService,
    public http: HttpClient,
    public router: Router,
    public serviceToast: ToastServiceService,
    public serviceCategory: CategoryService,
    public serviceBrand: BrandService,
    public serviceNhaCungCap: SupplierService) {
  }
  onSelectedList() {
    this.router.navigate(['admin/products']);
  }
  get Ten() { return this.newForm.get('Ten'); }
  get GiaBan() { return this.newForm.get('GiaBan'); }
  get GiaNhap() { return this.newForm.get('GiaNhap'); }
  get KhuyenMai() { return this.newForm.get('KhuyenMai'); }
  get MoTa() { return this.newForm.get('MoTa'); }
  get Tag() { return this.newForm.get('Tag'); }
  get HuongDan() { return this.newForm.get('HuongDan'); }
  get ThanhPhan() { return this.newForm.get('ThanhPhan'); }
  get Id_Loai() { return this.newForm.get('Id_Loai'); }
  get Id_NhanHieu() { return this.newForm.get('Id_NhanHieu'); }
  get Id_NhaCungCap() { return this.newForm.get('Id_NhaCungCap'); }
  get TrangThaiSanPham() { return this.newForm.get('TrangThaiSanPham'); }
  get TrangThaiHoatDong() { return this.newForm.get('TrangThaiHoatDong'); }
  get GioiTinh() { return this.newForm.get('GioiTinh') }
  ngOnInit(): void {
    console.log(this.service.product)
    this.serviceCategory.get().subscribe(
      data => {
        Object.assign(this.categories, data)
      }
    )
    this.serviceBrand.get().subscribe(
      data => {
        Object.assign(this.brands, data)
      }
    )
    this.serviceNhaCungCap.gethttp().subscribe(
      data => {
        Object.assign(this.nhacungcaps, data)
        console.log("nha cung cap", this.nhacungcaps);
      }
    )
    this.newForm = new FormGroup({
      Ten: new FormControl(null, [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(100),
      ]),
      GiaBan: new FormControl(null, [
        Validators.required,
        Validators.min(3),
        Validators.max(100000000000),
      ]),
      GiaNhap: new FormControl(null, [
        Validators.required,
        Validators.min(3),
        Validators.max(100000000000),
      ]),
      KhuyenMai: new FormControl(this.service.product.discount, [
        Validators.required,
        Validators.min(0),
        Validators.max(50000000000),
      ]),
      MoTa: new FormControl(null, [
        Validators.required,
        Validators.minLength(10),
        Validators.maxLength(10000000000000),
      ]),
      // HuongDan: new FormControl(null, [
      //   Validators.required,
      //   Validators.minLength(3),
      //   Validators.maxLength(1000000000000),
      // ]),
      ThanhPhan: new FormControl(null, [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(5000000000),
      ]),
      Tag: new FormControl(null, [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(15),
      ]),
      Id_Loai: new FormControl(null, [
        Validators.required,
      ]),
      Id_NhaCungCap: new FormControl(null, [
        Validators.required,
      ]),
      Id_NhanHieu: new FormControl(null, [
        Validators.required,
      ]),
      TrangThaiSanPham: new FormControl(null, [
        Validators.required,
      ]),
      TrangThaiHoatDong: new FormControl(null, [
        Validators.required,
      ]),
      GioiTinh: new FormControl(null, [
        Validators.required,
      ]),
    });
  }
  clearForm() {
    this.newForm.reset();
  }
  onSubmit = (data) => {
    if (this.service.product.id == 0) {
      let form = new FormData();
      for (let i = 0; i < this.urls.length; i++) {
        form.append('files', this.selectedFile.item(i))
      }
      form.append('Name', data.Ten);
      form.append('Discount', data.KhuyenMai);
      form.append('Description', data.MoTa);
      form.append('Price', data.GiaBan);
      form.append('OriginalPrice', data.GiaNhap);
      // form.append('HuongDan', data.HuongDan);
      form.append('Material', data.ThanhPhan);
      form.append('Tag', data.Tag);
      form.append('Gender', data.GioiTinh)
      form.append('CategoryId', data.Id_Loai);
      form.append('BrandId', data.Id_NhanHieu);
      form.append('SupplierId', data.Id_NhaCungCap);
      form.append('Status', data.TrangThaiSanPham);
      form.append('IsFeatured', data.TrangThaiHoatDong);
      var json_arr = JSON.stringify(data);
      console.log(json_arr)
      this.service.post(form)
        .subscribe(res => {
          this.serviceToast.showToastThemThanhCong()
          this.resetForm()
          this.service.getAllProducts();
          this.service.product.id = 0;
          this.onSelectedList();
          this.clearForm();
        }, err => {
          this.serviceToast.showToastThemThatBai()
        }
        );
    }
    else {
      const form = new FormData();
      form.append('Name', data.Ten);
      form.append('Discount', data.KhuyenMai);
      form.append('Description', data.MoTa);
      form.append('Price', data.GiaBan);
      form.append('OriginalPrice', data.GiaNhap);
      // form.append('HuongDan', data.HuongDan);
      form.append('Material', data.ThanhPhan);
      form.append('Tag', data.Tag);
      form.append('Gender', data.GioiTinh)
      form.append('CategoryId', data.Id_Loai);
      form.append('BrandId', data.Id_NhanHieu);
      form.append('SupplierId', data.Id_NhaCungCap);
      form.append('Status', data.TrangThaiSanPham);
      form.append('IsFeatured', data.TrangThaiHoatDong);
      // form.append('TrangThaiSanPhamThietKe', data.TrangThaiSanPhamThietKe);
      for (let i = 0; i < this.urls.length; i++) {
        form.append('files', this.selectedFile.item(i))
      }
      form.append('TrangThaiHoatDong', data.TrangThaiHoatDong);
      this.service.put(this.service.product.id, form)
        .subscribe(res => {
          this.serviceToast.showToastSuaThanhCong()
          this.resetForm()
          this.service.getAllProducts();
          this.service.product.id = 0;
          this.onSelectedList();
          this.clearForm();
        }, err => {
          this.serviceToast.showToastSuaThatBai()
        });
    }
  }
  resetForm() {
    this.newForm.reset();
    this.service.product = new Product();
  }
}
