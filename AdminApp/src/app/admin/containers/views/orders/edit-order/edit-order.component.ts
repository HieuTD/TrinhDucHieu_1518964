import { Component, OnInit } from '@angular/core';
import { OrderService } from '../order.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-edit-order',
  templateUrl: './edit-order.component.html',
  styleUrls: ['./edit-order.component.scss']
})
export class EditOrderComponent implements OnInit {
  constructor(public service: OrderService,
    public toastr: ToastrService) { }
  ngOnInit(): void {
  }
  onSubmit(hd: any) {
    console.log("log ket qua la: ", hd);
    this.service.put(hd).subscribe(
      result => {
        this.service.getAllHoaDons()
        this.toastr.success('Cập nhật trạng thái đơn hàng thành công');
      },
      error => {
        this.toastr.error('Cập nhật trạng thái đơn hàng thất bại');
      })
  }
}
