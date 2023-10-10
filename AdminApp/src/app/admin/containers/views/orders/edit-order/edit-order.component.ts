import { Component, OnInit } from '@angular/core';
import { OrderService } from '../order.service';

@Component({
  selector: 'app-edit-order',
  templateUrl: './edit-order.component.html',
  styleUrls: ['./edit-order.component.scss']
})
export class EditOrderComponent implements OnInit {
  constructor(public service: OrderService) { }
  ngOnInit(): void {
  }
  onSubmit(hd:any){
    console.log("log ket qua la: ",hd);
    this.service.put(hd).subscribe(
      result=>{
         this.service.getAllHoaDons() 
      },
      error=>{
      })
  }
}
