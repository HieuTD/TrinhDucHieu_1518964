import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { CouponService, DiscountCode } from './coupon.service';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { ToastServiceService } from '../../shared/toast-service.service';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import * as signalR from '@microsoft/signalr';
import { CreateOrEditCouponComponent } from './create-or-edit-coupon/create-or-edit-coupon.component';

@Component({
  selector: 'app-coupons',
  templateUrl: './coupons.component.html',
  styleUrls: ['./coupons.component.scss']
})
export class CouponsComponent implements OnInit {
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  constructor(public service: CouponService,
    public router: Router,
    public http: HttpClient,
    public dialog: MatDialog,
    public toastService: ToastServiceService) { }
  displayedColumns: string[] = ['id', 'code', 'sotiengiam',
    'actions'];
  ngOnInit(): void {
    this.service.getAllMaGiamGias();
  }
  ngAfterViewInit(): void {
    this.service.dataSource.sort = this.sort;
    this.service.dataSource.paginator = this.paginator;
  }
  onModalDialog() {
    this.service.magiamgia = new DiscountCode()
    this.dialog.open(CreateOrEditCouponComponent)
  }
  doFilter = (value: string) => {
    this.service.dataSource.filter = value.trim().toLocaleLowerCase();
  }
  populateForm(selectedRecord: DiscountCode) {
    this.service.magiamgia = Object.assign({}, selectedRecord)
    this.dialog.open(CreateOrEditCouponComponent)
  }
  clickDelete(id) {
    if (confirm('Bạn có chắc chắn xóa bản ghi này không ?')) {
      this.service.delete(id).subscribe(
        res => {
          this.toastService.showToastXoaThanhCong()
          this.service.getAllMaGiamGias()
        },
        err => {
          this.toastService.showToastXoaThatBai()
        }
      )
    }
  }
}