import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
@Injectable({
  providedIn: 'root'
})
export class StatisticService {
  constructor(private http: HttpClient) { }
  getThongKeThang(): Observable<any> {
    // return this.http.get<any>(environment.URL_API + "statistics/topprofitbymonth")
    return this.http.get<any>(environment.URL_API  + "statistics/topprofitbymonth")
    
  }
  getSoLanSanPhamXuatHienTrongDonHang():Observable<any>{
    // return this.http.get<any>(environment.URL_API+"statistics/toporderproducts")
    return this.http.get<any>(environment.URL_API +"statistics/toporderproducts")
    
  }
  getSanPhamDoanhThuTop():Observable<any>{
    // return this.http.get<any>(environment.URL_API+"statistics/topprofitproducts")
    return this.http.get<any>(environment.URL_API +"statistics/topprofitproducts")
  }

  dataThongKeNgay: any
  public dataSourceNgay: any = {
    chart: {
      caption: 'Doanh thu',
      xAxisName: 'Ngày',
      yAxisName: 'Số tiền thu về',
      numberSuffix: '',
      theme: 'umber'
    },
    data: [
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" }
    ]
  }
}
