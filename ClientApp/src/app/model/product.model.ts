export interface Product {
    id?: number;
    name?: string;
    discount?: number;
    description?: string;
    tag?: string;
    image?: string;
    price?: number;
    status?: string;
    trangThaiSanPhamThietKe?:string;
    isFeatured?:string;
    huongDan?:string;
    material?:string;
    brandName?:string;
    categoryName?:string;
    categoryId?:number;
    gender?:number;
    productImages:imageSanPhams[];
    sanPhamBienThes:sanPhamBienThes[];
    like?:number;
}
export interface sanPhamBienThes {
    id?:number;
    prodId?:number;
    colorName?:string;
    stock?:number;
    sizeName?:string;
}
export interface imageSanPhams {
    id?:number;
    imageName?:string;
    idSanPham?:number;
}

export interface ApiResponse {
    urlVNPay?: string;
  }