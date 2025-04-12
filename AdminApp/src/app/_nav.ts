import { INavData } from '@coreui/angular';
export const navItems: INavData[] = [
  {
    name:"THỐNG KÊ",
    children:
    [
      {
        name: 'Tổng quan',
        url: '/admin/dashboard',
        icon: 'cil-chart-line',
      },
      {
        name: 'Bán hàng',
        url: '/admin/statistic',
        icon: 'cil-bar-chart',
      },
      // {
      //   name: 'Nhập hàng',
      //   url: '/admin/chartthird',
      //   icon: 'cil-bar-chart',
      // },
    ]
  },
  {
    name:"HÓA ĐƠN / PHIẾU NHẬP",
    children:
    [
      {
        name: 'Hóa đơn bán hàng',
        url: '/admin/hoadons',   
        icon:'cil-notes'
      },
      {
        name:'Phiếu nhập hàng',
        url:'admin/taophieunhap',
        icon:'cil-clipboard'
      },
    ]
  },
  {
    name:"QUẢN LÝ",
    children:
    [
      {
        name: 'Loại',
        url: '/admin/categories',
        icon: 'cil-aperture',
      },
      {
        name: 'Nhà cung cấp',
        url: '/admin/suppliers',
        icon: 'cil-factory',
      },
      {
        name: 'Nhãn hiệu',
        url: '/admin/brands',
        icon: 'cil-apps',
      },
      {
        name: 'Size',
        url: '/admin/sizes',
        icon: 'cil-resize-width',
      },
      {
        name: 'Màu sắc',
        url: '/admin/colors',
        icon: 'cil-drop',
      },
      {
        name: 'Sản phẩm',
        url: '/admin/products',
        icon: 'cil-3d',
      },
      {
        name: 'Sản phẩm biến thể',
        url: '/admin/sanphambienthes',
        icon:'cil-dialpad'
      },
      {
        name:'Người dùng',
        url:'/admin/usermanagers',
        icon:'cil-group'
      },
      {
        name: 'Mã giảm giá',
        url: '/admin/coupons',
        icon: 'cil-tags',
      },
      {
        name:'Blog',
        url:'admin/blogs',
        icon:'cil-cloud'
      },
      {
        name:'Trò chuyện',
        url:'https://app.chatra.io/conversations/mychat',
        icon:'cil-chat-bubble'
      },
    ]
  },
];
