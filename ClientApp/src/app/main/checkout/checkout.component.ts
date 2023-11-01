import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import Swal from 'sweetalert2';
import { CartService } from 'src/app/service/product.service';
import { UserService } from 'src/app/service/account/user.service';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { ApiResponse } from 'src/app/model/product.model';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit {
  list_item: any;
  tongtien: any;
  Tinh: string;
  Xa: string;
  couponName: string;
  Huyen: string;
  public DiaChi: string;
  list_MGG: any;
  list_MGGSD: string[];
  check: any;
  check_sudung: any;
  tongThanhToan: any;
  list_tinh_thanh: any;
  list_quan_huyen: any;
  list_xa_phuong: any;
  id_user: string;
  public checkdiachi: boolean
  public soLuong: number;
  public DiaChiDefaul: string;
  public urlVNPay: string;
  constructor(public http: HttpClient, private cartService: CartService, private userService: UserService, public router: Router) {
    userService.checkLogin();
    this.id_user = localStorage.getItem('idUser');
    this.Tinh = null;
    this.http.get("https://provinces.open-api.vn/api/?depth=3").subscribe(res => {
      this.list_tinh_thanh = res;
    });
    // this.http.post(environment.URL_API+"Carts/getCart/"+this.id_user,{}).subscribe(
    this.http.get(environment.URL_API + "carts/getcartbyuserid/" + this.id_user, {}).subscribe(
      res => {
        this.list_item = res;
        console.log(this.list_item)
        this.tongtien = 0;
        for (let i = 0; i < this.list_item.length; i++) {
          this.tongtien = this.tongtien + (this.list_item[i].productDetail.price * this.list_item[i].quantity);
          this.tongThanhToan = this.tongtien + 25000;
        }
      });
    // this.http.get(environment.URL_API+"hoadons/magiamgia/").subscribe(res=>{
    this.http.get(environment.URL_API + "coupons").subscribe(res => {
      this.list_MGG = res;
    });
    //this.checkdiachi=true;
    this.list_MGGSD = [];
    this.check = null;
    // this.http.post(environment.URL_API+"Auth/getDiaChi/",{
    this.http.get(environment.URL_API + "users/getUserAddress/" + this.id_user, {responseType: 'text'}
    ).subscribe(
      res => {
        this.DiaChiDefaul = <string>res;
        this.DiaChi = this.DiaChiDefaul;
      }
    );
    this.checkdiachi = true;
  }
  changeValue(value) {
    this.checkdiachi = !value;
    if (this.check) {
      this.DiaChi = this.DiaChiDefaul;
    }
    else {
      this.DiaChi = '';
    }
  }
  deleteSanPham(item): void {
    const clicks = localStorage.getItem('idUser');
    let delproduct: deleteProduct = new deleteProduct()
    delproduct.prodVariantId = item.prodVariantId
    delproduct.userId = clicks
    console.log(delproduct)
    // this.http.post(environment.URL_API+"Carts/delete",delproduct
    this.http.post(environment.URL_API + "Carts/delete", delproduct
    ).subscribe(
      res => {
        Swal.fire("Xoá sản phẩm thành công .", '', 'success')
        // this.http.post(environment.URL_API+"Carts/getCart/"+clicks,{}).subscribe(
        this.http.get(environment.URL_API + "Carts/getcartbyuserid/" + clicks, {}).subscribe(
          res => {
            this.list_item = res;
            this.tongtien = 0;
            for (let i = 0; i < this.list_item.length; i++) {
              this.tongtien = this.tongtien + (this.list_item[i].productDetail.price * this.list_item[i].quantity);
              this.tongThanhToan = this.tongtien + 25000;
            }
            this.cartService.DeleteProduct(item.productDeatail);
          });
      })
  }
  ngOnInit(): void {
  }
  maGiamGia(): void {
    if (localStorage.getItem("products") === null) {
      Swal.fire("Giỏ hàng trống.", '', 'warning').then(function () {
      }
      )
    }
    else {
      //kiểm tra xem mã giảm giá có tồn tại không  
      this.check = this.list_MGG.filter(d => d.couponName == this.couponName)[0];
      //kiểm tra xem mã giảm giá được sử dụng chưa
      this.check_sudung = this.list_MGGSD.filter(d => d == this.couponName)[0];
      //nếu mã giảm giá tồn tại và chưa sử dụng thì 
      //1.add mã giảm giá đó vào list mã đã sử dụng
      //2.giảm tiền theo mã giảm giá
      if (this.check != null && this.check_sudung == null) {
        this.list_MGGSD.push(this.couponName)
        this.tongThanhToan = this.tongtien + 25000 - this.check.discount;
        Swal.fire("Áp dụng mã giảm giá thành công .", '', 'success')
      }
      else {
        if (this.check_sudung == this.couponName) {
          Swal.fire("Bạn đã áp mã này rồi.", '', 'error')
        }
        else {
          Swal.fire("Mã giảm giá không tồn tại .", '', 'error')
        }
      }
    }
  }

  onSubmitVnPay(): void {
    if (localStorage.getItem("products") === null) {
      Swal.fire("Giỏ hàng trống.", '', 'warning').then(() => { });
    } else {
      const clicks = localStorage.getItem('idUser');
      const val = {
        address: this.DiaChi,
        totalPrice: this.tongThanhToan - 25000,
        userId: clicks,
        typePayment: 0
      };

      this.http.post<ApiResponse>(environment.URL_API+"orders", val).subscribe(
        (res: ApiResponse) => {
          console.log(res);
          if (res.urlVNPay) {
            // Chuyển hướng đến URL trả về từ backend.
            Swal.fire("Đặt hàng thành công.", '', 'success').then(() => {
              window.location.href = res.urlVNPay;
              localStorage.removeItem('products');
            });
          }
        },
        error => {
          // Xử lý lỗi khi không nhận được URL trả về từ backend.
          console.error(error);
        }
      );
    }
  }


  onSubmit(): void {
    if (localStorage.getItem("products") === null) {
      Swal.fire("Giỏ hàng trống.", '', 'warning').then(() => { });
    } else {
      const clicks = localStorage.getItem('idUser');
      const val = {
        address: this.DiaChi,
        totalPrice: this.tongThanhToan - 25000,
        userId: clicks,
      };
      this.http.post(environment.URL_API + "orders", val).subscribe(
        res => {
          Swal.fire("Đặt hàng thành công.", '', 'success').then(function () {
            window.location.href = '/history';
            localStorage.removeItem('products');
          }
          );
        })
    }
  }

  ChangeSoLuong(cartID, i) {
    const clicks = localStorage.getItem('idUser');
    var val = {
      id: cartID,
      quantity: this.soLuong,
      userId: clicks
    };
    // this.http.post(environment.URL_API+"Carts/update/",val).subscribe(
    this.http.post(environment.URL_API + "Carts/update/", val).subscribe(
      res => {
        this.list_item = res;
        this.tongtien = 0;
        for (let i = 0; i < this.list_item.length; i++) {
          this.tongtien = this.tongtien + (this.list_item[i].productDetail.price * this.list_item[i].quantity);
          this.tongThanhToan = this.tongtien + 25000;
        }
      }
    );
    this.tongThanhToan = this.tongtien + 25000 - this.check.discount;
  }
  updateCongSanPham(cartID, soLuong) {
    const clicks = localStorage.getItem('idUser');
    var val = {
      id: cartID,
      quantity: soLuong + 1,
      userId: clicks
    };
    // this.http.post(environment.URL_API+"Carts/update/",val).subscribe(
    this.http.put(environment.URL_API + "Carts/update/", val).subscribe(
      res => {
        this.list_item = res;
        this.tongtien = 0;
        for (let i = 0; i < this.list_item.length; i++) {
          this.tongtien = this.tongtien + (this.list_item[i].productDetail.price * this.list_item[i].quantity);
          this.tongThanhToan = this.tongtien + 25000;
        }
      }
    );
    this.tongThanhToan = this.tongtien + 25000 - this.check.discount;
  }
  updateTruSanPham(cartID, soLuong) {
    const clicks = localStorage.getItem('idUser');
    if (soLuong == 1) {
      soLuong = 0;
    }
    else {
      soLuong = soLuong - 1;
    }
    var val = {
      id: cartID,
      quantity: soLuong,
      userId: clicks
    };
    // this.http.post(environment.URL_API+"Carts/update/",val).subscribe(
    this.http.put(environment.URL_API + "Carts/update/", val).subscribe(
      res => {
        this.list_item = res;
        this.tongtien = 0;
        for (let i = 0; i < this.list_item.length; i++) {
          this.tongtien = this.tongtien + (this.list_item[i].productDetail.price * this.list_item[i].quantity);
          this.tongThanhToan = this.tongtien + 25000;
        }
      }
    );
    this.tongThanhToan = this.tongtien + 25000 - this.check.discount;
  }
  changTinhThanh(event: any): void {
    this.Tinh = event;
    const tinh = event;
    const search = this.list_tinh_thanh.filter(d => d.name === tinh)[0];
    this.list_quan_huyen = search.districts;
  }
  changHuyenQuan(event: any): void {
    this.Huyen = event;
    const Huyen = event;
    const search = this.list_quan_huyen.filter(d => d.name === Huyen)[0];
    this.list_xa_phuong = search.wards;
  }
}
export class deleteProduct {
  prodVariantId: number
  userId: string
}
