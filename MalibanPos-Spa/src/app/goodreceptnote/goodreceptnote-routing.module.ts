import { GoodreceptnotereportComponent } from './goodreceptnotereport/goodreceptnotereport.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { GoodReceiptDetailResolver } from '../_resolvers/goodreceiptdetail.resolver';
import { GoodReceiptListResolver } from '../_resolvers/goodreceiptlist.resolver';
import { CreategrnComponent } from './creategrn/creategrn.component';
import { GrndetailComponent } from './grndetail/grndetail.component';
import { GrnlistComponent } from './grnlist/grnlist.component';
import { EditgrnComponent } from './editgrn/editgrn.component';


const routes: Routes = [
  {
    path: 'grnlist',
    component: GrnlistComponent,
    resolve: { goodReceptNoteHeaders: GoodReceiptListResolver }

  },
  {
    path: 'grndetail/:goodReceptNoteHeaderId',
    component: GrndetailComponent,
    resolve: { goodReceptNoteDetails: GoodReceiptDetailResolver }
  },
  {
    path: 'creategrn',
    component: CreategrnComponent
  },
  {
    path: 'editgrn/:receiptOrderId',
    component: EditgrnComponent
  },
  {
    path: 'goodreceptnotereport/:receiptOrderId',
    component: GoodreceptnotereportComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GoodreceptnoteRoutingModule { }
