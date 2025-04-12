import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { ColorService, MauSac } from './color.service';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { ToastServiceService } from '../../shared/toast-service.service';
import * as signalR from '@microsoft/signalr';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { CreateOrEditColorComponent } from './create-or-edit-color/create-or-edit-color.component';

@Component({
  selector: 'app-colors',
  templateUrl: './colors.component.html',
  styleUrls: ['./colors.component.scss']
})
export class ColorsComponent implements OnInit, AfterViewInit {
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  productList: any[];
  constructor(public service: ColorService,
    public router: Router,
    public http: HttpClient,
    public dialog: MatDialog,
    public serviceToast: ToastServiceService,) { }
  displayedColumns: string[] = ['id', 'maMau', 'tenLoai',
    'actions'];
  ngOnInit(): void {
    this.service.getAllMauSacs();
  }
  ngAfterViewInit(): void {
    this.service.dataSource.sort = this.sort;
    this.service.dataSource.paginator = this.paginator;
  }
  onModalDialog() {
    this.service.mausac = new MauSac()
    this.dialog.open(CreateOrEditColorComponent)
  }
  doFilter = (value: string) => {
    this.service.dataSource.filter = value.trim().toLocaleLowerCase();
  }
  populateForm(selectedRecord: MauSac) {
    this.service.mausac = Object.assign({}, selectedRecord)
    this.dialog.open(CreateOrEditColorComponent)
  }
  clickDelete(id) {
    if (confirm('Bạn có chắc chắn xóa bản ghi này không ?')) {
      this.service.delete(id).subscribe(
        res => {
          this.service.getAllMauSacs()
          this.serviceToast.showToastXoaThanhCong()
        }, err => {
          this.serviceToast.showToastXoaThatBai()
        }
      )
    }
  }
}