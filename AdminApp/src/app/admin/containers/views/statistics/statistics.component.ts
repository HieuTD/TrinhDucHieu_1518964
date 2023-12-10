import { Component, NgZone, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { StatisticService } from './statistic.service';
import * as signalR from '@microsoft/signalr';

@Component({
  selector: 'app-statistics',
  templateUrl: './statistics.component.html',
  styleUrls: ['./statistics.component.scss']
})
export class StatisticsComponent implements OnInit {
  dataSourceBrand: { chart: { caption: string; plottooltext: string; showLegend: string; showPercentValues: string; legendPosition: string; useDataPlotColorForLabels: string; enablemultislicing: string; showlegend: string; theme: string; }; data: { label: string; value: string; }[]; };
  nam2021: any;
  errorMessage: any;
  dataThongKe: any;
  soLanXuatHien: any;
  doanhthucaonhat: any;
  nam2021soluong: any;
  soLuongTon: any;
  constructor(public service: StatisticService, public dialog: MatDialog,
    public zone: NgZone) {

  }
  public dataSourceYear: any = {
    chart: {
      caption: 'Doanh thu các tháng trong năm 2023',
      xAxisName: 'Tháng',
      yAxisName: 'Số tiền bán được',
      numberSuffix: '',
      theme: 'fusion'
    },
    data: [
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" },
    ]
  }
    ;
  public dataSourceSoLanXuatHien: any = {
    chart: {
      caption: 'Top 10 sản phẩm bán chạy nhất theo số lượng',
      xAxisName: 'Tên sản phẩm',
      yAxisName: 'Số lượng đã bán',
      numberSuffix: '',
      theme: 'fusion'
    },
    data: [
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" }
    ]
  }
  public dataSourceDoanhThu: any = {
    chart: {
      caption: 'sản phẩm biến thể đạt top 10 doanh số cao nhất',
      xAxisName: 'Tên sản phẩm',
      yAxisName: 'Doanh thu',
      numberSuffix: '',
      theme: 'fusion'
    },
    data: [
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" },
      { label: "", value: "" },
    ]
  }
  ngOnInit(): void {
    this.getTop10SanPhamLoiNhats()
    this.getSoLanXuatHienTrongDonHang()
    this.getThongKeThang();
  }

  //quantrong
  getThongKeThang() {
    this.service.getThongKeThang().subscribe(
      (result: any) => {
        this.dataThongKe = result as any
        console.log(this.dataThongKe);
        for (var i = 0; i < this.dataThongKe.length; i++) {
          this.dataSourceYear.data[this.dataThongKe[i].month].label = this.dataThongKe[i].month as any
          this.dataSourceYear.data[this.dataThongKe[i].month].value = this.dataThongKe[i].profit as any
        }
      },
      error => {
        this.errorMessage = <any>error
      }
    )
  }
  //quantrong
  //Quan trong
  lengthtopsolan: any
  getSoLanXuatHienTrongDonHang() {
    this.service.getSoLanSanPhamXuatHienTrongDonHang().subscribe(
      result => {
        this.soLanXuatHien = result as any
        this.lengthtopsolan = this.soLanXuatHien.length
        for (var i = 0; i < this.lengthtopsolan; i++) {
          this.dataSourceSoLanXuatHien.data[i].label = this.soLanXuatHien[i].productName
          this.dataSourceSoLanXuatHien.data[i].value = this.soLanXuatHien[i].count
        }
      },
      error => {
        this.errorMessage = <any>error
        console.log(error);
      }
    )
  }
  //quantrong
  ///Quan trong
  lengthtop: any
  getTop10SanPhamLoiNhats() {
    this.service.getSanPhamDoanhThuTop().subscribe(
      result => {
        this.doanhthucaonhat = result as any
        this.lengthtop = this.doanhthucaonhat.length
        for (var i = 0; i < this.lengthtop; i++) {
          this.dataSourceDoanhThu.data[i].label = this.doanhthucaonhat[i].productName
          this.dataSourceDoanhThu.data[i].value = this.doanhthucaonhat[i].profit
        }
      }
    )
  }


}
