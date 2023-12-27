import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from './../../environments/environment';
import { CommonService } from './common.service';

@Injectable({
  providedIn: 'root'
})
export class SubcategoryService {
  baseUrl = environment.apiUrl + 'SubCategory/';
  constructor(
    private http: HttpClient,
    private commonService: CommonService
  ) { }
  GetSubcategories(categoryId: any): Observable<any> {
    return this.http.get(this.baseUrl + 'SubCategories/' + categoryId);
  }
}
