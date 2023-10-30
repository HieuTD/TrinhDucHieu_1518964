import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { environment } from 'src/environments/environment';
import Swal from 'sweetalert2';
@Component({
  selector: 'app-contact',
  templateUrl: './bill_details.component.html',
  styleUrls: ['./bill_details.component.scss']
})
export class BillDetailsComponent implements OnInit {
  list_hoadon:any;
  id_bill:any;
  bill:any;
  bill_details:any;
  info_user:any;
  user:any;
  tongtien:any;
  status: any;
  constructor(public http:HttpClient,public route: ActivatedRoute) {
    this.route.params.subscribe(params => {
      this.id_bill = params['id'];
      // this.http.post(environment.URL_API+"hoadons/hoadon/"+this.id_bill,{
      this.http.get("https://localhost:44391/api/"+"orders/"+this.id_bill,{
  }).subscribe(
    res=>{
      this.bill=res;
    });
    // this.http.post(environment.URL_API+"chitiethoadons/chitiethoadon/"+this.id_bill,{
    this.http.get("https://localhost:44391/api/"+"orderdetails/"+this.id_bill,{
  }).subscribe(
    res=>{
      this.bill_details=res;
      this.tongtien=0;
      for (let i = 0; i < this.bill_details.length; i++) {
        this.tongtien=this.tongtien+(this.bill_details[i].price*this.bill_details[i].quantity)
    }
    });
  });
}
Huy(){
  var val = {
    status: 3
  };
  // this.http.post(environment.URL_API+"chitiethoadons/huydon/"+this.id_bill,{
  this.http.put("https://localhost:44391/api/"+"orders/updatestatus/"+this.id_bill, val).subscribe(
    res=>{
      Swal.fire("Huỷ đơn hàng thành công.", '', 'success')
      window.location.href = '/history';
      this.loadChiTietPhieu()
    })
}
loadChiTietPhieu()
{
  // this.http.post(environment.URL_API+"hoadons/hoadon/"+this.id_bill,{
    this.http.get("https://localhost:44391/api/"+"orders/"+this.id_bill,{
  }).subscribe(
    res=>{
      this.bill=res;
    });
    // this.http.post(environment.URL_API+"chitiethoadons/chitiethoadon/"+this.id_bill,{
    this.http.get("https://localhost:44391/api/"+"orderdetails/"+this.id_bill,{
  }).subscribe(
    res=>{
      this.bill_details=res;
      this.tongtien=0;
      for (let i = 0; i < this.bill_details.length; i++) {
        this.tongtien=this.tongtien+(this.bill_details[i].price*this.bill_details[i].quantity)
    }
    });
 }
  ngOnInit(): void {
  }
}
