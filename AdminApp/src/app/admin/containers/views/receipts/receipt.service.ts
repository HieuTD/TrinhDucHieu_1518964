import { HttpClient } from '@angular/common/http';
import { Injectable, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
@Injectable({
  providedIn: 'root'
})
export class ReceiptService {
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  public dataSource = new MatTableDataSource<PhieuNhapNhaCungCap>();
  phieunhap: PhieuNhap = new PhieuNhap()
  sanpham: SanPham = new SanPham()
  phieunhapchange: PhieuNhapChange = new PhieuNhapChange()
  chitietphieunhap: ChiTietPhieuNhap = new ChiTietPhieuNhap()
  constructor(private http: HttpClient) { }
  phieunhappost: PostPhieuNhap = new PostPhieuNhap()
  idphieunhap: number
  getphieunhaphttp(): Observable<any> {
    return this.http.get(environment.URL_API + "receipts")
  }
  getnhacungcaphttp(): Observable<any> {
    return this.http.get(environment.URL_API + "suppliers")
  }
  //Lấy ra nhà cung cấp theo Id
  getonenhacungcaphttp(id): Observable<any> {
    return this.http.get(`${environment.URL_API}suppliers/${id}`)
  }
  //Lấy ra sản phẩm dựa theo Id nhà cung cấp
  gettensanphamhttp(supplierid): Observable<any> {
    return this.http.get(`${environment.URL_API}products/listprod/${supplierid}`)
  }
  //Lấy ra sản phẩm biến thể dựa theo Id sản phẩm
  gettensanphambienthehttp(prodid): Observable<any> {
    return this.http.get(`${environment.URL_API}productVariants/listprodvariant/${prodid}`)
  }
  getdetailphieunhap(id: number): Observable<any> {
    return this.http.get<any>(environment.URL_API + "receipts/" + id)
  }
  post(data): Observable<any> {
    return this.http.post(environment.URL_API + "receipts", data)
  }
  delete(id: number) {
    return this.http.delete(`${environment.URL_API}/${id}`)
  }
  getAllPhieuNhaps() {
    this.getphieunhaphttp().subscribe(
      res => {
        this.dataSource.data = res as PhieuNhapNhaCungCap[];
        console.log(this.dataSource.data);
      }
    )
  }
}
export class SanPham {
  id: number = 0
  ten: string
  id_ncc: number
}
export class PhieuNhap {
  id: number = 0
  nguoiLapPhieu: string
  chiTiets: Array<ChiTietPhieuNhap>
}
export class ChiTietPhieuNhap {
  id: number = 0
  soLuongNhap: number = 100
  id_SanPhamBienThe: number
  tenSanPhamBienThe: string
}
export class PhieuNhapChange {
  nhacungcap: any
  sanpham: any
  sanphambienthe: any
  idspbt: any
}
export class UploadChiTietPhieuNhapHang {
  GiaNhapSanPhamBienThe: number
  TenSanPhamBienThe: string
  SoluongNhap: number
}
export class PostPhieuNhap {
  userId: string
  description: string
  totalPrice: number
  supplierId: string
  receiptDetails: any = []
}
export class PhieuNhapNhaCungCap {
  id: number
  soChungTu: string
  ngayTao: any
  tongTien: number
  ghiChu: string
  nguoiLapPhieu: string
  tenNhaCungCap: string
}
