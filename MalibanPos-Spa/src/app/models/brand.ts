import { BaseModel } from './basemodel';
import { Photo } from './photo';

export interface Brand extends BaseModel {
  brandId: number;
  brandName: string;
  brandDescription: string;
  brandUid: string;
  photoId: number | null;
  photo: Photo;
}