import { GoodreceptnoteModule } from './goodreceptnote/goodreceptnote.module';
import { AuthGuard } from './_guards/auth.guard';
import { NgModule } from '@angular/core';
import { Routes, RouterModule, PreloadAllModules } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { CustomPreloadingStrategyService } from './_services/custom-preloading-strategy.service';
import { SigninComponent } from './authentication/signin/signin.component';

const routes: Routes = [
  {
    path: '',
    component: SigninComponent,
    data: { showHeader: false, showSidebar: false, showFooter: false },
  },
  {
    path: 'authentication',
    loadChildren: () =>
      import('./authentication/authentication.module').then(
        (m) => m.AuthenticationModule
      ),
    data: { showHeader: false, showSidebar: false, showFooter: false },
  },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      {
        path: '',
        redirectTo: 'home',
        pathMatch: 'full',
        data: { showHeader: false, showSidebar: false, showFooter: false },
      },
      {
        path: 'supplier',
        loadChildren: () =>
          import('./supplier/supplier.module').then((m) => m.SupplierModule),
        data: { showHeader: true, showSidebar: true, preload: true },
      },
      {
        path: 'customer',
        loadChildren: () =>
          import('./customer/customer.module').then((m) => m.CustomerModule),
        data: { showHeader: true, showSidebar: true, preload: true, roles: [ 'Manager'] },
      },
      {
        path: 'product',
        loadChildren: () =>
          import('./product/product.module').then((m) => m.ProductModule),
        data: { showHeader: true,showSidebar: true,claims: ['CanAccessProductList','CanAccessAddProduct']},
        // canActivate: [AuthGuard],
      },
      {
        path: 'pos',
        loadChildren: () => import('./pos/pos.module').then((m) => m.PosModule),
        data: {
          showHeader: false,
          showSidebar: false,
          showFooter: false,
          roles: ['User'],
        },
      },
      {
        path: 'stock',
        loadChildren: () =>
          import('./stock/stock.module').then((m) => m.StockModule),
        data: { roles: ['Administrator', 'Manager', 'User'] },
      },
      {
        path: 'supplier',
        loadChildren: () =>
          import('./supplier/supplier.module').then((m) => m.SupplierModule),
        data: { roles: ['Administrator', 'Manager', 'User'] },
      },
      {
        path: 'price',
        loadChildren: () =>
          import('./price/price.module').then((m) => m.PriceModule),
        data: { roles: ['Administrator', 'Manager'] },
      },
      {
        path: 'purchase',
        loadChildren: () =>
          import('./purchase/purchase.module').then((m) => m.PurchaseModule),
        data: { roles: ['Administrator', 'Manager'] },
      },
      {
        path: 'request',
        loadChildren: () =>
          import('./request/request.module').then((m) => m.RequestModule),
      },
      {
        path: 'grn',
        loadChildren: () =>
          import('./goodreceptnote/goodreceptnote.module').then(
            (m) => m.GoodreceptnoteModule
          ),
        data: { roles: ['Administrator', 'Manager'] },
      },
      {
        path: 'supplierproduct',
        loadChildren: () =>
          import('./supplierproduct/supplierproduct.module').then(
            (m) => m.SupplierproductModule
          ),
      },
      {
        path: 'supplierprice',
        loadChildren: () =>
          import('./supplierprice/supplierprice.module').then(
            (m) => m.SupplierpriceModule
          ),
        data: { roles: ['Administrator', 'Manager'] ,claims: ['CanAccessSupplierPriceList','CanAccessAddSupplierPrice']},
        //canLoad: [AuthGuard],
      },
      {
        path: 'report',
        loadChildren: () =>
          import('./report/report.module').then((m) => m.ReportModule),
      },
      {
        path: 'setting',
        loadChildren: () =>
          import('./setting/setting.module').then((m) => m.SettingModule),
      },
      {
        path: 'manage',
        loadChildren: () =>
          import('./user/user.module').then((m) => m.UserModule),
      },
    ],
  },
  {
    path: 'dashboard',
    loadChildren: () =>
      import('./dashboard/dashboard.module').then((m) => m.DashboardModule),
  },
  { path: 'setting', loadChildren: () => import('./setting/setting.module').then(m => m.SettingModule) },
  { path: '**', redirectTo: '/', pathMatch: 'full' },
];

@NgModule({
  providers: [ AuthGuard ],
  imports: [
    RouterModule.forRoot(routes, {
      // https://blog.bitsrc.io/preloading-strategies-boost-up-angular-app-loading-time-ffb19da63155
      //preloadingStrategy: PreloadAllModules //all routing module preloading
      preloadingStrategy: CustomPreloadingStrategyService, //custom routing module preloading
    }),
  ],
  exports: [RouterModule],
})
export class AppRoutingModule {}
