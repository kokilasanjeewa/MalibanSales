import { BaseModel } from './basemodel';
import { Bin } from './bin';
import { StoreArea } from './storearea';

export interface Rack extends BaseModel {
  rackId: number;
  rackName: string;
  rackUid: string;
  rackDescription: string;
  storeArea: StoreArea;
  bin: Bin[];
  numberofShelves: number;
}
