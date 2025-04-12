import { Component, OnInit } from '@angular/core'
import { Product, ProductService } from './product.service'
import * as signalR from '@microsoft/signalr';
import { AfterViewInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { map } from 'rxjs/operators';
import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { MatDialog } from '@angular/material/dialog';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { MatPaginator } from '@angular/material/paginator';
import { MatTable, MatTableDataSource } from '@angular/material/table';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { MatAccordion } from '@angular/material/expansion';
import { ToastServiceService } from '../../shared/toast-service.service';
import { environment } from '../../../../../environments/environment';
import { ProductVariantService } from '../product-variants/product-variant.service';
import { ToastrService } from 'ngx-toastr';
@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss']
})
export class ProductsComponent implements OnInit, AfterViewInit {
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  constructor(public servicespbt: ProductVariantService,
    public service: ProductService,
    private breakpointObserver: BreakpointObserver,
    public router: Router,
    public http: HttpClient,
    public dialog: MatDialog,
    public serviceToast: ToastServiceService,
    public toastr: ToastrService) { }
  displayedColumns: string[] = ['id', 'ten', 'hinh',
    'gia',
    'giaNhap',
    'trangThaiSanPham',
    'trangThaiHoatDong',
    'tenNhanHieu',
    'tenLoai',
    'toggle',
    'actions'
  ];
  updateActiveStatus(element: Product) {
    var checkFeatured;
    if (element.isFeatured == true) {
      checkFeatured = 'Tắt';
    } else {
      checkFeatured = 'Bật';
    }
    this.service.putHoatDong(element.id, element).subscribe(
      result => {
        this.service.getAllProducts()
        this.toastr.success(`${checkFeatured + " trạng thái hoạt động của sản phẩm thành công"}`, 'Thông báo', { timeOut: 2000 });
      },
      error => {
        console.log(error);
      }
    )
    console.log("element là: ", element);
    console.log("element.trangThaiHoatDong là: ", element.isFeatured);
  }
  ngOnInit() {
    this.service.getAllProducts();
  }
  ngAfterViewInit(): void {
    this.service.dataSource.sort = this.sort;
    this.service.dataSource.paginator = this.paginator;
  }
  public doFilter = (value: string) => {
    this.service.dataSource.filter = value.trim().toLocaleLowerCase();
  }
  onSelectedAdd() {
    this.service.product = new Product();
    this.router.navigate(['admin/product/add']);
  }
  onselectedDetail() {
    this.router.navigate(['admin/product/detail/' + this.service.product.id]);
  }
  onSelectedEdit() {
    this.router.navigate(['admin/product/edit/' + this.service.product.id]);
  }
  detail(selectedRecord: Product) {
    this.service.product = Object.assign({}, selectedRecord)
    this.onselectedDetail()
  }
  populateForm(selectedRecord: Product) {
    this.service.product = Object.assign({}, selectedRecord)
    this.onSelectedEdit();
  }
  clickDelete(id) {
    if (confirm('Bạn có chắc chắn xóa bản ghi này không ?')) {
      this.service.delete(id).subscribe(
        res => {
          this.service.getAllProducts()
          this.serviceToast.showToastXoaThanhCong()
        }
        , err => {
          this.serviceToast.showToastXoaThatBai()
        }
      )
    }
  }
}