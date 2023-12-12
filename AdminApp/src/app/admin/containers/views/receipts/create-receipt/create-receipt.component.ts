import { Component, OnInit } from '@angular/core';
import { ReceiptService } from '../receipt.service';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { ToastServiceService } from '../../../shared/toast-service.service';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-create-receipt',
  templateUrl: './create-receipt.component.html',
  styleUrls: ['./create-receipt.component.scss']
})
export class CreateReceiptComponent implements OnInit {
  constructor(public service: ReceiptService,
    private http: HttpClient,
    private route: Router,
    private serviceToast: ToastServiceService,
    private _formBuilder: FormBuilder
  ) { }
  chitiets: any = []
  nhacungcaps: any[] = [];
  sanphams: any[] = [];
  sanphambienthes: any[] = [];
  motnhacungcap: any
  idUser: any
  ngOnInit(): void {
    this.service.getnhacungcaphttp().subscribe(
      data => {
        Object.assign(this.nhacungcaps, data)
      }
    )
    this.firstFormGroup = this._formBuilder.group({
      firstCtrl: []
    });
    this.secondFormGroup = this._formBuilder.group({
      secondCtrl: []
    });
    this.newFormGroupChiTiet = new FormGroup({
      ProdVariantPrice: new FormControl(null,
        [
        ]),
        ProdVariantName: new FormControl(null,
        [
          Validators.required
        ]),
      Quantity: new FormControl(100,
        [
          Validators.required,
        ]),
    });
    this.newFormGroupPhieuNhap = new FormGroup({
      GhiChu: new FormControl(null,
        [
          Validators.required,
        ]),
    })
  }
  public firstFormGroup: FormGroup;
  public secondFormGroup: FormGroup;
  public newFormGroupChiTiet: FormGroup;
  public newFormGroupPhieuNhap: FormGroup;
  get ProdVariantName() { return this.newFormGroupChiTiet.get('ProdVariantName'); }
  get Quantity() { return this.newFormGroupChiTiet.get('Quantity'); }
  get ProdVariantPrice() { return this.newFormGroupChiTiet.get('ProdVariantPrice'); }
  get GhiChu() { return this.newFormGroupPhieuNhap.get('GhiChu'); }
  GiaNhapSanPhamBienThes(value) {
    this.newFormGroupChiTiet.get('ProdVariantPrice').setValue(value)
  }
  idncc: any
  getSanPhamNhaCungCap(event) {
    var obj = {
      id: event.target.value
    }
    this.idncc = obj.id
    console.log("object :", obj.id);
    this.service.gettensanphamhttp(obj.id).subscribe(res => {
      this.sanphams = res;
      console.log(this.sanphams);
    });
    this.service.getonenhacungcaphttp(obj.id).subscribe(res => {
      this.motnhacungcap = res;
      console.log("mot nha cung cap", this.motnhacungcap);
    });
  }
  selectedFruit = null;
  getSanPhamBienTheSanPham(event) {
    this.selectedFruit = 'Apple'
    console.log(this.selectedFruit);
    var obj = {
      id: event.target.value
    }
    this.service.gettensanphambienthehttp(obj.id).subscribe(res => {
      this.sanphambienthes = res;
      console.log("san pham bien the", this.sanphambienthes);
      this.GiaNhapSanPhamBienThes(this.sanphambienthes[0].originalPrice)
    });
  }
  onSubmitChiTiet = (data) => {
    this.chitiets.push(data)
    console.log("chi tiet", this.chitiets)
  }
  tongTien() {
    let sum = 0
    for (var i = 0; i < this.chitiets.length; i++) {
      sum = sum + (this.chitiets[i].Quantity * this.chitiets[i].ProdVariantPrice)
    }
    return sum
  }
  public deleteDetail(item: any) {
    for (var index = 0; index < this.chitiets.length; index++) {
      let detail = this.chitiets[index];
      if (detail.ProdVariantName == item.ProdVariantName
        && detail.Quantity == item.Quantity) {
        this.chitiets.splice(index, 1);
      }
    }
  }
  phieunhappost: any
  onSubmit = (data) => {
    this.idUser = localStorage.getItem("idUser")
    this.service.phieunhappost.userId = this.idUser
    this.service.phieunhappost.description = data.GhiChu
    this.service.phieunhappost.totalPrice = this.tongTien()
    this.service.phieunhappost.supplierId = this.idncc
    this.service.phieunhappost.receiptDetails = this.chitiets
    console.log("this. service : ", this.service.phieunhappost);
    this.service.post(this.service.phieunhappost).subscribe(
      result => {
        this.route.navigate(["admin/taophieunhapsuccess"])
      },
      error => {
      }
    )
  }
}