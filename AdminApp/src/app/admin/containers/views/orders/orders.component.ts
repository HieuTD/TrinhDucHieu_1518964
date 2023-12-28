import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { HoaDonUser, OrderService } from './order.service';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { ToastServiceService } from '../../shared/toast-service.service';
import { DatePipe } from '@angular/common';
import * as signalR from '@microsoft/signalr';
import { EditOrderComponent } from './edit-order/edit-order.component';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss']
})
export class OrdersComponent implements OnInit {
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  productList: any[];
  constructor(public service: OrderService,
    public router: Router,
    public http: HttpClient,
    public dialog: MatDialog,
    public serviceToast: ToastServiceService,
    public datepipe: DatePipe
  ) { }
  displayedColumns: string[] = ['id', 'id_User', 'ngayTao', 'ghiChi', 'tongTien', 'trangThai', 'thanhToan', 'actions'];
  ngOnInit(): void {
    this.service.getAllHoaDons();
  }
  ngAfterViewInit(): void {
    this.service.dataSource.sort = this.sort;
    this.service.dataSource.paginator = this.paginator;
  }
  routeChiTiet(selectedRecord: HoaDonUser) {
    this.service.hoadon = Object.assign({}, selectedRecord)
    this.router.navigate(['admin/hoadon/detail/' + this.service.hoadon.id])
  }
  doFilter = (value: string) => {
    this.service.dataSource.filter = value.trim().toLocaleLowerCase();
  }
  someMethod(value: any, element: any) {
    console.log("selected value", value);
    console.log("selected element", element);
    element.daLayTien = value;
  }
  populateForm(selectedRecord: HoaDonUser) {
    this.service.hoadon = Object.assign({}, selectedRecord)
    this.dialog.open(EditOrderComponent)
  }

  clickDelete(id) {
    if (confirm('Bạn có chắc chắn xóa bản ghi này không ?')) {
      this.service.delete(id).subscribe(
        res => {
          this.service.getAllHoaDons()
          this.serviceToast.showToastXoaThanhCong();
        },
        err => {
          this.serviceToast.showToastXoaThatBai();
        }
      )
    }
  }
}
