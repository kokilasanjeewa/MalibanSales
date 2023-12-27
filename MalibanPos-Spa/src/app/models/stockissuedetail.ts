import { BaseModel } from './basemodel';
import { Bin } from './bin';
import { GoodReceptNoteDetail } from './goodreceptnotedetail';
import { Product } from './product';
import { StockIssueHeader } from './stockissueheader';

export interface StockIssueDetail extends BaseModel {
  stockIssueDetailId: number;
  stockIssueDetailUid: string;
  issueQuantity: number;
  productId: number;
  product: Product;
  binId: number;
  bin: Bin;
  goodReceptNoteDetailId: number;
  goodReceptNoteDetail: GoodReceptNoteDetail;
  stockIssueHeaderId: number;
  stockIssueHeader: StockIssueHeader;
}
