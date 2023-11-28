import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { PhieuNhap, ReceiptService } from './receipt.service';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { ToastServiceService } from '../../shared/toast-service.service';
import * as signalR from '@microsoft/signalr';

@Component({
  selector: 'app-receipts',
  templateUrl: './receipts.component.html',
  styleUrls: ['./receipts.component.scss']
})
export class ReceiptsComponent implements OnInit, AfterViewInit {
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  constructor(
    public service: ReceiptService,
    public router: Router,
    public http: HttpClient,
    public dialog: MatDialog,
    public serviceToast: ToastServiceService
  ) { }
  displayedColumns: string[] = [
    "id",
    "soChungTu",
    "tenNhaCungCap",
    "ngayTao",
    "tongTien",
    "nguoiLapPhieu",
    "actions",
  ];
  ngOnInit(): void {
    this.service.getAllPhieuNhaps();
  }
  doFilter = (value: string) => {
    this.service.dataSource.filter = value.trim().toLocaleLowerCase();
  };
  ngAfterViewInit(): void {
    this.service.dataSource.sort = this.sort;
    this.service.dataSource.paginator = this.paginator;
  }
  addphieu() {
    this.service.phieunhap = new PhieuNhap();
    this.router.navigate(["admin/taophieunhap/them"]);
  }
  populateForm(id: any) {
    this.service.idphieunhap = id;
    this.router.navigate(["admin/taophieunhap/detail/" + id]);
  }
  clickDelete(id) {
    if (confirm("Bạn có chắc chắn xóa bản ghi này không ?")) {
      this.service.delete(id).subscribe(
        (res) => {
          this.serviceToast.showToastXoaThanhCong();
          this.service.getAllPhieuNhaps();
        },
        (err) => {
          this.serviceToast.showToastXoaThatBai();
        }
      );
    }
  }
}
