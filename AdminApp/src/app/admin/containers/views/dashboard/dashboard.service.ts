import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../../../environments/environment';
import { ToastServiceService } from '../../shared/toast-service.service';
import { options } from 'fusioncharts';
@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  constructor(private http: HttpClient) { } 
  getCountProduct(): Observable<number> {
    // return this.http.get<number>(environment.URL_API + "ThongKeSoLuongs/countproduct")
    return this.http.get<number>("https://localhost:44391/api/" + "Statistics/productcount")
  }
  getCountOrder(): Observable<number> {
    // return this.http.get<number>(environment.URL_API + "ThongKeSoLuongs/countorder")
    return this.http.get<number>("https://localhost:44391/api/" + "Statistics/ordercount")
  }
  getCountUser(): Observable<number> {
    // return this.http.get<number>(environment.URL_API + "ThongKeSoLuongs/countuser")
    return this.http.get<number>("https://localhost:44391/api/" + "Statistics/usercount")
  }
  getCountTotalMoney(): Observable<number> {
    // return this.http.get<number>(environment.URL_API + "ThongKeSoLuongs/countmoney")
    return this.http.get<number>("https://localhost:44391/api/" + "Statistics/moneycount")
  }
  getTopDataSetBanRaTonKho():Observable<any>{
    return this.http.get<any>(environment.URL_API+"ThongKeBieuDos/topdatasetbanratonkho") 
 }
}
