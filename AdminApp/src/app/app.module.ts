import { CommonModule, DatePipe } from '@angular/common';
import { NotifierModule } from 'angular-notifier';
import { KonvaModule } from 'ng2-konva';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { NgxPaginationModule } from 'ngx-pagination';
import { MatTabsModule } from '@angular/material/tabs';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatStepperModule } from '@angular/material/stepper';
import { MatSortModule } from '@angular/material/sort';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http'
import { IconModule, IconSetModule, IconSetService } from '@coreui/icons-angular';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { DefaultLayoutComponent } from './admin/containers';
import { P404Component } from './admin/containers/views/error/404.component';
import { P500Component } from './admin/containers/views/error/500.component';
import { LoginComponent } from './admin/containers/views/account/login/login.component';
import { MatDialogModule } from '@angular/material/dialog';
const APP_CONTAINERS = [
  DefaultLayoutComponent
];
import {
  AppAsideModule,
  AppBreadcrumbModule,
  AppHeaderModule,
  AppFooterModule,
  AppSidebarModule,
} from '@coreui/angular';
// Import routing module
import { AppRoutingModule } from './app.routing';
// Import 3rd party components
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TabsModule } from 'ngx-bootstrap/tabs';
// import { OrdersTableComponent } from './admin/containers/orders-table/orders-table.component';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { RegistrationFormComponent } from './admin/containers/views/account/registration-form/registration-form.component';
import { UserService } from './admin/containers/views/account/user.service';
// import { UserdetailComponent } from './admin/containers/userdetail/userdetail.component';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { BrandsComponent } from './admin/containers/views/brands/brands.component';
import { ColorPickerModule } from 'ngx-color-picker';
import { SizesComponent } from './admin/containers/views/sizes/sizes.component';
import { ProductsComponent } from './admin/containers/views/products/products.component';
// import { ImagesmodelComponent } from './admin/containers/views/products/imagesmodel/imagesmodel.component';
import { CategoriesComponent } from './admin/containers/views/categories/categories.component';
import { AuthGuard } from './admin/containers/views/auth.guard';
import { ModalComponent } from './admin/containers/modal/modal.component';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { ChartsModule } from 'ng2-charts';
import { ToastrModule } from 'ngx-toastr';
import { ProfileComponent } from './admin/containers/views/profile/profile.component';
import { DashboardComponent } from './admin/containers/views/dashboard/dashboard.component';
import * as FusionCharts from "fusioncharts";
import * as charts from "fusioncharts/fusioncharts.charts";
import * as FusionTheme from "fusioncharts/themes/fusioncharts.theme.fusion";
import { FusionChartsModule } from 'angular-fusioncharts';
import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
// import { ChartThirdComponent } from './admin/containers/views/chart-third/chart-third.component';
// Report viewer
import '@boldreports/javascript-reporting-controls/Scripts/bold.report-viewer.min';
// data-visualization
import '@boldreports/javascript-reporting-controls/Scripts/data-visualization/ej.bulletgraph.min';
import '@boldreports/javascript-reporting-controls/Scripts/data-visualization/ej.chart.min';
// import { ChatsComponent } from './admin/containers/views/chats/chats.component';
import { BlogsComponent } from './admin/containers/views/blogs/blogs.component';
import { UsermanagersComponent } from './admin/containers/views/usermanagers/usermanagers.component';
import { CreateOrEditCategoryComponent } from './admin/containers/views/categories/create-or-edit-category/create-or-edit-category.component';
import { CreateOrEditBrandComponent } from './admin/containers/views/brands/create-or-edit-brand/create-or-edit-brand.component';
import { CreateOrEditSizeComponent } from './admin/containers/views/sizes/create-or-edit-size/create-or-edit-size.component';
import { CouponsComponent } from './admin/containers/views/coupons/coupons.component';
import { CreateOrEditCouponComponent } from './admin/containers/views/coupons/create-or-edit-coupon/create-or-edit-coupon.component';
import { ColorsComponent } from './admin/containers/views/colors/colors.component';
import { CreateOrEditColorComponent } from './admin/containers/views/colors/create-or-edit-color/create-or-edit-color.component';
import { SuppliersComponent } from './admin/containers/views/suppliers/suppliers.component';
import { CreateOrEditSupplierComponent } from './admin/containers/views/suppliers/create-or-edit-supplier/create-or-edit-supplier.component';
import { CreateOrEditProductVariantComponent } from './admin/containers/views/product-variants/create-or-edit-product-variant/create-or-edit-product-variant.component';
import { ProductVariantsComponent } from './admin/containers/views/product-variants/product-variants.component';
import { CreateOrEditBlogComponent } from './admin/containers/views/blogs/create-or-edit-blog/create-or-edit-blog.component';
import { CreateOrEditProductComponent } from './admin/containers/views/products/create-or-edit-product/create-or-edit-product.component';
import { OrdersComponent } from './admin/containers/views/orders/orders.component';
import { OrderDetailComponent } from './admin/containers/views/orders/order-detail/order-detail.component';
import { EditOrderComponent } from './admin/containers/views/orders/edit-order/edit-order.component';
import { ReceiptsComponent } from './admin/containers/views/receipts/receipts.component';
import { CreateReceiptComponent } from './admin/containers/views/receipts/create-receipt/create-receipt.component';
import { ReceiptSuccessComponent } from './admin/containers/views/receipts/receipt-success/receipt-success.component';
import { ReceiptDetailComponent } from './admin/containers/views/receipts/receipt-detail/receipt-detail.component';
import { StatisticsComponent } from './admin/containers/views/statistics/statistics.component';
FusionChartsModule.fcRoot(FusionCharts, charts, FusionTheme);
@NgModule({
  imports: [
    FusionChartsModule,
    KonvaModule,
    ChartsModule,
    HttpClientModule,
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    AppAsideModule,
    AppBreadcrumbModule.forRoot(),
    AppFooterModule,
    AppHeaderModule,
    AppSidebarModule,
    NgxPaginationModule,
    BsDropdownModule.forRoot(),
    TabsModule.forRoot(),
    IconModule,
    IconSetModule.forRoot(),
    MatTableModule,
    MatPaginatorModule,
    FormsModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatExpansionModule,
    MatSlideToggleModule,
    ColorPickerModule,
    ToastrModule.forRoot({
      timeOut: 1000,
      progressBar: true,
      progressAnimation: 'increasing'
    }
    ),
    BrowserAnimationsModule,
    //
    NotifierModule,
    MatDatepickerModule,
    CommonModule,
    MatDialogModule,
    MatTableModule,
    MatIconModule,
    MatButtonModule,
    MatSnackBarModule,
    MatTabsModule,
    MatFormFieldModule,
    MatInputModule,
    MatPaginatorModule,
    MatSelectModule,
    MatStepperModule,
    FormsModule,
    ReactiveFormsModule,
    MatSortModule,
  ],
  declarations: [
    DashboardComponent,
    AppComponent,
    ...APP_CONTAINERS,
    P404Component,
    P500Component,
    LoginComponent,
    // OrdersTableComponent,
    // UserdetailComponent,
    BrandsComponent,
    ColorsComponent,
    CreateOrEditColorComponent,
    SizesComponent,
    CreateOrEditCategoryComponent,
    OrdersComponent,
    UsermanagersComponent,
    CreateOrEditBrandComponent,
    ProductsComponent,
    CreateOrEditProductComponent,
    RegistrationFormComponent,
    // ImagesmodelComponent,
    CategoriesComponent,
    CreateOrEditSizeComponent,
    CreateOrEditProductVariantComponent,
    ProductVariantsComponent,
    ModalComponent,
    ProfileComponent,
    OrderDetailComponent,
    DefaultLayoutComponent,
    StatisticsComponent,
    CreateOrEditSupplierComponent,
    SuppliersComponent,
    CreateOrEditCouponComponent,
    CouponsComponent,
    ReceiptsComponent,
    CreateReceiptComponent,
    ReceiptSuccessComponent,
    ReceiptDetailComponent,
    // ChartThirdComponent,
    EditOrderComponent,
    // ChatsComponent,
    BlogsComponent,
    CreateOrEditBlogComponent,
  ],
  providers: [
    IconSetService,
    UserService,
    [AuthGuard],
    LoginComponent,
    DatePipe,
    {
      provide: STEPPER_GLOBAL_OPTIONS,
      useValue: { showError: true }
    }
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  bootstrap: [AppComponent]
})
export class AppModule { }
