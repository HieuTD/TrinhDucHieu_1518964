import { HttpClient } from "@angular/common/http";
import { Injectable, ViewChild } from "@angular/core";
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { MatTableDataSource } from "@angular/material/table";
import { environment } from "../../../../../environments/environment";
import { User } from "./usermanagers.component";
@Injectable({
    providedIn: 'root'
  })
export class UserManagersService{
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  public dataSource = new MatTableDataSource<User>();
    user:User = new User()
    constructor(public http:HttpClient) { }
    get(){
      return this.http.get(environment.URL_API+"users")
    }
    delete(id:number){
      return this.http.delete(`${environment.URL_API+"users"}/${id}`)
    }
    getAllUsers(){
      this.http.get(environment.URL_API+"users").subscribe(
        res=>{
          this.dataSource.data = res as User[];
        }
      )
    }
  }
