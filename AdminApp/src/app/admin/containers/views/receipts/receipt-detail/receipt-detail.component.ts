import { Component, Input, OnInit } from '@angular/core';
import { ReceiptService } from '../receipt.service';

@Component({
  selector: 'app-receipt-detail',
  templateUrl: './receipt-detail.component.html',
  styleUrls: ['./receipt-detail.component.scss']
})
export class ReceiptDetailComponent implements OnInit {
  @Input() childMessage: string;
  constructor(private service: ReceiptService) { }
  phieunhapchitietphieunhap: any
  ngOnInit(): void {
    this.service.getdetailphieunhap(this.service.idphieunhap).subscribe(
      result => {
        this.phieunhapchitietphieunhap = result as any
        console.log(this.phieunhapchitietphieunhap);
      },
      error => {
      }
    )
  }
}
