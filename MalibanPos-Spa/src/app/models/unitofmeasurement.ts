import { BaseModel } from './basemodel';

export interface UnitOfMeasurement extends BaseModel {
  unitOfMeasurementId: number;
  unitOfMeasurementUid: string;
  uOM: string;
  description: string;
}
