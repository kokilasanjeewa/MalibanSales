export interface BaseModel {
  createDate: Date;
  modifiedDate: Date;
  createdBy: string;
  modifiedBy: string;
  active: boolean;
  isDeleted: boolean;
}