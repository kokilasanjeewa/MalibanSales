import { NavigationService } from './_services/navigation.service';
import { PosModule } from './pos/pos.module';
import { SupplierModule } from './supplier/supplier.module';
import { SupplierService } from 'src/app/_services/supplier.service';
import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { AuthenticationModule } from './authentication/authentication.module';
import { AppRoutingModule } from './app-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from './_services/auth.service';
import { HomeComponent } from './home/home.component';
import { ProductModule } from './product/product.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { LayoutModule } from './layout/layout.module';
import { MaterialModule } from './material.module';
import { CommonService } from './_services/common.service';
import { StockModule } from './stock/stock.module';
import { SupplierListResolver } from './_resolvers/supplierlist.resolver';
import { PriceModule } from './price/price.module';
import { PurchaseModule } from './purchase/purchase.module';
import { GoodreceptnoteModule } from './goodreceptnote/goodreceptnote.module';
import { RequestModule } from './request/request.module';
import { CartService } from './_services/cart.service';
import { SupplierproductModule } from './supplierproduct/supplierproduct.module';
import { SupplierpriceModule } from './supplierprice/supplierprice.module';
import { JwtModule } from "@auth0/angular-jwt";
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule, ToastNoAnimation, ToastNoAnimationModule } from 'ngx-toastr';
import { PinkToast } from './pink.toast';
import { NotyfToast } from './notyf.toast';
import { ReportModule } from './report/report.module';
import { NgSelectModule } from '@ng-select/ng-select';
import { CacheInterceptor } from './_services/cache.interceptor';
import { ModalModule } from 'ngx-bootstrap/modal';
import { NgxPrintModule } from 'ngx-print';
import { ThermalPrintModule } from 'ng-thermal-print';
import { ReversePipe } from './_custompipe/reverse.pipe';
import { PricemodalComponent } from './pos/pricemodal/pricemodal.component';
import { IconSpacerModule } from './iconspacer.module';
import { UserModule } from './user/user.module';
import { SharedModule } from './shared/shared.module';
import { CustomerModule } from './customer/customer.module';
import { WebsocketService } from './_services/websocket.service';
import { DashboardModule } from './dashboard/dashboard.module';
import { ConnectionService } from './_services/connection.service';
import { ConnectionInterceptor } from './_services/connection.interceptor';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    PinkToast,
    NotyfToast,
  ],
  imports: [
    CommonModule,
    IconSpacerModule,
    BrowserModule,
    HttpClientModule,
    JwtModule.forRoot({
      config: {
        // tslint:disable-next-line: object-literal-shorthand
        // tslint:disable-next-line: arrow-return-shorthand
        tokenGetter: () => {
          return localStorage.getItem('token');
        },
        whitelistedDomains: ['localhost:5000', 'localhost:4200'],
        blacklistedRoutes: ['localhost:5000/api/auth'],
        skipWhenExpired: true,
      }
    }),
    BrowserAnimationsModule,
    ToastNoAnimationModule.forRoot(),// https://www.npmjs.com/package/ngx-toastr
    ToastrModule.forRoot({
    //  toastComponent: PinkToast,
      timeOut: 1000,
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
      countDuplicates: true,
      newestOnTop: true,
      autoDismiss: true,
      closeButton: true,
    }),
    BsDropdownModule.forRoot(),
    ModalModule.forRoot(),
    AppRoutingModule,
    AuthenticationModule,
    NgSelectModule,
    FormsModule,
    ReactiveFormsModule,
    ProductModule,
    MaterialModule,
    LayoutModule,
    StockModule,
    PriceModule,
    SupplierModule,
    PurchaseModule,
    PosModule,
    GoodreceptnoteModule,
    RequestModule,
    SupplierproductModule,
    SupplierpriceModule,
    DashboardModule,
    ReportModule,
    NgxPrintModule,
    ThermalPrintModule,
    UserModule,
    SharedModule,
    CustomerModule,

  ],
  entryComponents: [PinkToast, NotyfToast],
  providers: [AuthService, SupplierService, CommonService, NavigationService, CartService, ErrorInterceptorProvider,ConnectionInterceptor, SupplierListResolver,ConnectionService /*, { provide: HTTP_INTERCEPTORS, useClass: CacheInterceptor, multi: true }*/

],
  bootstrap: [AppComponent],
})
export class AppModule {

}

