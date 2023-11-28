import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Blog, BlogService } from './blog.service';
import * as signalR from '@microsoft/signalr';
import { CreateOrEditBlogComponent } from './create-or-edit-blog/create-or-edit-blog.component';
@Component({
  selector: 'app-blogs',
  templateUrl: './blogs.component.html',
  styleUrls: ['./blogs.component.scss']
})
export class BlogsComponent implements OnInit {
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  constructor(
    public service: BlogService,
    public router: Router,
    public http: HttpClient,
    public dialog: MatDialog,
    public toastr: ToastrService,
  ) { }
  displayedColumns: string[] = ['id', 'tieude', 'hinh', 'noidung', 'actions'];
  public blog: Blog
  ngOnInit(): void {
    this.service.getAllBlogs()
  }
  ngAfterViewInit(): void {
    this.service.dataSource.sort = this.sort;
    this.service.dataSource.paginator = this.paginator;
  }
  onModalDialog() {
    this.service.blog = new Blog()
    this.dialog.open(CreateOrEditBlogComponent)
  }
  doFilter = (value: string) => {
    this.service.dataSource.filter = value.trim().toLocaleLowerCase();
  }
  showToastXoaThanhCong() {
    this.toastr.success("Xóa thành công")
  }
  showToastXoaThatBai() {
    this.toastr.error("Xóa thất bại")
  }
  populateForm(selectedRecord: Blog) {
    this.service.blog = Object.assign({}, selectedRecord)
    this.dialog.open(CreateOrEditBlogComponent)
    console.log(selectedRecord)
  }
  clickDelete(id) {
    if (confirm('Bạn có chắc chắn xóa bản ghi này không ?')) {
      this.service.delete(id).subscribe(
        res => {
          this.service.getAllBlogs()
          this.showToastXoaThanhCong()
        }
        , err => {
          this.showToastXoaThatBai()
        }
      )
    }
  }
}