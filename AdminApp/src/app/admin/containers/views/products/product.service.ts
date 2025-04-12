import { Injectable, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { environment } from '../../../../../environments/environment';
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class ProductService {
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  public dataSource = new MatTableDataSource<Product>();
  constructor(public http: HttpClient) { }
  product: Product = new Product();
  delete(id: number): Observable<any> {
    return this.http.delete<any>(`${environment.URL_API + "products"}/${id}`)
  }
  get(): Observable<any> {
    return this.http.get<any>(environment.URL_API + "products")
  }
  getAllProducts() {
    return this.get().subscribe(res => {
      this.dataSource.data = res as Product[]
    })
  }
  //
  post(product: any): Observable<any> {
    return this.http.post<any>(environment.URL_API + 'products', product)
  }
  put(id: number, product: any): Observable<any> {
    return this.http.put<any>(environment.URL_API + 'products/' + id, product)
  }
  putHoatDong(id: number, element: Product): Observable<any> {
    return this.http.put<any>(environment.URL_API + 'products/updateproductfeature/' + id, element)
  }
}
export class Product {
  id: number = 0
  name: string
  discount: number = 0
  description: string
  khoiLuong: number
  price: number
  originalPrice: number
  gender: number
  tag: string
  huongDan: string
  material: string
  status: string
  isFeatured: boolean
  trangThaiSanPhamThietKe: string
  brandId: number
  categoryId: number
  supplierId: number
  giaSanPhams: number
  sanPhamThietKes: number
  sanPham_SanPhamThietKe: number
  brandName: string
  categoryName: string
  soLuongLike: number
  soLuongComment: number
}
