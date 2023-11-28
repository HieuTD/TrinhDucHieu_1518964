import { Component, OnInit, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { UserManagersService } from './usermanagers.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-usermanagers',
  templateUrl: './usermanagers.component.html',
  styleUrls: ['./usermanagers.component.scss']
})
export class UsermanagersComponent implements OnInit {
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  productList: any[];
  constructor(public service: UserManagersService,
    public router: Router,
    public http: HttpClient,
    public dialog: MatDialog,
    public toast: ToastrService) { }
  public dataSource = new MatTableDataSource<User>();
  displayedColumns: string[] = ['id', 'firstName', 'lastName',
    'userName', 'quyen', 'actions'];
  ngOnInit(): void {
    this.service.getAllUsers();
  }
  ngAfterViewInit(): void {
    this.service.dataSource.sort = this.sort;
    this.service.dataSource.paginator = this.paginator;
  }
  onModalDialog() {
    this.service.user = new User()
  }
  doFilter = (value: string) => {
    this.service.dataSource.filter = value.trim().toLocaleLowerCase();
  }
  populateForm(selectedRecord: User) {
    this.service.user = Object.assign({}, selectedRecord)
  }
  clickDelete(id) {
    if (confirm('Bạn có chắc chắn xóa bản ghi này không ?')) {
      this.service.delete(id).subscribe(
        res => {
          this.toast.success("Xóa người dùng thành công")
          this.service.getAllUsers()
        },
        error => {
          this.toast.error("Xóa người dùng thất bại")
        }
      )
    }
  }
}
export class User {
  id: string
  imagePath: string
  userName: string
  lastName: string
  firstName: string
  quyen: string
}