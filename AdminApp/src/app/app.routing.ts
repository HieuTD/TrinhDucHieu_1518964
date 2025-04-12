import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
// Import Containers
import { DefaultLayoutComponent } from './admin/containers';
import { RegistrationFormComponent } from './admin/containers/views/account/registration-form/registration-form.component';
import { AuthGuard } from './admin/containers/views/auth.guard';
import { BrandsComponent } from './admin/containers/views/brands/brands.component';
import { CategoriesComponent } from './admin/containers/views/categories/categories.component';
import { P404Component } from './admin/containers/views/error/404.component';
import { P500Component } from './admin/containers/views/error/500.component';
import { LoginComponent } from './admin/containers/views/account/login/login.component';
import { ProductsComponent } from './admin/containers/views/products/products.component';
import { SizesComponent } from './admin/containers/views/sizes/sizes.component';
// import { ProductdetailComponent } from './admin/containers/views/products/productdetail/productdetail.component';
import { DashboardComponent } from './admin/containers/views/dashboard/dashboard.component';
import { ChartJSComponent } from './admin/containers/views/chartjs/chartjs.component';
// import { UserdetailComponent } from './admin/containers/userdetail/userdetail.component';
import { WidgetsComponent } from './admin/containers/views/widgets/widgets.component';
import { ProfileComponent } from './admin/containers/views/profile/profile.component';
// import { ChartThirdComponent } from './admin/containers/views/chart-third/chart-third.component';
// import { ChatsComponent } from './admin/containers/views/chats/chats.component';
import { BlogsComponent } from './admin/containers/views/blogs/blogs.component';
import { UsermanagersComponent } from './admin/containers/views/usermanagers/usermanagers.component';
import { CouponsComponent } from './admin/containers/views/coupons/coupons.component';
import { ColorsComponent } from './admin/containers/views/colors/colors.component';
import { CreateOrEditSupplierComponent } from './admin/containers/views/suppliers/create-or-edit-supplier/create-or-edit-supplier.component';
import { SuppliersComponent } from './admin/containers/views/suppliers/suppliers.component';
import { ProductVariantsComponent } from './admin/containers/views/product-variants/product-variants.component';
import { CreateOrEditProductComponent } from './admin/containers/views/products/create-or-edit-product/create-or-edit-product.component';
import { OrdersComponent } from './admin/containers/views/orders/orders.component';
import { OrderDetailComponent } from './admin/containers/views/orders/order-detail/order-detail.component';
import { ReceiptsComponent } from './admin/containers/views/receipts/receipts.component';
import { ReceiptSuccessComponent } from './admin/containers/views/receipts/receipt-success/receipt-success.component';
import { CreateReceiptComponent } from './admin/containers/views/receipts/create-receipt/create-receipt.component';
import { ReceiptDetailComponent } from './admin/containers/views/receipts/receipt-detail/receipt-detail.component';
import { StatisticsComponent } from './admin/containers/views/statistics/statistics.component';
// import { DefaultLayoutClientComponent } from './client/containers/default-layout-client/default-layout-client.component';
export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full',
  },
  // {
  //   path:'client',
  //   component:DefaultLayoutClientComponent,
  // },
  {
    path: '404',
    component: P404Component,
    data: {
      title: 'Page 404'
    }
  },
  {
    path: '500',
    component: P500Component,
    data: {
      title: 'Page 500'
    }
  },
  {
    path: 'login',
    component: LoginComponent,
    data: {
      title: 'Login Page'
    }
  },
  {
    path: 'register',
    component: RegistrationFormComponent,
    data: {
      title: 'Register Page'
    }
  },
  {
    path: '',
    component: DefaultLayoutComponent, canActivate: [AuthGuard],
    data: {
      title: 'Home'
    },
    children: [
      {
        path: 'admin/profile',
        component: ProfileComponent
      }
      ,
      {
        path: 'admin/widget',
        component: WidgetsComponent
      },
      {
        path: 'admin/statistic',
        component: StatisticsComponent
      },
      {
        path: 'admin/dashboard',
        component: DashboardComponent, canActivate: [AuthGuard],
      },
      {
        path: 'admin/product/add',
        component: CreateOrEditProductComponent, canActivate: [AuthGuard],
      },
      {
        path: 'admin/product/edit/:id',
        component: CreateOrEditProductComponent, canActivate: [AuthGuard],
      },
      {
        path: 'admin/products',
        component: ProductsComponent, canActivate: [AuthGuard],
      },
      {
        path: 'admin/suppliers',
        component: SuppliersComponent, canActivate: [AuthGuard],
      },
      {
        path: 'admin/supplier',
        component: CreateOrEditSupplierComponent, canActivate: [AuthGuard],
      },
      {
        path: 'admin/categories',
        component: CategoriesComponent, canActivate: [AuthGuard],
      },
      {
        path: 'admin/sanphambienthes',
        component: ProductVariantsComponent, canActivate: [AuthGuard],
      },
      {
        path: 'admin/brands',
        component: BrandsComponent, canActivate: [AuthGuard],
      },
      {
        path: 'admin/coupons',
        component: CouponsComponent, canActivate: [AuthGuard],
      },
      {
        path: 'admin/sizes',
        component: SizesComponent, canActivate: [AuthGuard],
      },
      {
        path: 'admin/colors',
        component: ColorsComponent, canActivate: [AuthGuard],
      },
      {
        path: 'admin/hoadons',
        component: OrdersComponent, canActivate: [AuthGuard],
      },
      {
        path: 'admin/usermanagers',
        component: UsermanagersComponent, canActivate: [AuthGuard],
      },
      {
        path: 'admin/taophieunhap',
        component: ReceiptsComponent, canActivate: [AuthGuard],
      },
      {
        path: 'admin/taophieunhapsuccess',
        component: ReceiptSuccessComponent, canActivate: [AuthGuard],
      },
      {
        path: 'admin/taophieunhap/them',
        component: CreateReceiptComponent, canActivate: [AuthGuard],
      },
      {
        path: 'admin/hoadon/detail/:id',
        component: OrderDetailComponent, canActivate: [AuthGuard],
      },
      {
        path: 'admin/taophieunhap/detail/:id',
        component: ReceiptDetailComponent, canActivate: [AuthGuard],
      },
      {
        path: 'admin/blogs',
        component: BlogsComponent, canActivate: [AuthGuard],
      },
    ]
  },
  { path: '**', component: P404Component }
];
@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: false })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
