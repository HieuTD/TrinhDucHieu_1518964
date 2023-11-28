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
export class SupplierService {
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  public dataSource = new MatTableDataSource<NhaCungCap>();
  nhacungcap: NhaCungCap = new NhaCungCap()
  constructor(public http: HttpClient) { }
  delete(id: number) {
    return this.http.delete(`${environment.URL_API}suppliers/${id}`)
  }
  gethttp(): Observable<any> {
    return this.http.get(environment.URL_API + "suppliers")
  }
  getAllNhaCungCaps() {
    this.gethttp().subscribe(
      res => {
        this.dataSource.data = res as NhaCungCap[];
      }
    )
  }
}
export class NhaCungCap {
  id: number = 0
  supplierName: string
  phoneNumber: string
  description: string
  address: string
}
