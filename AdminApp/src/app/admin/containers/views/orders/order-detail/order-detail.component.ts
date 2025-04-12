import { Component, OnInit } from '@angular/core';
import { OrderService } from '../order.service';
import { environment } from '../../../../../../environments/environment';

@Component({
  selector: 'app-order-detail',
  templateUrl: './order-detail.component.html',
  styleUrls: ['./order-detail.component.scss']
})
export class OrderDetailComponent implements OnInit {
  constructor(private service: OrderService) {
  }
  url: any
  hd: any
  ngOnInit(): void {
    this.url = environment.URL_API
    this.getMotHoaDon(this.service.hoadon.id)
  }

  getMotHoaDon(id: any) {
    this.service.getMotHoaDonService(id).subscribe(
      res => {
        this.hd = res as any;
        console.log("chi tiet hoa don la: ", this.hd);
      }
    )
  }
}
