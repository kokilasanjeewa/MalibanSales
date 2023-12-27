import { EmailValidator } from '@angular/forms';

export interface Supplier {
  supplierId: number;
  supplierName: string;
  firstName: string;
  lastName: string;
  address1: string;
  phone: string;
  isDeleted: true;
  districtName: string;
  titleName: string;
  provinceName: string;
  cityName: string;
  titleId: number;
  provinceId: number;
  districtId: number;
  cityId: number;
  typeofGoodId: number;
  paymentMethodId: number;
  fax: string,
  email: EmailValidator,
  active: boolean,
  brno: string,
}
