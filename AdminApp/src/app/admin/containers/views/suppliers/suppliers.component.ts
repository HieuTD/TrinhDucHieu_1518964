import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { NhaCungCap, SupplierService } from './supplier.service';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { ToastServiceService } from '../../shared/toast-service.service';
import * as signalR from '@microsoft/signalr';
import { CreateOrEditSupplierComponent } from './create-or-edit-supplier/create-or-edit-supplier.component';

@Component({
  selector: 'app-suppliers',
  templateUrl: './suppliers.component.html',
  styleUrls: ['./suppliers.component.scss']
})
export class SuppliersComponent implements OnInit, AfterViewInit {
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  productList: any[];
  constructor(public service: SupplierService,
    public router: Router,
    public http: HttpClient,
    public dialog: MatDialog,
    public serviceToast: ToastServiceService,) { }
  displayedColumns: string[] = ['id', 'ten', 'sdt', 'thongTin', 'diaChi',
    'actions'];
  ngOnInit(): void {
    this.service.getAllNhaCungCaps();
  }
  ngAfterViewInit(): void {
    this.service.dataSource.sort = this.sort;
    this.service.dataSource.paginator = this.paginator;
  }
  onModalDialog() {
    this.service.nhacungcap = new NhaCungCap()
    this.dialog.open(CreateOrEditSupplierComponent)
  }
  doFilter = (value: string) => {
    this.service.dataSource.filter = value.trim().toLocaleLowerCase();
  }
  populateForm(selectedRecord: NhaCungCap) {
    this.service.nhacungcap = Object.assign({}, selectedRecord)
    this.dialog.open(CreateOrEditSupplierComponent)
  }
  clickDelete(id) {
    if (confirm('Bạn có chắc chắn xóa bản ghi này không ?')) {
      this.service.delete(id).subscribe(
        res => {
          this.serviceToast.showToastXoaThanhCong()
          this.service.getAllNhaCungCaps()
        }, err => {
          this.serviceToast.showToastXoaThatBai()
        }
      )
    }
  }
}