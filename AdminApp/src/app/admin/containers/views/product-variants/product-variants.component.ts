import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { SanPhamBienThe, ProductVariantService } from './product-variant.service';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { ToastServiceService } from '../../shared/toast-service.service';
import * as signalR from '@microsoft/signalr';
import { CreateOrEditProductVariantComponent } from './create-or-edit-product-variant/create-or-edit-product-variant.component';

@Component({
  selector: 'app-product-variants',
  templateUrl: './product-variants.component.html',
  styleUrls: ['./product-variants.component.scss']
})
export class ProductVariantsComponent implements OnInit, AfterViewInit {
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  productList: any[];
  constructor(public service: ProductVariantService,
    public router: Router,
    public http: HttpClient,
    public dialog: MatDialog,
    public serviceToast: ToastServiceService,) { }
  displayedColumns: string[] = ['id', 'sanPham', 'mauLoai', 'sizeLoai', 'soLuongTon',
    'actions'];
  public sanphambienthe: SanPhamBienThe
  ngOnInit(): void {
    this.service.getAllGiaSanPhamMauSacSanPhamSizes();
  }
  ngAfterViewInit(): void {
    this.service.dataSource.sort = this.sort;
    this.service.dataSource.paginator = this.paginator;
  }
  onModalDialog() {
    this.service.sanphambienthe = new SanPhamBienThe()
    this.dialog.open(CreateOrEditProductVariantComponent)
  }
  doFilter = (value: string) => {
    this.service.dataSource.filter = value.trim().toLocaleLowerCase();
  }
  populateForm(selectedRecord: SanPhamBienThe) {
    this.service.sanphambienthe = Object.assign({}, selectedRecord)
    this.dialog.open(CreateOrEditProductVariantComponent)
  }
  clickDelete(id) {
    if (confirm('Bạn có chắc chắn xóa bản ghi này không ?')) {
      this.service.delete(id).subscribe(
        res => {
          this.serviceToast.showToastXoaThanhCong()
          this.service.getAllGiaSanPhamMauSacSanPhamSizes()
        },
        err => {
          this.serviceToast.showToastXoaThatBai()
        }
      )
    }
  }
}