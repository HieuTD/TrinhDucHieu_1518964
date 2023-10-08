import { HttpClient } from "@angular/common/http";
import { Injectable, ViewChild } from "@angular/core";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { MatTableDataSource } from "@angular/material/table";
import { Observable } from "rxjs";
import { environment } from "../../../../../environments/environment";
@Injectable({
    providedIn: 'root'
  })
export class ProductVariantService{
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  public dataSource = new MatTableDataSource<SanPhamBienThe>();
    sanphambienthe:SanPhamBienThe = new SanPhamBienThe()
    readonly url=environment.URL_API+"sanphambienthes"
    constructor(public http:HttpClient) { }
    delete(id:number):Observable<any>{
    //   return this.http.delete<any>(`${this.url}/${id}`)
      return this.http.delete<any>(`${"https://localhost:44391/api/"}/${id}`)
    }
    gethttp():Observable<any>{
      return this.http.get("https://localhost:44391/api/"+"productDetails")
    }
    getAllGiaSanPhamMauSacSanPhamSizes(){
      this.gethttp().subscribe(
        res=>{
          this.dataSource.data = res as SanPhamBienThe[];
          console.log(this.dataSource.data)
        }
      )
    }
    /* Dùng cho component modal thêm xóa sửa */
    getAllSanPhams(){
    //   return this.http.get(environment.URL_API+"sanphams")
      return this.http.get("https://localhost:44391/api/"+"products")
    }
    getAllTenSizeLoai(){
    //   return this.http.get(environment.URL_API+"sizes/tensizeloai")
      return this.http.get("https://localhost:44391/api/"+"sizes/listSizeCategory")
    }
    getAllTenMauLoai(){
    //   return this.http.get(environment.URL_API+"mausacs/tenmauloai")
      return this.http.get("https://localhost:44391/api/"+"colors/listColorCategory")
    }
  }
  export class LoaiMau{
    loaiTenMau: string
  }
  // export class SanPhamBienThe{
  //   id : number = 0
  //   mauId : number 
  //   sanPhamId : number
  //   sizeId : number
  //   soLuongTon:number =0
  // }

  // GiaSanPhamMauSacSanPhamSize
  export class SanPhamBienThe{
    id : number
    maMau : string
    tenSize :string
    tenSanPham :string
    colorId : number 
    prodId : number
    sizeId : number
    stock : number
  }