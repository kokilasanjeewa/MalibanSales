import { BaseModel } from './basemodel';

export interface Photo extends BaseModel {
  photoId: number;
  url: string;
  description: string;
  dateAdded: string;
  isMain: boolean;
  attachmentPath: string;
  length: number;
  contentType: string;
  fileName: string;
}